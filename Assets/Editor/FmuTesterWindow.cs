// Assets/Editor/FmuTesterWindow.cs
// ============================================================
// Generic FMU Tester — Speedma
// ============================================================
// Opens via: Speedma > FMU Tester
//
// v4 changes:
//   - Toggleable plot visibility for Real outputs
//   - Conflict warning for boolean inputs all active at once
//   - Real INPUTS can also be plotted (optional per-input toggle)
//   - Real/Integer INPUTS can be edited with configurable slider ranges
//   - Per-session input ranges with auto-generated defaults
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
        private string _backendUrl = "http://localhost:8000";
        private float _dt = 0.02f;

        private List<string> _availableFmus = new();
        private int _selectedFmuIdx = 0;
        private List<FmuSession> _sessions = new();
        private Vector2 _scrollPos;

        private bool _stylesReady;
        private GUIStyle _titleStyle, _sectionStyle, _okStyle, _errStyle, _dimStyle;

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

        private class VarMeta
        {
            public string Type;
            public string Causality;
        }

        private class NumericRange
        {
            public float Min;
            public float Max;
            public bool UseSlider;
        }

        private class FmuSession
        {
            public string FmuName;
            public string SessionId = "";
            public string Status = "Starting...";
            public bool Requesting = false;
            public bool AutoStep = false;
            public double LastAutoStep;
            public float SimTime = 0f;
            public bool Collapsed = false;

            public Dictionary<string, VarMeta> Variables = new();
            public Dictionary<string, object> InputValues = new();
            public Dictionary<string, float> OutputValues = new();

            // Plot visibility for both real outputs and selected real inputs
            public Dictionary<string, bool> SeriesVisible = new();
            public Dictionary<string, List<float>> PlotTimes = new();
            public Dictionary<string, List<float>> PlotValues = new();
            public Dictionary<string, int> SeriesColorIdx = new();
            public const int MaxSamples = 500;

            // Config for numeric inputs
            public Dictionary<string, NumericRange> InputRanges = new();
        }

        [MenuItem("Speedma/FMU Tester")]
        public static void ShowWindow()
        {
            var w = GetWindow<FmuTesterWindow>("FMU Tester");
            w.minSize = new Vector2(560, 650);
        }

        private void OnEnable() => EditorApplication.update += Tick;
        private void OnDisable() => EditorApplication.update -= Tick;

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

        private void EnsureStyles()
        {
            if (_stylesReady) return;
            _stylesReady = true;
            _titleStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 13, alignment = TextAnchor.MiddleCenter };
            _sectionStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 11 };
            _okStyle = new GUIStyle(EditorStyles.label) { normal = { textColor = new Color(0.2f, 0.85f, 0.3f) } };
            _errStyle = new GUIStyle(EditorStyles.label) { normal = { textColor = new Color(0.95f, 0.25f, 0.25f) } };
            _dimStyle = new GUIStyle(EditorStyles.label) { normal = { textColor = new Color(0.55f, 0.55f, 0.55f) } };
        }

        private void OnGUI()
        {
            EnsureStyles();
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            GUILayout.Space(6);
            GUILayout.Label("Speedma — FMU Tester", _titleStyle);
            HR();

            GUILayout.Label("Backend", _sectionStyle);
            _backendUrl = EditorGUILayout.TextField("URL", _backendUrl);
            _dt = EditorGUILayout.Slider("Step size dt (s)", _dt, 0.001f, 0.5f);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("🔄  Refresh FMU List", GUILayout.Height(26))) _ = RefreshFmuListAsync();
                if (GUILayout.Button("❤  Health Check", GUILayout.Height(26))) _ = HealthCheckAsync();
            }

            HR();

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

            if (_sessions.Count == 0)
                GUILayout.Label("No active sessions.", EditorStyles.centeredGreyMiniLabel);
            else
                for (int i = _sessions.Count - 1; i >= 0; i--)
                    DrawSession(_sessions[i], i);

            GUILayout.Space(10);
            EditorGUILayout.EndScrollView();
        }

        private void DrawSession(FmuSession s, int idx)
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                s.Collapsed = !EditorGUILayout.Foldout(!s.Collapsed, $"  {s.FmuName}  |  t = {s.SimTime:F3} s", true);
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

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("⏩ Step", GUILayout.Height(24))) _ = StepAsync(s);
                string lbl = s.AutoStep ? "⏸ Stop Auto" : "⏯ Auto-Step";
                if (GUILayout.Button(lbl, GUILayout.Height(24))) s.AutoStep = !s.AutoStep;
                if (GUILayout.Button("🔄 Reset", GUILayout.Height(24))) _ = ResetAsync(s);
            }

            GUILayout.Space(4);

            var inputVars = s.Variables.Where(kv => kv.Value.Causality == "input").ToList();
            if (inputVars.Count > 0)
            {
                GUILayout.Label("Inputs", _sectionStyle);

                var boolInputs = inputVars.Where(kv => kv.Value.Type == "Boolean").ToList();
                bool allBoolActive = boolInputs.Count > 1 && boolInputs.All(kv =>
                    s.InputValues.TryGetValue(kv.Key, out var v) && Convert.ToBoolean(v));
                if (allBoolActive)
                    EditorGUILayout.HelpBox("⚠ All boolean inputs are active simultaneously — this may cause conflicting states.", MessageType.Warning);

                foreach (var kv in inputVars)
                {
                    string name = kv.Key;
                    var meta = kv.Value;

                    switch (meta.Type)
                    {
                        case "Boolean":
                        {
                            bool cur = s.InputValues.TryGetValue(name, out var v) && Convert.ToBoolean(v);
                            bool next = EditorGUILayout.Toggle(name, cur);
                            s.InputValues[name] = next;
                            break;
                        }
                        case "Integer":
                        {
                            if (!s.InputRanges.TryGetValue(name, out var range))
                                range = s.InputRanges[name] = new NumericRange { Min = 0, Max = 100, UseSlider = false };

                            int cur = s.InputValues.TryGetValue(name, out var v) ? Convert.ToInt32(v) : 0;
                            DrawNumericInputRow(name, cur, range, out float nextIntAsFloat, isInteger: true, plotToggleAllowed: false, s);
                            s.InputValues[name] = Mathf.RoundToInt(nextIntAsFloat);
                            break;
                        }
                        default: // Real
                        {
                            if (!s.InputRanges.TryGetValue(name, out var range))
                            {
                                float curGuess = s.InputValues.TryGetValue(name, out var v) ? Convert.ToSingle(v) : 0f;
                                float mag = Mathf.Max(1f, Mathf.Abs(curGuess) * 2f);
                                range = s.InputRanges[name] = new NumericRange { Min = -mag, Max = mag, UseSlider = false };
                            }

                            float cur = s.InputValues.TryGetValue(name, out var v) ? Convert.ToSingle(v) : 0f;
                            DrawNumericInputRow(name, cur, range, out float next, isInteger: false, plotToggleAllowed: true, s);
                            s.InputValues[name] = next;
                            break;
                        }
                    }
                }
            }

            if (s.OutputValues.Count > 0)
            {
                GUILayout.Space(4);
                GUILayout.Label("Outputs", _sectionStyle);
                foreach (var kv in s.OutputValues)
                {
                    string name = kv.Key;
                    bool isRealSeries = s.PlotTimes.ContainsKey(name);
                    bool visible = !s.SeriesVisible.ContainsKey(name) || s.SeriesVisible[name];

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (isRealSeries)
                            DrawSeriesToggleAndColor(s, name, visible);
                        else
                            GUILayout.Space(34);

                        EditorGUILayout.LabelField(name, visible ? $"{kv.Value:F5}" : "—", visible ? EditorStyles.label : _dimStyle);
                    }
                }
            }

            bool anyVisible = s.PlotTimes.Keys.Any(k => s.PlotTimes[k].Count >= 2 && (!s.SeriesVisible.ContainsKey(k) || s.SeriesVisible[k]));
            if (anyVisible)
            {
                GUILayout.Space(4);
                GUILayout.Label("Live Plot", _sectionStyle);
                Rect pr = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.Height(180));
                DrawPlot(pr, s);
            }

            EditorGUI.indentLevel--;
            HR();
        }

        private void DrawNumericInputRow(string name, float cur, NumericRange range, out float next, bool isInteger, bool plotToggleAllowed, FmuSession s)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (plotToggleAllowed)
                    {
                        bool vis = !s.SeriesVisible.ContainsKey(name) || s.SeriesVisible[name];
                        DrawSeriesToggleAndColor(s, name, vis);
                    }
                    else
                    {
                        GUILayout.Space(34);
                    }

                    GUILayout.Label(name, GUILayout.Width(160));
                    range.UseSlider = EditorGUILayout.ToggleLeft("Slider", range.UseSlider, GUILayout.Width(56));
                    GUILayout.Label("Min", GUILayout.Width(24));
                    range.Min = EditorGUILayout.FloatField(range.Min, GUILayout.Width(70));
                    GUILayout.Label("Max", GUILayout.Width(28));
                    range.Max = EditorGUILayout.FloatField(range.Max, GUILayout.Width(70));
                }

                if (range.Max < range.Min)
                {
                    float tmp = range.Min;
                    range.Min = range.Max;
                    range.Max = tmp;
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (range.UseSlider)
                    {
                        next = EditorGUILayout.Slider(cur, range.Min, range.Max);
                    }
                    else
                    {
                        next = EditorGUILayout.FloatField("Value", cur);
                    }

                    if (isInteger)
                        next = Mathf.Round(next);
                }
            }
        }

        private void DrawSeriesToggleAndColor(FmuSession s, string name, bool visible)
        {
            if (!s.SeriesVisible.ContainsKey(name)) s.SeriesVisible[name] = true;
            if (!s.SeriesColorIdx.ContainsKey(name)) s.SeriesColorIdx[name] = s.SeriesColorIdx.Count;

            Rect totalRect = GUILayoutUtility.GetRect(48, 16, GUILayout.Width(48), GUILayout.Height(16));
            Rect toggleRect = new Rect(totalRect.x, totalRect.y, 20, totalRect.height);

            int ci = s.SeriesColorIdx[name];
            Color col = PlotColors[ci % PlotColors.Length];
            Rect colorRect = new Rect(totalRect.x + 24, totalRect.y + (totalRect.height - 12f) * 0.5f, 12, 12);
            EditorGUI.DrawRect(colorRect, visible ? col : new Color(col.r, col.g, col.b, 0.25f));

            bool next = EditorGUI.Toggle(toggleRect, visible);
            if (next != visible) s.SeriesVisible[name] = next;

            var e = Event.current;
            if (e.type == EventType.MouseDown && totalRect.Contains(e.mousePosition))
            {
                s.SeriesVisible[name] = !s.SeriesVisible[name];
                e.Use();
            }
        }

        private void DrawPlot(Rect r, FmuSession s)
        {
            EditorGUI.DrawRect(r, new Color(0.1f, 0.1f, 0.1f));
            float tMax = s.SimTime > 1f ? s.SimTime : 1f;

            float vMin = float.MaxValue, vMax = -float.MaxValue;
            foreach (var name in s.PlotValues.Keys)
            {
                if (s.SeriesVisible.TryGetValue(name, out var vis) && !vis) continue;
                foreach (var v in s.PlotValues[name])
                {
                    if (v < vMin) vMin = v;
                    if (v > vMax) vMax = v;
                }
            }
            if (vMin >= vMax || vMin == float.MaxValue) { vMin = -1; vMax = 1; }
            float pad = (vMax - vMin) * 0.08f;
            vMin -= pad; vMax += pad;

            Handles.color = new Color(0.22f, 0.22f, 0.22f);
            for (int g = 0; g <= 4; g++)
            {
                float y = r.y + r.height * (1f - g / 4f);
                float val = Mathf.Lerp(vMin, vMax, g / 4f);
                Handles.DrawLine(new Vector3(r.x, y), new Vector3(r.xMax, y));
                GUI.color = Color.gray;
                GUI.Label(new Rect(r.x + 2, y - 8, 64, 16), val.ToString("F2"));
            }
            GUI.color = Color.white;

            int legendRow = 0;
            foreach (var name in s.PlotTimes.Keys)
            {
                bool visible = !s.SeriesVisible.ContainsKey(name) || s.SeriesVisible[name];
                int ci = s.SeriesColorIdx.TryGetValue(name, out var c) ? c : 0;
                Color col = PlotColors[ci % PlotColors.Length];
                var times = s.PlotTimes[name];
                var values = s.PlotValues[name];

                if (visible && times.Count >= 2)
                {
                    Handles.color = col;
                    for (int i = 1; i < times.Count; i++)
                    {
                        float x0 = r.x + Mathf.InverseLerp(0, tMax, times[i - 1]) * r.width;
                        float y0 = r.y + (1f - Mathf.InverseLerp(vMin, vMax, values[i - 1])) * r.height;
                        float x1 = r.x + Mathf.InverseLerp(0, tMax, times[i]) * r.width;
                        float y1 = r.y + (1f - Mathf.InverseLerp(vMin, vMax, values[i])) * r.height;
                        Handles.DrawLine(new Vector3(x0, y0), new Vector3(x1, y1));
                    }
                }

                float ly = r.y + 4 + legendRow * 16;
                Color legendCol = visible ? col : new Color(col.r, col.g, col.b, 0.2f);
                EditorGUI.DrawRect(new Rect(r.xMax - 140, ly, 10, 10), legendCol);
                GUI.color = visible ? Color.white : new Color(1, 1, 1, 0.3f);
                GUI.Label(new Rect(r.xMax - 126, ly - 2, 124, 14), name);
                GUI.color = Color.white;
                legendRow++;
            }

            GUI.color = Color.gray;
            GUI.Label(new Rect(r.x + 64, r.yMax - 14, 40, 14), "0 s");
            GUI.Label(new Rect(r.xMax - 44, r.yMax - 14, 44, 14), $"{tMax:F1} s");
            GUI.color = Color.white;
        }

        private async Task RefreshFmuListAsync()
        {
            string json = await GetAsync($"{_backendUrl}/sim/list");
            if (string.IsNullOrEmpty(json)) return;
            _availableFmus = ParseStringArray(json, "fmus");
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
            s.Status = "Session active";
            Repaint();
            await FetchVariablesAsync(s);
        }

        private async Task FetchVariablesAsync(FmuSession s)
        {
            string json = await GetAsync($"{_backendUrl}/sim/variables/{s.SessionId}");
            if (string.IsNullOrEmpty(json)) return;

            s.Variables = ParseVariables(json);

            foreach (var kv in s.Variables)
            {
                if (kv.Value.Causality != "input") continue;
                object defaultValue = kv.Value.Type == "Boolean" ? (object)false : kv.Value.Type == "Integer" ? (object)0 : (object)0f;
                s.InputValues.TryAdd(kv.Key, defaultValue);

                if (kv.Value.Type == "Real")
                {
                    s.PlotTimes[kv.Key] = new List<float>();
                    s.PlotValues[kv.Key] = new List<float>();
                    s.SeriesVisible[kv.Key] = false; // inputs off by default on plot
                    s.SeriesColorIdx[kv.Key] = s.SeriesColorIdx.Count;

                    float mag = 1f;
                    if (s.InputValues.TryGetValue(kv.Key, out var rv))
                        mag = Mathf.Max(1f, Mathf.Abs(Convert.ToSingle(rv)) * 2f);
                    s.InputRanges[kv.Key] = new NumericRange { Min = -mag, Max = mag, UseSlider = false };
                }
                else if (kv.Value.Type == "Integer")
                {
                    s.InputRanges[kv.Key] = new NumericRange { Min = 0, Max = 100, UseSlider = false };
                }
            }

            foreach (var kv in s.Variables)
            {
                if (kv.Value.Causality != "output" && kv.Value.Causality != "local") continue;
                if (kv.Value.Type != "Real") continue;
                if (!s.PlotTimes.ContainsKey(kv.Key)) s.PlotTimes[kv.Key] = new List<float>();
                if (!s.PlotValues.ContainsKey(kv.Key)) s.PlotValues[kv.Key] = new List<float>();
                s.SeriesVisible[kv.Key] = true;
                s.SeriesColorIdx[kv.Key] = s.SeriesColorIdx.Count;
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
                string val = kv.Value is bool b ? (b ? "true" : "false") : Convert.ToString(kv.Value, System.Globalization.CultureInfo.InvariantCulture);
                sb.Append($"\"{kv.Key}\":{val}");
            }
            sb.Append("}}");

            string json = await PostAsync($"{_backendUrl}/sim/step", sb.ToString());
            if (!string.IsNullOrEmpty(json))
            {
                s.SimTime = ParseFloat(json, "\"t\"");

                foreach (var name in s.PlotTimes.Keys.ToList())
                {
                    float val;
                    bool isInputReal = s.Variables.TryGetValue(name, out var meta) && meta.Causality == "input" && meta.Type == "Real";
                    if (isInputReal)
                    {
                        val = s.InputValues.TryGetValue(name, out var iv) ? Convert.ToSingle(iv) : 0f;
                    }
                    else
                    {
                        val = ParseFloat(json, $"\"{name}\"");
                        s.OutputValues[name] = val;
                    }

                    var tList = s.PlotTimes[name];
                    var vList = s.PlotValues[name];
                    if (tList.Count >= FmuSession.MaxSamples) { tList.RemoveAt(0); vList.RemoveAt(0); }
                    tList.Add(s.SimTime);
                    vList.Add(val);
                }

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
                s.Status = "Error on step — stopping auto-step";
                s.AutoStep = false;
            }

            s.Requesting = false;
            Repaint();
        }

        private async Task ResetAsync(FmuSession s)
        {
            s.AutoStep = false;
            await PostAsync($"{_backendUrl}/sim/reset", $"{{\"session_id\":\"{s.SessionId}\"}}");
            foreach (var k in s.PlotTimes.Keys)
            {
                s.PlotTimes[k].Clear();
                s.PlotValues[k].Clear();
            }
            s.OutputValues.Clear();
            s.SimTime = 0f;
            s.Status = "Reset OK";
            Repaint();
        }

        private async Task StopSessionAsync(FmuSession s)
        {
            s.AutoStep = false;
            await PostAsync($"{_backendUrl}/sim/stop", $"{{\"session_id\":\"{s.SessionId}\"}}");
        }

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
            req.uploadHandler = new UploadHandlerRaw(bytes);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.timeout = 5;
            var op = req.SendWebRequest();
            while (!op.isDone) await Task.Yield();
            return req.result == UnityWebRequest.Result.Success ? req.downloadHandler.text : string.Empty;
        }

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
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '.' || json[end] == '-' || json[end] == 'e' || json[end] == 'E' || json[end] == '+')) end++;
            return float.TryParse(json.Substring(i, end - i), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float v) ? v : 0f;
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

                string type = ExtractQuoted(inner, "type");
                string causality = ExtractQuoted(inner, "causality");
                if (!string.IsNullOrEmpty(varName)) result[varName] = new VarMeta { Type = type, Causality = causality };

                pos = cb + 1;
                while (pos < json.Length && (json[pos] == ',' || json[pos] == ' ' || json[pos] == '\n' || json[pos] == '\r')) pos++;
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

        private static void HR()
        {
            GUILayout.Space(4);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.Height(1)), new Color(0.35f, 0.35f, 0.35f));
            GUILayout.Space(4);
        }
    }
}
