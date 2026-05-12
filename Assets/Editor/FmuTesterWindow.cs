// Assets/Editor/FmuTesterWindow.cs
// ============================================================
// Generic FMU Tester — Speedma
// ============================================================
// Opens via: Speedma > FMU Tester
//
// v3 changes:
//   - Outputs section: each Real output has a eye-toggle checkbox
//     to show/hide it from the live plot independently
//   - Conflict detection: if ALL boolean inputs are true simultaneously,
//     shows a yellow warning (e.g. sw_carga + sw_descarga both on)
//   - Plot only renders enabled (visible) outputs
//   - History is still collected for all outputs (re-enable = history shown)
// ============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Speedma.Editor
{
    public class FmuTesterWindow : EditorWindow
    {
        // ── Config ─────────────────────────────────────────────────────────
        private string _backendUrl = "http://localhost:8000";
        private float  _dt         = 0.02f;

        // ── FMU list ───────────────────────────────────────────────────────
        private List<string> _availableFmus  = new();
        private int          _selectedFmuIdx = 0;

        // ── Active sessions ────────────────────────────────────────────────
        private List<FmuSession> _sessions = new();

        // ── Scroll ────────────────────────────────────────────────────────
        private Vector2 _scrollPos;

        // ── Styles ────────────────────────────────────────────────────────
        private bool     _stylesReady;
        private GUIStyle _titleStyle, _sectionStyle, _okStyle, _errStyle, _warnStyle, _dimStyle;

        // ── Plot colours ──────────────────────────────────────────────────
        private static readonly Color[] PlotColors =
        {
            new(0.2f, 0.9f, 0.3f),
            new(0.3f, 0.6f, 1.0f),
            new(1.0f, 0.7f, 0.2f),
            new(1.0f, 0.3f, 0.4f),
            new(0.8f, 0.3f, 1.0f),
            new(0.3f, 0.9f, 0.9f),
            new(1.0f, 0.5f, 0.8f),
        };

        // ══════════════════════════════════════════════════════════════════
        //  Session data model
        // ══════════════════════════════════════════════════════════════════
        private class FmuSession
        {
            public string FmuName;
            public string SessionId  = "";
            public string Status     = "Starting...";
            public bool   Requesting = false;
            public bool   AutoStep   = false;
            public double LastAutoStep;
            public float  SimTime    = 0f;
            public bool   Collapsed  = false;

            // Variable metadata  { name -> VarMeta }
            public Dictionary<string, VarMeta> Variables   = new();
            // Current INPUT values set by user
            public Dictionary<string, object>  InputValues = new();
            // Last OUTPUT values received
            public Dictionary<string, float>   OutputValues = new();

            // Per-output: is it visible on the plot?
            public Dictionary<string, bool> OutputVisible = new();

            // Plot history (always collected, visibility is render-only)
            public Dictionary<string, List<float>> PlotTimes  = new();
            public Dictionary<string, List<float>> PlotValues = new();
            public const int MaxSamples = 500;

            // Color index assigned at variable-load time (stable)
            public Dictionary<string, int> OutputColorIdx = new();
        }

        private class VarMeta
        {
            public string Type;       // Real | Boolean | Integer | String
            public string Causality;  // input | output | local | parameter | independent
        }

        // ══════════════════════════════════════════════════════════════════

        [MenuItem("Speedma/FMU Tester")]
        public static void ShowWindow()
        {
            var w = GetWindow<FmuTesterWindow>("FMU Tester");
            w.minSize = new Vector2(520, 620);
        }

        private void OnEnable()  => EditorApplication.update += Tick;
        private void OnDisable() => EditorApplication.update -= Tick;

        // ── Auto-step ─────────────────────────────────────────────────────
        private void Tick()
        {
            const double interval = 0.05;
            double now = EditorApplication.timeSinceStartup;
            foreach (var s in _sessions)
            {
                if (!s.AutoStep || s.Requesting || string.IsNullOrEmpty(s.SessionId)) continue;
                if (now - s.LastAutoStep >= interval)
                {
                    s.LastAutoStep = now;
                    _ = StepAsync(s);
                }
            }
        }

        // ── Styles ────────────────────────────────────────────────────────
        private void EnsureStyles()
        {
            if (_stylesReady) return;
            _stylesReady  = true;
            _titleStyle   = new GUIStyle(EditorStyles.boldLabel) { fontSize = 13, alignment = TextAnchor.MiddleCenter };
            _sectionStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 11 };
            _okStyle      = new GUIStyle(EditorStyles.label) { normal = { textColor = new Color(0.2f, 0.85f, 0.3f) } };
            _errStyle     = new GUIStyle(EditorStyles.label) { normal = { textColor = new Color(0.95f, 0.25f, 0.25f) } };
            _warnStyle    = new GUIStyle(EditorStyles.label) { normal = { textColor = new Color(1.0f,  0.85f, 0.1f)  }, fontStyle = FontStyle.Bold };
            _dimStyle     = new GUIStyle(EditorStyles.label) { normal = { textColor = new Color(0.55f, 0.55f, 0.55f) } };
        }

        // ══════════════════════════════════════════════════════════════════
        //  OnGUI
        // ══════════════════════════════════════════════════════════════════
        private void OnGUI()
        {
            EnsureStyles();
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            GUILayout.Space(6);
            GUILayout.Label("Speedma — FMU Tester", _titleStyle);
            GUILayout.Space(2);
            HR();

            // ── Backend config ───────────────────────────────────────────
            GUILayout.Label("Backend", _sectionStyle);
            _backendUrl = EditorGUILayout.TextField("URL", _backendUrl);
            _dt         = EditorGUILayout.Slider("Step size dt (s)", _dt, 0.001f, 0.5f);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("🔄  Refresh FMU List", GUILayout.Height(26))) _ = RefreshFmuListAsync();
                if (GUILayout.Button("❤  Health Check",      GUILayout.Height(26))) _ = HealthCheckAsync();
            }

            HR();

            // ── Session launcher ─────────────────────────────────────────
            GUILayout.Label("Launch FMU Session", _sectionStyle);
            if (_availableFmus.Count == 0)
            {
                EditorGUILayout.HelpBox("No FMUs found. Click \"Refresh FMU List\" with the backend running.", MessageType.Info);
            }
            else
            {
                _selectedFmuIdx = EditorGUILayout.Popup("FMU", _selectedFmuIdx, _availableFmus.ToArray());
                if (GUILayout.Button("▶  Start Session for " + _availableFmus[_selectedFmuIdx], GUILayout.Height(28)))
                    _ = StartSessionAsync(_availableFmus[_selectedFmuIdx]);
            }

            HR();

            // ── Active sessions ──────────────────────────────────────────
            if (_sessions.Count == 0)
                GUILayout.Label("No active sessions.", EditorStyles.centeredGreyMiniLabel);
            else
                for (int i = _sessions.Count - 1; i >= 0; i--)
                    DrawSession(_sessions[i], i);

            GUILayout.Space(10);
            EditorGUILayout.EndScrollView();
        }

        // ══════════════════════════════════════════════════════════════════
        //  Session panel
        // ══════════════════════════════════════════════════════════════════
        private void DrawSession(FmuSession s, int idx)
        {
            // Header
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                s.Collapsed = !EditorGUILayout.Foldout(!s.Collapsed,
                    $"  {s.FmuName}  |  t = {s.SimTime:F3} s", true);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("✕ Close", EditorStyles.toolbarButton, GUILayout.Width(64)))
                {
                    _ = StopSessionAsync(s);
                    _sessions.RemoveAt(idx);
                    return;
                }
            }

            if (s.Collapsed) return;

            EditorGUI.indentLevel++;

            EditorGUILayout.LabelField("Session ID", string.IsNullOrEmpty(s.SessionId) ? "—" : s.SessionId);
            bool isErr = s.Status.StartsWith("Error", StringComparison.OrdinalIgnoreCase);
            GUILayout.Label(s.Status, isErr ? _errStyle : _okStyle);

            if (string.IsNullOrEmpty(s.SessionId)) { EditorGUI.indentLevel--; HR(); return; }

            // Controls
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("⏩ Step",    GUILayout.Height(24))) _ = StepAsync(s);
                string lbl = s.AutoStep ? "⏸ Stop Auto" : "⏯ Auto-Step";
                if (GUILayout.Button(lbl,          GUILayout.Height(24))) s.AutoStep = !s.AutoStep;
                if (GUILayout.Button("🔄 Reset",   GUILayout.Height(24))) _ = ResetAsync(s);
            }

            GUILayout.Space(4);

            // ── Inputs ────────────────────────────────────────────────────
            var inputVars = s.Variables.Where(kv => kv.Value.Causality == "input").ToList();
            if (inputVars.Count > 0)
            {
                GUILayout.Label("Inputs", _sectionStyle);

                // Conflict detection: all boolean inputs active at once?
                var boolInputs = inputVars.Where(kv => kv.Value.Type == "Boolean").ToList();
                bool allBoolActive = boolInputs.Count > 1
                    && boolInputs.All(kv => s.InputValues.TryGetValue(kv.Key, out var v) && Convert.ToBoolean(v));
                if (allBoolActive)
                    EditorGUILayout.HelpBox(
                        "⚠  All boolean inputs are active simultaneously — this may cause numerical instability (conflicting states).",
                        MessageType.Warning);

                foreach (var kv in inputVars)
                {
                    string name = kv.Key;
                    var    meta = kv.Value;
                    switch (meta.Type)
                    {
                        case "Boolean":
                        {
                            bool cur  = s.InputValues.TryGetValue(name, out var v) && Convert.ToBoolean(v);
                            bool next = EditorGUILayout.Toggle(name, cur);
                            s.InputValues[name] = next;
                            break;
                        }
                        case "Integer":
                        {
                            int cur  = s.InputValues.TryGetValue(name, out var v) ? Convert.ToInt32(v) : 0;
                            int next = EditorGUILayout.IntField(name, cur);
                            s.InputValues[name] = next;
                            break;
                        }
                        default:
                        {
                            float cur  = s.InputValues.TryGetValue(name, out var v) ? Convert.ToSingle(v) : 0f;
                            float next = EditorGUILayout.FloatField(name, cur);
                            s.InputValues[name] = next;
                            break;
                        }
                    }
                }
            }

            // ── Outputs (with eye toggle) ──────────────────────────────────
            if (s.OutputValues.Count > 0)
            {
                GUILayout.Space(4);
                GUILayout.Label("Outputs", _sectionStyle);

                foreach (var kv in s.OutputValues)
                {
                    string name    = kv.Key;
                    bool   visible = !s.OutputVisible.ContainsKey(name) || s.OutputVisible[name];
                    bool   isReal  = s.PlotTimes.ContainsKey(name);

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        // Eye toggle (only for Real outputs that can be plotted)
                        if (isReal)
                        {
                            bool next = EditorGUILayout.Toggle(visible, GUILayout.Width(16));
                            if (next != visible) s.OutputVisible[name] = next;

                            // Colored square matching plot color
                            int ci = s.OutputColorIdx.TryGetValue(name, out var c) ? c : 0;
                            Color col = PlotColors[ci % PlotColors.Length];
                            var sqRect = GUILayoutUtility.GetRect(12, 12, GUILayout.Width(12), GUILayout.Height(12));
                            sqRect.y += 3;
                            EditorGUI.DrawRect(sqRect, visible ? col : new Color(col.r, col.g, col.b, 0.25f));
                            GUILayout.Space(4);
                        }
                        else
                        {
                            GUILayout.Space(34); // align with real outputs
                        }

                        EditorGUILayout.LabelField(name, visible ? $"{kv.Value:F5}" : "—",
                                                   visible ? EditorStyles.label : _dimStyle);
                    }
                }
            }

            // ── Live plot ─────────────────────────────────────────────────
            bool anyVisible = s.PlotTimes.Keys.Any(k =>
                s.PlotTimes[k].Count >= 2 &&
                (!s.OutputVisible.ContainsKey(k) || s.OutputVisible[k]));

            if (anyVisible)
            {
                GUILayout.Space(4);
                GUILayout.Label("Live Plot", _sectionStyle);
                Rect pr = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none,
                    GUILayout.ExpandWidth(true), GUILayout.Height(170));
                DrawPlot(pr, s);
            }

            EditorGUI.indentLevel--;
            HR();
        }

        // ══════════════════════════════════════════════════════════════════
        //  Plot
        // ══════════════════════════════════════════════════════════════════
        private void DrawPlot(Rect r, FmuSession s)
        {
            EditorGUI.DrawRect(r, new Color(0.1f, 0.1f, 0.1f));

            float tMax = s.SimTime > 1f ? s.SimTime : 1f;

            // Axis range — only from visible outputs
            float vMin =  float.MaxValue;
            float vMax = -float.MaxValue;
            foreach (var name in s.PlotValues.Keys)
            {
                if (s.OutputVisible.TryGetValue(name, out var vis) && !vis) continue;
                foreach (var v in s.PlotValues[name]) { if (v < vMin) vMin = v; if (v > vMax) vMax = v; }
            }
            if (vMin >= vMax || vMin == float.MaxValue) { vMin = -1; vMax = 1; }
            float pad = (vMax - vMin) * 0.08f;
            vMin -= pad; vMax += pad;

            // Grid
            Handles.color = new Color(0.22f, 0.22f, 0.22f);
            for (int g = 0; g <= 4; g++)
            {
                float y   = r.y + r.height * (1f - g / 4f);
                float val = Mathf.Lerp(vMin, vMax, g / 4f);
                Handles.DrawLine(new Vector3(r.x, y), new Vector3(r.xMax, y));
                GUI.color = Color.gray;
                GUI.Label(new Rect(r.x + 2, y - 8, 62, 16), val.ToString("F2"));
            }
            GUI.color = Color.white;

            // Curves
            int legendRow = 0;
            foreach (var name in s.PlotTimes.Keys)
            {
                bool visible = !s.OutputVisible.ContainsKey(name) || s.OutputVisible[name];
                int  ci      = s.OutputColorIdx.TryGetValue(name, out var c) ? c : 0;
                Color col    = PlotColors[ci % PlotColors.Length];

                var times  = s.PlotTimes[name];
                var values = s.PlotValues[name];

                if (visible && times.Count >= 2)
                {
                    Handles.color = col;
                    for (int i = 1; i < times.Count; i++)
                    {
                        float x0 = r.x + Mathf.InverseLerp(0, tMax, times[i-1]) * r.width;
                        float y0 = r.y + (1f - Mathf.InverseLerp(vMin, vMax, values[i-1])) * r.height;
                        float x1 = r.x + Mathf.InverseLerp(0, tMax, times[i])   * r.width;
                        float y1 = r.y + (1f - Mathf.InverseLerp(vMin, vMax, values[i]))   * r.height;
                        Handles.DrawLine(new Vector3(x0, y0), new Vector3(x1, y1));
                    }
                }

                // Legend (always shown, dimmed if hidden)
                float ly = r.y + 4 + legendRow * 16;
                Color legendCol = visible ? col : new Color(col.r, col.g, col.b, 0.2f);
                EditorGUI.DrawRect(new Rect(r.xMax - 130, ly, 10, 10), legendCol);
                GUI.color = visible ? Color.white : new Color(1,1,1,0.3f);
                GUI.Label(new Rect(r.xMax - 116, ly - 2, 114, 14), name);
                GUI.color = Color.white;
                legendRow++;
            }

            // Axis labels
            GUI.color = Color.gray;
            GUI.Label(new Rect(r.x + 64,      r.yMax - 14, 40, 14), "0 s");
            GUI.Label(new Rect(r.xMax - 44,   r.yMax - 14, 44, 14), $"{tMax:F1} s");
            GUI.color = Color.white;
        }

        // ══════════════════════════════════════════════════════════════════
        //  HTTP actions
        // ══════════════════════════════════════════════════════════════════

        private async Task RefreshFmuListAsync()
        {
            string json = await GetAsync($"{_backendUrl}/sim/list");
            if (string.IsNullOrEmpty(json)) return;
            _availableFmus  = ParseStringArray(json, "fmus");
            _selectedFmuIdx = 0;
            Repaint();
        }

        private async Task HealthCheckAsync()
        {
            string json = await GetAsync($"{_backendUrl}/sim/health");
            Debug.Log($"[FMU Tester] Health: {json}");
        }

        private async Task StartSessionAsync(string fmuName)
        {
            var s = new FmuSession { FmuName = fmuName };
            _sessions.Add(s);
            Repaint();

            string json = await PostAsync($"{_backendUrl}/sim/start", $"{{\"fmu_name\":\"{fmuName}\"}}");
            if (string.IsNullOrEmpty(json)) { s.Status = "Error: backend unreachable"; Repaint(); return; }

            s.SessionId = ParseString(json, "session_id");
            s.Status    = $"Session active";
            Repaint();
            await FetchVariablesAsync(s);
        }

        private async Task FetchVariablesAsync(FmuSession s)
        {
            string json = await GetAsync($"{_backendUrl}/sim/variables/{s.SessionId}");
            if (string.IsNullOrEmpty(json)) return;

            s.Variables = ParseVariables(json);

            // Init input defaults
            foreach (var kv in s.Variables)
            {
                if (kv.Value.Causality != "input") continue;
                s.InputValues.TryAdd(kv.Key,
                    kv.Value.Type == "Boolean" ? (object)false :
                    kv.Value.Type == "Integer" ? (object)0     : (object)0f);
            }

            // Init plot series + assign stable color index
            int ci = 0;
            foreach (var kv in s.Variables)
            {
                if (kv.Value.Causality != "output" && kv.Value.Causality != "local") continue;
                if (kv.Value.Type != "Real") continue;
                s.PlotTimes[kv.Key]      = new List<float>();
                s.PlotValues[kv.Key]     = new List<float>();
                s.OutputVisible[kv.Key]  = true;   // visible by default
                s.OutputColorIdx[kv.Key] = ci++;
            }

            s.Status = $"Ready — {s.Variables.Count} variables";
            Repaint();
        }

        private async Task StepAsync(FmuSession s)
        {
            if (s.Requesting || string.IsNullOrEmpty(s.SessionId)) return;
            s.Requesting = true;

            var sb = new StringBuilder();
            sb.Append($"{{\"session_id\":\"{s.SessionId}\",");
            sb.Append($"\"dt\":{_dt.ToString(System.Globalization.CultureInfo.InvariantCulture)},");
            sb.Append("\"inputs\":{");
            bool first = true;
            foreach (var kv in s.InputValues)
            {
                if (!first) sb.Append(',');
                first = false;
                string val = kv.Value is bool b
                    ? (b ? "true" : "false")
                    : Convert.ToString(kv.Value, System.Globalization.CultureInfo.InvariantCulture);
                sb.Append($"\"{kv.Key}\":{val}");
            }
            sb.Append("}}");

            string json = await PostAsync($"{_backendUrl}/sim/step", sb.ToString());

            if (!string.IsNullOrEmpty(json))
            {
                s.SimTime = ParseFloat(json, "\"t\"");

                // Real outputs → update value + plot history
                foreach (var name in s.PlotTimes.Keys)
                {
                    float val = ParseFloat(json, $"\"{name}\"");
                    s.OutputValues[name] = val;
                    var tList = s.PlotTimes[name];
                    var vList = s.PlotValues[name];
                    if (tList.Count >= FmuSession.MaxSamples) { tList.RemoveAt(0); vList.RemoveAt(0); }
                    tList.Add(s.SimTime);
                    vList.Add(val);
                }

                // Non-Real outputs → just update value display
                foreach (var kv in s.Variables)
                {
                    if (kv.Value.Causality != "output" && kv.Value.Causality != "local") continue;
                    if (kv.Value.Type == "Real") continue;
                    float v = ParseFloat(json, $"\"{kv.Key}\"");
                    s.OutputValues[kv.Key] = v;
                }

                s.Status = $"t = {s.SimTime:F4} s";
            }
            else
            {
                s.Status   = "Error on step — stopping auto-step";
                s.AutoStep = false;
            }

            s.Requesting = false;
            Repaint();
        }

        private async Task ResetAsync(FmuSession s)
        {
            s.AutoStep = false;
            await PostAsync($"{_backendUrl}/sim/reset", $"{{\"session_id\":\"{s.SessionId}\"}}");
            foreach (var k in s.PlotTimes.Keys) { s.PlotTimes[k].Clear(); s.PlotValues[k].Clear(); }
            s.OutputValues.Clear();
            s.SimTime = 0f;
            s.Status  = "Reset OK";
            Repaint();
        }

        private async Task StopSessionAsync(FmuSession s)
        {
            s.AutoStep = false;
            await PostAsync($"{_backendUrl}/sim/stop", $"{{\"session_id\":\"{s.SessionId}\"}}");
        }

        // ══════════════════════════════════════════════════════════════════
        //  HTTP helpers
        // ══════════════════════════════════════════════════════════════════

        private static async Task<string> GetAsync(string url)
        {
            using var req = UnityWebRequest.Get(url);
            req.timeout = 5;
            var op = req.SendWebRequest();
            while (!op.isDone) await Task.Yield();
            return req.result == UnityWebRequest.Result.Success ? req.downloadHandler.text : string.Empty;
        }

        private static async Task<string> PostAsync(string url, string body)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(body);
            using var req = new UnityWebRequest(url, "POST");
            req.uploadHandler   = new UploadHandlerRaw(bytes);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.timeout = 5;
            var op = req.SendWebRequest();
            while (!op.isDone) await Task.Yield();
            return req.result == UnityWebRequest.Result.Success ? req.downloadHandler.text : string.Empty;
        }

        // ══════════════════════════════════════════════════════════════════
        //  Lightweight JSON parsers
        // ══════════════════════════════════════════════════════════════════

        private static string ParseString(string json, string key)
        {
            string search = $"\"{key}\"";
            int i = json.IndexOf(search, StringComparison.Ordinal);
            if (i < 0) return string.Empty;
            i += search.Length;
            while (i < json.Length && (json[i] == ' ' || json[i] == ':')) i++;
            if (i >= json.Length || json[i] != '"') return string.Empty;
            i++;
            int end = json.IndexOf('"', i);
            return end < 0 ? string.Empty : json.Substring(i, end - i);
        }

        private static float ParseFloat(string json, string key)
        {
            int i = json.IndexOf(key, StringComparison.Ordinal);
            if (i < 0) return 0f;
            i += key.Length;
            while (i < json.Length && (json[i] == ' ' || json[i] == ':')) i++;
            int end = i;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '.' ||
                   json[end] == '-' || json[end] == 'e' || json[end] == 'E' || json[end] == '+')) end++;
            return float.TryParse(json.Substring(i, end - i),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float v) ? v : 0f;
        }

        private static List<string> ParseStringArray(string json, string key)
        {
            var result = new List<string>();
            int i = json.IndexOf($"\"{key}\"", StringComparison.Ordinal);
            if (i < 0) return result;
            i = json.IndexOf('[', i); if (i < 0) return result;
            int end = json.IndexOf(']', i); if (end < 0) return result;
            string inner = json.Substring(i + 1, end - i - 1);
            foreach (var part in inner.Split(','))
            {
                string t = part.Trim().Trim('"');
                if (!string.IsNullOrEmpty(t)) result.Add(t);
            }
            return result;
        }

        private static Dictionary<string, VarMeta> ParseVariables(string json)
        {
            var result = new Dictionary<string, VarMeta>();
            int start = json.IndexOf("\"variables\"", StringComparison.Ordinal);
            if (start < 0) return result;
            start = json.IndexOf('{', start + 11);
            if (start < 0) return result;

            int pos = start + 1;
            while (pos < json.Length)
            {
                int q1 = json.IndexOf('"', pos); if (q1 < 0) break;
                int q2 = json.IndexOf('"', q1 + 1); if (q2 < 0) break;
                string varName = json.Substring(q1 + 1, q2 - q1 - 1);
                if (varName == "}") break;

                int ob = json.IndexOf('{', q2); if (ob < 0) break;
                int cb = json.IndexOf('}', ob); if (cb < 0) break;
                string inner = json.Substring(ob, cb - ob + 1);

                string type      = ExtractQuoted(inner, "type");
                string causality = ExtractQuoted(inner, "causality");
                if (!string.IsNullOrEmpty(varName))
                    result[varName] = new VarMeta { Type = type, Causality = causality };

                pos = cb + 1;
                while (pos < json.Length && (json[pos] == ',' || json[pos] == ' ' ||
                       json[pos] == '\n' || json[pos] == '\r')) pos++;
                if (pos < json.Length && json[pos] == '}') break;
            }
            return result;
        }

        private static string ExtractQuoted(string json, string key)
        {
            string search = $"\"{key}\"";
            int i = json.IndexOf(search, StringComparison.Ordinal);
            if (i < 0) return string.Empty;
            i = json.IndexOf('"', i + search.Length); if (i < 0) return string.Empty;
            i++;
            int end = json.IndexOf('"', i);
            return end < 0 ? string.Empty : json.Substring(i, end - i);
        }

        // ── UI ────────────────────────────────────────────────────────────
        private static void HR()
        {
            GUILayout.Space(4);
            EditorGUI.DrawRect(
                GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none,
                    GUILayout.ExpandWidth(true), GUILayout.Height(1)),
                new Color(0.35f, 0.35f, 0.35f));
            GUILayout.Space(4);
        }
    }
}
