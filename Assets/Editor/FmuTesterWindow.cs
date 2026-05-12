// Assets/Editor/FmuTesterWindow.cs
// ============================================================
// Generic FMU Tester — Speedma  v5
// ============================================================
// Opens via: Speedma > FMU Tester
//
// New in v5:
//   - Consumes full variable metadata from backend:
//       type, causality, variability, start, min, max, description, unit
//   - Real/Integer input sliders auto-populated from FMU min/max
//   - Input default values taken from FMU start value
//   - Unit shown next to each value (inputs + outputs)
//   - Description shown as tooltip and greyed label
//   - Variability shown as tag next to variable name
//   - Real inputs can optionally be plotted alongside outputs
//   - Boolean conflict warning still present
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
        private float  _dt         = 0.02f;

        private List<string>    _availableFmus  = new();
        private int             _selectedFmuIdx = 0;
        private List<FmuSession> _sessions       = new();
        private Vector2         _scrollPos;

        private bool     _stylesReady;
        private GUIStyle _titleStyle, _sectionStyle, _okStyle, _errStyle,
                         _dimStyle, _tagStyle, _unitStyle, _descStyle;

        private static readonly Color[] PlotColors =
        {
            new(0.2f, 0.9f, 0.3f),  new(0.3f, 0.6f, 1.0f),
            new(1.0f, 0.7f, 0.2f),  new(1.0f, 0.3f, 0.4f),
            new(0.8f, 0.3f, 1.0f),  new(0.3f, 0.9f, 0.9f),
            new(1.0f, 0.5f, 0.8f),  new(0.9f, 0.9f, 0.3f),
        };

        // ══════════════════════════════════════════════════════════════════
        //  Data model
        // ══════════════════════════════════════════════════════════════════
        private class VarMeta
        {
            public string Type;         // Real | Boolean | Integer | String
            public string Causality;    // input | output | local | parameter | independent
            public string Variability;  // continuous | discrete | fixed | tunable | constant
            public float? Start;        // initial/default (null = not declared)
            public float? Min;          // declared min (null = unbounded)
            public float? Max;          // declared max (null = unbounded)
            public string Description;  // human description from model
            public string Unit;         // physical unit string (e.g. "V", "A", "Ohm")
        }

        private class NumericRange
        {
            public float Min;
            public float Max;
            public bool  UseSlider;
        }

        private class FmuSession
        {
            public string  FmuName;
            public string  SessionId  = "";
            public string  Status     = "Starting...";
            public bool    Requesting = false;
            public bool    AutoStep   = false;
            public double  LastAutoStep;
            public float   SimTime    = 0f;
            public bool    Collapsed  = false;

            public Dictionary<string, VarMeta>    Variables    = new();
            public Dictionary<string, object>     InputValues  = new();
            public Dictionary<string, float>      OutputValues = new();

            public Dictionary<string, bool>        SeriesVisible  = new();
            public Dictionary<string, List<float>> PlotTimes      = new();
            public Dictionary<string, List<float>> PlotValues     = new();
            public Dictionary<string, int>         SeriesColorIdx = new();
            public const int MaxSamples = 500;

            public Dictionary<string, NumericRange> InputRanges = new();
        }

        // ══════════════════════════════════════════════════════════════════

        [MenuItem("Speedma/FMU Tester")]
        public static void ShowWindow()
        {
            var w = GetWindow<FmuTesterWindow>("FMU Tester");
            w.minSize = new Vector2(580, 660);
        }

        private void OnEnable()  => EditorApplication.update += Tick;
        private void OnDisable() => EditorApplication.update -= Tick;

        private void Tick()
        {
            const double interval = 0.05;
            double now = EditorApplication.timeSinceStartup;
            foreach (var s in _sessions)
            {
                if (!s.AutoStep || s.Requesting || string.IsNullOrEmpty(s.SessionId)) continue;
                if (now - s.LastAutoStep >= interval) { s.LastAutoStep = now; _ = StepAsync(s); }
            }
        }

        private void EnsureStyles()
        {
            if (_stylesReady) return;
            _stylesReady  = true;
            _titleStyle   = new GUIStyle(EditorStyles.boldLabel)  { fontSize = 13, alignment = TextAnchor.MiddleCenter };
            _sectionStyle = new GUIStyle(EditorStyles.boldLabel)  { fontSize = 11 };
            _okStyle      = new GUIStyle(EditorStyles.label)      { normal = { textColor = new Color(0.2f, 0.85f, 0.3f) } };
            _errStyle     = new GUIStyle(EditorStyles.label)      { normal = { textColor = new Color(0.95f, 0.25f, 0.25f) } };
            _dimStyle     = new GUIStyle(EditorStyles.label)      { normal = { textColor = new Color(0.55f, 0.55f, 0.55f) } };
            _unitStyle    = new GUIStyle(EditorStyles.miniLabel)  { normal = { textColor = new Color(0.45f, 0.80f, 1.0f) } };
            _descStyle    = new GUIStyle(EditorStyles.miniLabel)  { normal = { textColor = new Color(0.60f, 0.60f, 0.60f) }, wordWrap = true };
            _tagStyle     = new GUIStyle(EditorStyles.miniLabel)  { normal = { textColor = new Color(0.70f, 0.70f, 0.30f) } };
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
            HR();

            GUILayout.Label("Backend", _sectionStyle);
            _backendUrl = EditorGUILayout.TextField("URL", _backendUrl);
            _dt         = EditorGUILayout.Slider("Step dt (s)", _dt, 0.001f, 0.5f);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("🔄  Refresh FMU List", GUILayout.Height(26))) _ = RefreshFmuListAsync();
                if (GUILayout.Button("❤  Health Check",      GUILayout.Height(26))) _ = HealthCheckAsync();
            }
            HR();

            GUILayout.Label("Launch FMU Session", _sectionStyle);
            if (_availableFmus.Count == 0)
                EditorGUILayout.HelpBox("No FMUs found. Click \"Refresh FMU List\" with the backend running.", MessageType.Info);
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

        // ══════════════════════════════════════════════════════════════════
        //  Session panel
        // ══════════════════════════════════════════════════════════════════
        private void DrawSession(FmuSession s, int idx)
        {
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

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("⏩ Step",    GUILayout.Height(24))) _ = StepAsync(s);
                string lbl = s.AutoStep ? "⏸ Stop Auto" : "⏯ Auto-Step";
                if (GUILayout.Button(lbl,          GUILayout.Height(24))) s.AutoStep = !s.AutoStep;
                if (GUILayout.Button("🔄 Reset",   GUILayout.Height(24))) _ = ResetAsync(s);
            }
            GUILayout.Space(4);

            // ──────── INPUTS ──────────────────────────────────────────────
            var inputVars = s.Variables.Where(kv => kv.Value.Causality == "input").ToList();
            if (inputVars.Count > 0)
            {
                GUILayout.Label("Inputs", _sectionStyle);

                // Conflict warning: all booleans active
                var boolInputs = inputVars.Where(kv => kv.Value.Type == "Boolean").ToList();
                bool allBoolOn = boolInputs.Count > 1 && boolInputs.All(kv =>
                    s.InputValues.TryGetValue(kv.Key, out var bv) && Convert.ToBoolean(bv));
                if (allBoolOn)
                    EditorGUILayout.HelpBox("⚠ All boolean inputs are active simultaneously — may cause conflicting states.", MessageType.Warning);

                foreach (var kv in inputVars)
                {
                    var meta = kv.Value;
                    string name = kv.Key;
                    DrawInputVariable(s, name, meta);
                }
            }

            // ──────── OUTPUTS ────────────────────────────────────────────
            if (s.OutputValues.Count > 0)
            {
                GUILayout.Space(4);
                GUILayout.Label("Outputs", _sectionStyle);
                foreach (var kv in s.OutputValues)
                {
                    string name    = kv.Key;
                    bool   isPlot  = s.PlotTimes.ContainsKey(name);
                    bool   visible = !s.SeriesVisible.ContainsKey(name) || s.SeriesVisible[name];
                    var    meta    = s.Variables.TryGetValue(name, out var m) ? m : null;

                    using (new EditorGUILayout.VerticalScope())
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            if (isPlot)
                                DrawSeriesToggleAndColor(s, name, visible);
                            else
                                GUILayout.Space(34);

                            // variability tag
                            string tag = meta?.Variability ?? "";
                            if (!string.IsNullOrEmpty(tag))
                                GUILayout.Label($"[{tag}]", _tagStyle, GUILayout.Width(72));

                            EditorGUILayout.LabelField(name,
                                visible ? FormatValue(kv.Value, meta?.Unit) : "—",
                                visible ? EditorStyles.label : _dimStyle);
                        }

                        // description below
                        if (meta != null && !string.IsNullOrEmpty(meta.Description))
                        {
                            EditorGUI.indentLevel++;
                            GUILayout.Label(meta.Description, _descStyle);
                            EditorGUI.indentLevel--;
                        }
                    }
                }
            }

            // ──────── LIVE PLOT ───────────────────────────────────────────
            bool anyVisible = s.PlotTimes.Keys.Any(k =>
                s.PlotTimes[k].Count >= 2 &&
                (!s.SeriesVisible.ContainsKey(k) || s.SeriesVisible[k]));
            if (anyVisible)
            {
                GUILayout.Space(4);
                GUILayout.Label("Live Plot", _sectionStyle);
                Rect pr = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none,
                    GUILayout.ExpandWidth(true), GUILayout.Height(180));
                DrawPlot(pr, s);
            }

            EditorGUI.indentLevel--;
            HR();
        }

        // ══════════════════════════════════════════════════════════════════
        //  Input variable renderer
        // ══════════════════════════════════════════════════════════════════
        private void DrawInputVariable(FmuSession s, string name, VarMeta meta)
        {
            switch (meta.Type)
            {
                case "Boolean":
                {
                    bool cur  = s.InputValues.TryGetValue(name, out var v) && Convert.ToBoolean(v);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        bool next = EditorGUILayout.Toggle(name, cur);
                        if (!string.IsNullOrEmpty(meta.Description))
                            GUILayout.Label(meta.Description, _descStyle);
                        s.InputValues[name] = next;
                    }
                    break;
                }

                case "Integer":
                {
                    if (!s.InputRanges.TryGetValue(name, out var range))
                        range = s.InputRanges[name] = new NumericRange
                        {
                            Min = meta.Min.HasValue ? meta.Min.Value : 0f,
                            Max = meta.Max.HasValue ? meta.Max.Value : 100f,
                            UseSlider = meta.Min.HasValue && meta.Max.HasValue,
                        };

                    int cur  = s.InputValues.TryGetValue(name, out var v) ? Convert.ToInt32(v) : 0;
                    DrawNumericRow(name, cur, range, meta, isInteger: true, canPlot: false, s,
                        out float next);
                    s.InputValues[name] = Mathf.RoundToInt(next);
                    break;
                }

                default: // Real
                {
                    float startVal = meta.Start ?? 0f;
                    if (!s.InputRanges.TryGetValue(name, out var range))
                    {
                        float lo, hi;
                        if (meta.Min.HasValue && meta.Max.HasValue)
                        { lo = meta.Min.Value; hi = meta.Max.Value; }
                        else
                        { float mag = Mathf.Max(1f, Mathf.Abs(startVal) * 2f); lo = -mag; hi = mag; }

                        range = s.InputRanges[name] = new NumericRange
                        {
                            Min = lo, Max = hi,
                            UseSlider = meta.Min.HasValue && meta.Max.HasValue,
                        };
                    }

                    float cur = s.InputValues.TryGetValue(name, out var rv) ? Convert.ToSingle(rv) : startVal;
                    DrawNumericRow(name, cur, range, meta, isInteger: false, canPlot: true, s,
                        out float next);
                    s.InputValues[name] = next;
                    break;
                }
            }
        }

        private void DrawNumericRow(string name, float cur, NumericRange range, VarMeta meta,
                                    bool isInteger, bool canPlot, FmuSession s, out float next)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                // ── Header row: plot toggle | color | tag | name | unit | Slider toggle | min | max
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (canPlot)
                    {
                        bool vis = !s.SeriesVisible.ContainsKey(name) || s.SeriesVisible[name];
                        DrawSeriesToggleAndColor(s, name, vis);
                    }
                    else GUILayout.Space(34);

                    if (!string.IsNullOrEmpty(meta.Variability))
                        GUILayout.Label($"[{meta.Variability}]", _tagStyle, GUILayout.Width(72));

                    GUILayout.Label(name, GUILayout.MinWidth(80));

                    if (!string.IsNullOrEmpty(meta.Unit))
                        GUILayout.Label(meta.Unit, _unitStyle, GUILayout.Width(40));

                    GUILayout.FlexibleSpace();
                    range.UseSlider = EditorGUILayout.ToggleLeft("Slider", range.UseSlider, GUILayout.Width(56));
                    GUILayout.Label("Min", GUILayout.Width(24));
                    range.Min = EditorGUILayout.FloatField(range.Min, GUILayout.Width(64));
                    GUILayout.Label("Max", GUILayout.Width(28));
                    range.Max = EditorGUILayout.FloatField(range.Max, GUILayout.Width(64));
                }

                if (range.Max < range.Min) { float tmp = range.Min; range.Min = range.Max; range.Max = tmp; }

                // ── Value row
                using (new EditorGUILayout.HorizontalScope())
                {
                    next = range.UseSlider
                        ? EditorGUILayout.Slider(cur, range.Min, range.Max)
                        : EditorGUILayout.FloatField("Value", cur);
                    if (isInteger) next = Mathf.Round(next);

                    if (meta.Start.HasValue && GUILayout.Button("reset", GUILayout.Width(44)))
                        next = meta.Start.Value;
                }

                // ── Description row
                if (!string.IsNullOrEmpty(meta.Description))
                    GUILayout.Label(meta.Description, _descStyle);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        //  Series toggle + color box
        // ══════════════════════════════════════════════════════════════════
        private void DrawSeriesToggleAndColor(FmuSession s, string name, bool visible)
        {
            if (!s.SeriesVisible.ContainsKey(name))  s.SeriesVisible[name]  = true;
            if (!s.SeriesColorIdx.ContainsKey(name)) s.SeriesColorIdx[name] = s.SeriesColorIdx.Count;

            Rect total  = GUILayoutUtility.GetRect(48, 16, GUILayout.Width(48), GUILayout.Height(16));
            Rect toggle = new(total.x, total.y, 20, total.height);
            int  ci     = s.SeriesColorIdx[name];
            Color col   = PlotColors[ci % PlotColors.Length];
            Rect cbox   = new(total.x + 24, total.y + (total.height - 12f) * 0.5f, 12, 12);

            EditorGUI.DrawRect(cbox, visible ? col : new Color(col.r, col.g, col.b, 0.25f));
            bool next = EditorGUI.Toggle(toggle, visible);
            if (next != visible) s.SeriesVisible[name] = next;

            var e = Event.current;
            if (e.type == EventType.MouseDown && total.Contains(e.mousePosition))
            { s.SeriesVisible[name] = !s.SeriesVisible[name]; e.Use(); }
        }

        // ══════════════════════════════════════════════════════════════════
        //  Plot
        // ══════════════════════════════════════════════════════════════════
        private void DrawPlot(Rect r, FmuSession s)
        {
            EditorGUI.DrawRect(r, new Color(0.1f, 0.1f, 0.1f));
            float tMax = s.SimTime > 1f ? s.SimTime : 1f;

            float vMin = float.MaxValue, vMax = -float.MaxValue;
            foreach (var name in s.PlotValues.Keys)
            {
                if (s.SeriesVisible.TryGetValue(name, out var vis) && !vis) continue;
                foreach (var v in s.PlotValues[name]) { if (v < vMin) vMin = v; if (v > vMax) vMax = v; }
            }
            if (vMin >= vMax || vMin == float.MaxValue) { vMin = -1; vMax = 1; }
            float pad = (vMax - vMin) * 0.08f;
            vMin -= pad; vMax += pad;

            Handles.color = new Color(0.22f, 0.22f, 0.22f);
            for (int g = 0; g <= 4; g++)
            {
                float y   = r.y + r.height * (1f - g / 4f);
                float val = Mathf.Lerp(vMin, vMax, g / 4f);
                Handles.DrawLine(new Vector3(r.x, y), new Vector3(r.xMax, y));
                GUI.color = Color.gray;
                GUI.Label(new Rect(r.x + 2, y - 8, 64, 16), val.ToString("F2"));
            }
            GUI.color = Color.white;

            int legendRow = 0;
            foreach (var name in s.PlotTimes.Keys)
            {
                bool  visible = !s.SeriesVisible.ContainsKey(name) || s.SeriesVisible[name];
                int   ci      = s.SeriesColorIdx.TryGetValue(name, out var c) ? c : 0;
                Color col     = PlotColors[ci % PlotColors.Length];
                var   times   = s.PlotTimes[name];
                var   values  = s.PlotValues[name];

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

                // legend label includes unit if available
                var  vmeta     = s.Variables.TryGetValue(name, out var vm) ? vm : null;
                string label   = vmeta != null && !string.IsNullOrEmpty(vmeta.Unit)
                                   ? $"{name} [{vmeta.Unit}]" : name;
                float ly       = r.y + 4 + legendRow * 16;
                Color lcol     = visible ? col : new Color(col.r, col.g, col.b, 0.2f);
                EditorGUI.DrawRect(new Rect(r.xMax - 150, ly, 10, 10), lcol);
                GUI.color = visible ? Color.white : new Color(1,1,1,0.3f);
                GUI.Label(new Rect(r.xMax - 136, ly - 2, 134, 14), label);
                GUI.color = Color.white;
                legendRow++;
            }

            GUI.color = Color.gray;
            GUI.Label(new Rect(r.x + 64,    r.yMax - 14, 40, 14), "0 s");
            GUI.Label(new Rect(r.xMax - 44, r.yMax - 14, 44, 14), $"{tMax:F1} s");
            GUI.color = Color.white;
        }

        // ══════════════════════════════════════════════════════════════════
        //  HTTP
        // ══════════════════════════════════════════════════════════════════
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
            _sessions.Add(s); Repaint();

            string json = await PostAsync($"{_backendUrl}/sim/start", $"{{\"fmu_name\":\"{fmuName}\"}}");
            if (string.IsNullOrEmpty(json)) { s.Status = "Error: backend unreachable"; Repaint(); return; }
            s.SessionId = ParseString(json, "session_id");
            s.Status    = "Session active";
            Repaint();
            await FetchVariablesAsync(s);
        }

        private async Task FetchVariablesAsync(FmuSession s)
        {
            string json = await GetAsync($"{_backendUrl}/sim/variables/{s.SessionId}");
            if (string.IsNullOrEmpty(json)) return;

            s.Variables = ParseVariablesFull(json);

            foreach (var kv in s.Variables)
            {
                var meta = kv.Value;
                string name = kv.Key;

                if (meta.Causality == "input")
                {
                    object def = meta.Type == "Boolean" ? (object)false
                               : meta.Type == "Integer" ? (object)(int)(meta.Start ?? 0f)
                               : (object)(meta.Start ?? 0f);
                    s.InputValues.TryAdd(name, def);

                    if (meta.Type == "Real")
                    {
                        s.PlotTimes[name]      = new List<float>();
                        s.PlotValues[name]     = new List<float>();
                        s.SeriesVisible[name]  = false;
                        s.SeriesColorIdx[name] = s.SeriesColorIdx.Count;

                        float lo, hi;
                        if (meta.Min.HasValue && meta.Max.HasValue) { lo = meta.Min.Value; hi = meta.Max.Value; }
                        else { float mag = Mathf.Max(1f, Mathf.Abs(meta.Start ?? 0f) * 2f); lo = -mag; hi = mag; }
                        s.InputRanges[name] = new NumericRange { Min = lo, Max = hi, UseSlider = meta.Min.HasValue && meta.Max.HasValue };
                    }
                    else if (meta.Type == "Integer")
                    {
                        s.InputRanges[name] = new NumericRange
                        {
                            Min = meta.Min ?? 0f, Max = meta.Max ?? 100f,
                            UseSlider = meta.Min.HasValue && meta.Max.HasValue,
                        };
                    }
                }
                else if (meta.Causality == "output" || meta.Causality == "local")
                {
                    if (meta.Type == "Real" && !s.PlotTimes.ContainsKey(name))
                    {
                        s.PlotTimes[name]      = new List<float>();
                        s.PlotValues[name]     = new List<float>();
                        s.SeriesVisible[name]  = true;
                        s.SeriesColorIdx[name] = s.SeriesColorIdx.Count;
                    }
                }
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
                string val = kv.Value is bool b ? (b ? "true" : "false")
                           : Convert.ToString(kv.Value, System.Globalization.CultureInfo.InvariantCulture);
                sb.Append($"\"{kv.Key}\":{val}");
            }
            sb.Append("}}");

            string json = await PostAsync($"{_backendUrl}/sim/step", sb.ToString());
            if (!string.IsNullOrEmpty(json))
            {
                s.SimTime = ParseFloat(json, "\"t\"");

                foreach (var name in s.PlotTimes.Keys.ToList())
                {
                    bool isInputSeries = s.Variables.TryGetValue(name, out var vm)
                                        && vm.Causality == "input" && vm.Type == "Real";
                    float val = isInputSeries
                        ? (s.InputValues.TryGetValue(name, out var iv) ? Convert.ToSingle(iv) : 0f)
                        : ParseFloat(json, $"\"{name}\"");

                    if (!isInputSeries) s.OutputValues[name] = val;

                    var tl = s.PlotTimes[name]; var vl = s.PlotValues[name];
                    if (tl.Count >= FmuSession.MaxSamples) { tl.RemoveAt(0); vl.RemoveAt(0); }
                    tl.Add(s.SimTime); vl.Add(val);
                }

                foreach (var kv in s.Variables)
                {
                    if (kv.Value.Causality != "output" && kv.Value.Causality != "local") continue;
                    if (kv.Value.Type == "Real") continue;
                    s.OutputValues[kv.Key] = ParseFloat(json, $"\"{kv.Key}\"");
                }

                s.Status = $"t = {s.SimTime:F4} s";
            }
            else { s.Status = "Error on step — stopping auto-step"; s.AutoStep = false; }

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
        //  JSON parsers
        // ══════════════════════════════════════════════════════════════════

        // Parses the full /sim/variables response into VarMeta objects
        // JSON shape: {"variables": {"name": {"type":"Real","causality":"output","variability":"continuous",
        //                                     "start":0.0,"min":null,"max":null,"description":"","unit":"V"}, ...}}
        private static Dictionary<string, VarMeta> ParseVariablesFull(string json)
        {
            var result = new Dictionary<string, VarMeta>();
            int start  = json.IndexOf("\"variables\"", StringComparison.Ordinal);
            if (start < 0) return result;
            start = json.IndexOf('{', start + 11);
            if (start < 0) return result;

            int pos = start + 1;
            while (pos < json.Length)
            {
                int q1 = json.IndexOf('"', pos);           if (q1 < 0) break;
                int q2 = json.IndexOf('"', q1 + 1);        if (q2 < 0) break;
                string varName = json.Substring(q1 + 1, q2 - q1 - 1);
                if (varName == "}") break;

                int ob = json.IndexOf('{', q2);             if (ob < 0) break;
                // find matching closing brace
                int depth = 1; int cb = ob + 1;
                while (cb < json.Length && depth > 0)
                {
                    if (json[cb] == '{') depth++;
                    else if (json[cb] == '}') depth--;
                    cb++;
                }
                cb--; // now cb points to }
                string inner = json.Substring(ob, cb - ob + 1);

                if (!string.IsNullOrEmpty(varName))
                    result[varName] = new VarMeta
                    {
                        Type        = ExtractQ(inner, "type"),
                        Causality   = ExtractQ(inner, "causality"),
                        Variability = ExtractQ(inner, "variability"),
                        Start       = ExtractNullableFloat(inner, "start"),
                        Min         = ExtractNullableFloat(inner, "min"),
                        Max         = ExtractNullableFloat(inner, "max"),
                        Description = ExtractQ(inner, "description"),
                        Unit        = ExtractQ(inner, "unit"),
                    };

                pos = cb + 1;
                while (pos < json.Length && (json[pos] == ',' || json[pos] == ' ' || json[pos] == '\n' || json[pos] == '\r')) pos++;
                if (pos < json.Length && json[pos] == '}') break;
            }
            return result;
        }

        private static float? ExtractNullableFloat(string json, string key)
        {
            string search = $"\"{key}\"";
            int i = json.IndexOf(search, StringComparison.Ordinal);
            if (i < 0) return null;
            i += search.Length;
            while (i < json.Length && (json[i] == ' ' || json[i] == ':')) i++;
            if (i >= json.Length) return null;
            if (json[i] == 'n') return null; // null
            int end = i;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '.' ||
                   json[end] == '-' || json[end] == 'e' || json[end] == 'E' || json[end] == '+')) end++;
            if (end == i) return null;
            return float.TryParse(json.Substring(i, end - i),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float v) ? v : (float?)null;
        }

        private static string ExtractQ(string json, string key)
        {
            string search = $"\"{key}\"";
            int i = json.IndexOf(search, StringComparison.Ordinal);
            if (i < 0) return string.Empty;
            i = json.IndexOf('"', i + search.Length);
            if (i < 0) return string.Empty;
            i++;
            int end = json.IndexOf('"', i);
            return end < 0 ? string.Empty : json.Substring(i, end - i);
        }

        private static string ParseString(string json, string key)
        {
            string search = $"\"{key}\"";
            int i = json.IndexOf(search, StringComparison.Ordinal);
            if (i < 0) return string.Empty;
            i += search.Length;
            while (i < json.Length && (json[i] == ' ' || json[i] == ':')) i++;
            if (i >= json.Length || json[i] != '"') return string.Empty;
            i++; int end = json.IndexOf('"', i);
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
            foreach (var part in json.Substring(i + 1, end - i - 1).Split(','))
            {
                string t = part.Trim().Trim('"');
                if (!string.IsNullOrEmpty(t)) result.Add(t);
            }
            return result;
        }

        // ══════════════════════════════════════════════════════════════════
        //  Helpers
        // ══════════════════════════════════════════════════════════════════
        private static string FormatValue(float v, string unit)
        {
            string s = $"{v:F5}";
            return string.IsNullOrEmpty(unit) ? s : $"{s} {unit}";
        }

        private static void HR()
        {
            GUILayout.Space(4);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none,
                GUILayout.ExpandWidth(true), GUILayout.Height(1)), new Color(0.35f, 0.35f, 0.35f));
            GUILayout.Space(4);
        }
    }
}
