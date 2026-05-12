// Assets/Editor/RCSimTester.cs
// Unity Editor Window to test the CircuitoRC_CargaDescarga simulation
// via the speedma-backend FMU simulation API.
//
// Usage:
//   Unity menu → Speedma → RC Sim Tester
//   1. Start Backend (uvicorn app.api:app --reload --port 8000)
//   2. Click "Check Health" to confirm the FMU is ready
//   3. Click "Start Session" to allocate a simulation instance
//   4. Use "Step" or "Auto-Step" to advance the simulation
//   5. Watch v_c (voltage) plotted in real-time in the editor window

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Speedma.Editor
{
    public class RCSimTester : EditorWindow
    {
        // ── Config ────────────────────────────────────────────────────────
        private string _backendUrl = "http://localhost:8000";
        private string _sessionId  = "";
        private float  _dt         = 0.02f;   // step size (seconds)
        private bool   _autoStep   = false;
        private double _lastAutoStepTime;
        private const double AUTO_STEP_INTERVAL = 0.05; // 20 Hz in editor

        // ── Simulation state ──────────────────────────────────────────────
        private float _simTime    = 0f;
        private float _vC         = 0f;       // v_c from FMU output
        private string _statusMsg = "Idle";
        private bool   _requesting = false;

        // ── Plot data ─────────────────────────────────────────────────────
        private const int MAX_SAMPLES = 400;
        private List<float> _times  = new List<float>();
        private List<float> _values = new List<float>();
        private Vector2     _scrollPos;

        // ── Styles ────────────────────────────────────────────────────────
        private GUIStyle _titleStyle;
        private GUIStyle _labelBoldStyle;
        private GUIStyle _statusOkStyle;
        private GUIStyle _statusErrStyle;
        private bool     _stylesInit = false;

        // ─────────────────────────────────────────────────────────────────
        [MenuItem("Speedma/RC Sim Tester")]
        public static void ShowWindow()
        {
            var win = GetWindow<RCSimTester>("RC Sim Tester");
            win.minSize = new Vector2(460, 560);
        }

        private void OnEnable()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
            _autoStep = false;
        }

        // ── Auto-step loop ────────────────────────────────────────────────
        private void OnEditorUpdate()
        {
            if (!_autoStep || _requesting || string.IsNullOrEmpty(_sessionId))
                return;

            double now = EditorApplication.timeSinceStartup;
            if (now - _lastAutoStepTime >= AUTO_STEP_INTERVAL)
            {
                _lastAutoStepTime = now;
                _ = DoStepAsync();
            }
        }

        // ── Init styles ───────────────────────────────────────────────────
        private void InitStyles()
        {
            if (_stylesInit) return;
            _stylesInit = true;

            _titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter
            };
            _labelBoldStyle = new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Bold };
            _statusOkStyle  = new GUIStyle(EditorStyles.label) { normal = { textColor = new Color(0.2f, 0.8f, 0.2f) } };
            _statusErrStyle = new GUIStyle(EditorStyles.label) { normal = { textColor = new Color(0.9f, 0.2f, 0.2f) } };
        }

        // ── GUI ───────────────────────────────────────────────────────────
        private void OnGUI()
        {
            InitStyles();
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            GUILayout.Space(8);
            GUILayout.Label("CircuitoRC_CargaDescarga — Simulation Tester", _titleStyle);
            GUILayout.Space(4);
            DrawSeparator();

            // ── Config section
            GUILayout.Label("Backend Configuration", _labelBoldStyle);
            _backendUrl = EditorGUILayout.TextField("Backend URL", _backendUrl);
            _dt         = EditorGUILayout.Slider("Step size (dt) [s]", _dt, 0.001f, 0.5f);
            GUILayout.Space(4);

            // ── Health check
            if (GUILayout.Button("🔍  Check Health", GUILayout.Height(28)))
                _ = DoHealthCheckAsync();

            DrawSeparator();

            // ── Session controls
            GUILayout.Label("Session", _labelBoldStyle);
            using (new EditorGUI.DisabledScope(!string.IsNullOrEmpty(_sessionId)))
            {
                if (GUILayout.Button("▶  Start Session", GUILayout.Height(28)))
                    _ = DoStartSessionAsync();
            }

            if (!string.IsNullOrEmpty(_sessionId))
            {
                EditorGUILayout.LabelField("Session ID", _sessionId);

                GUILayout.Space(4);
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("⏩  Step", GUILayout.Height(26)))
                        _ = DoStepAsync();

                    string autoLabel = _autoStep ? "⏸  Stop Auto" : "⏯  Auto-Step";
                    if (GUILayout.Button(autoLabel, GUILayout.Height(26)))
                        _autoStep = !_autoStep;

                    if (GUILayout.Button("🔄  Reset", GUILayout.Height(26)))
                        _ = DoResetAsync();

                    if (GUILayout.Button("⏹  Stop", GUILayout.Height(26)))
                        _ = DoStopAsync();
                }
            }

            DrawSeparator();

            // ── Live data
            GUILayout.Label("Live Output", _labelBoldStyle);
            EditorGUILayout.LabelField("Simulation Time (t)", $"{_simTime:F4} s");
            EditorGUILayout.LabelField("Capacitor Voltage (v_c)", $"{_vC:F4} V");

            DrawSeparator();

            // ── Plot
            GUILayout.Label("v_c over Time", _labelBoldStyle);
            DrawPlot(GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none,
                     GUILayout.ExpandWidth(true), GUILayout.Height(180)));

            DrawSeparator();

            // ── Status
            GUILayout.Label("Status", _labelBoldStyle);
            bool isError = _statusMsg.StartsWith("Error", StringComparison.OrdinalIgnoreCase)
                        || _statusMsg.StartsWith("Fail",  StringComparison.OrdinalIgnoreCase);
            GUILayout.Label(_statusMsg, isError ? _statusErrStyle : _statusOkStyle,
                            GUILayout.ExpandWidth(true));

            GUILayout.Space(8);
            EditorGUILayout.EndScrollView();
        }

        // ── Helpers ───────────────────────────────────────────────────────
        private void DrawSeparator()
        {
            GUILayout.Space(4);
            Rect r = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none,
                                              GUILayout.ExpandWidth(true), GUILayout.Height(1));
            EditorGUI.DrawRect(r, new Color(0.4f, 0.4f, 0.4f, 1f));
            GUILayout.Space(4);
        }

        private void DrawPlot(Rect rect)
        {
            // Background
            EditorGUI.DrawRect(rect, new Color(0.12f, 0.12f, 0.12f));

            if (_times.Count < 2) return;

            float tMin = 0f, tMax = Mathf.Max(_times[_times.Count - 1], 7f);
            float vMin = -5f, vMax = 110f;

            Handles.color = new Color(0.3f, 0.3f, 0.3f);
            // Grid lines
            for (int g = 0; g <= 4; g++)
            {
                float y = rect.y + rect.height * (1f - (float)g / 4f);
                Handles.DrawLine(new Vector3(rect.x, y), new Vector3(rect.xMax, y));
            }

            // Plot v_c curve
            Handles.color = new Color(0.2f, 0.8f, 0.3f);
            for (int i = 1; i < _times.Count; i++)
            {
                float x0 = rect.x + Mathf.InverseLerp(tMin, tMax, _times[i - 1]) * rect.width;
                float y0 = rect.y + (1f - Mathf.InverseLerp(vMin, vMax, _values[i - 1])) * rect.height;
                float x1 = rect.x + Mathf.InverseLerp(tMin, tMax, _times[i]) * rect.width;
                float y1 = rect.y + (1f - Mathf.InverseLerp(vMin, vMax, _values[i])) * rect.height;
                Handles.DrawLine(new Vector3(x0, y0), new Vector3(x1, y1));
            }

            // Y-axis labels
            GUI.color = Color.gray;
            GUI.Label(new Rect(rect.x + 2, rect.y,          50, 16), "110 V");
            GUI.Label(new Rect(rect.x + 2, rect.y + rect.height * 0.5f - 8, 50, 16), "55 V");
            GUI.Label(new Rect(rect.x + 2, rect.yMax - 16,  50, 16), "0 V");
            // X-axis labels
            GUI.Label(new Rect(rect.x,           rect.yMax - 16, 30, 16), "0s");
            GUI.Label(new Rect(rect.xMax - 30,   rect.yMax - 16, 30, 16), "7s");
            GUI.color = Color.white;
        }

        // ── HTTP calls (async) ────────────────────────────────────────────

        private async Task DoHealthCheckAsync()
        {
            _statusMsg = "Checking health...";
            Repaint();
            string json = await GetAsync($"{_backendUrl}/sim/health");
            _statusMsg = string.IsNullOrEmpty(json) ? "Error: no response" : $"Health: {json}";
            Repaint();
        }

        private async Task DoStartSessionAsync()
        {
            _statusMsg = "Starting session...";
            _requesting = true;
            Repaint();

            string json = await PostAsync($"{_backendUrl}/sim/start", "{}");
            if (!string.IsNullOrEmpty(json))
            {
                // parse session_id from {"session_id":"..."}
                var parsed = JsonUtility.FromJson<StartResponse>(json);
                _sessionId = parsed?.session_id ?? "";
                _statusMsg = string.IsNullOrEmpty(_sessionId)
                    ? $"Error parsing session_id: {json}"
                    : $"Session started: {_sessionId}";
                _times.Clear();
                _values.Clear();
                _simTime = 0f;
                _vC      = 0f;
            }
            else
            {
                _statusMsg = "Error: backend unreachable. Is uvicorn running?";
            }

            _requesting = false;
            Repaint();
        }

        private async Task DoStepAsync()
        {
            if (_requesting || string.IsNullOrEmpty(_sessionId)) return;
            _requesting = true;

            string body = $"{{\"session_id\":\"{_sessionId}\",\"dt\":{_dt.ToString(System.Globalization.CultureInfo.InvariantCulture)}}}";
            string json = await PostAsync($"{_backendUrl}/sim/step", body);

            if (!string.IsNullOrEmpty(json))
            {
                var parsed = JsonUtility.FromJson<StepResponse>(json);
                if (parsed != null)
                {
                    _simTime = parsed.t;
                    // Try to read v_c from the raw JSON (JsonUtility doesn't support dicts)
                    _vC = ParseFloatFromJson(json, "v_c");

                    if (_times.Count >= MAX_SAMPLES)
                    {
                        _times.RemoveAt(0);
                        _values.RemoveAt(0);
                    }
                    _times.Add(_simTime);
                    _values.Add(_vC);
                    _statusMsg = $"t={_simTime:F3}s  v_c={_vC:F3}V";
                }
            }
            else
            {
                _statusMsg = "Error on step — stopping auto-step";
                _autoStep  = false;
            }

            _requesting = false;
            Repaint();
        }

        private async Task DoResetAsync()
        {
            _statusMsg = "Resetting...";
            Repaint();
            await PostAsync($"{_backendUrl}/sim/reset",
                            $"{{\"session_id\":\"{_sessionId}\"}}" );
            _times.Clear();
            _values.Clear();
            _simTime = 0f;
            _vC      = 0f;
            _autoStep = false;
            _statusMsg = "Reset OK";
            Repaint();
        }

        private async Task DoStopAsync()
        {
            _autoStep = false;
            await PostAsync($"{_backendUrl}/sim/stop",
                            $"{{\"session_id\":\"{_sessionId}\"}}" );
            _sessionId = "";
            _times.Clear();
            _values.Clear();
            _statusMsg = "Session stopped";
            Repaint();
        }

        // ── HTTP helpers ──────────────────────────────────────────────────

        private static async Task<string> GetAsync(string url)
        {
            using var req = UnityWebRequest.Get(url);
            req.timeout = 5;
            var op = req.SendWebRequest();
            while (!op.isDone) await Task.Yield();
            if (req.result != UnityWebRequest.Result.Success)
                return string.Empty;
            return req.downloadHandler.text;
        }

        private static async Task<string> PostAsync(string url, string jsonBody)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(jsonBody);
            using var req = new UnityWebRequest(url, "POST");
            req.uploadHandler   = new UploadHandlerRaw(bytes);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.timeout = 5;
            var op = req.SendWebRequest();
            while (!op.isDone) await Task.Yield();
            if (req.result != UnityWebRequest.Result.Success)
                return string.Empty;
            return req.downloadHandler.text;
        }

        // ── Very lightweight JSON value extractor (avoids Newtonsoft dep) ─
        private static float ParseFloatFromJson(string json, string key)
        {
            string search = $"\"{key}\"";
            int idx = json.IndexOf(search, StringComparison.Ordinal);
            if (idx < 0) return 0f;
            idx += search.Length;
            // skip whitespace and colon
            while (idx < json.Length && (json[idx] == ' ' || json[idx] == ':')) idx++;
            int end = idx;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '.' ||
                                          json[end] == '-' || json[end] == 'e' || json[end] == 'E' ||
                                          json[end] == '+')) end++;
            if (end == idx) return 0f;
            return float.TryParse(json.Substring(idx, end - idx),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float val) ? val : 0f;
        }

        // ── JSON deserialization helpers ──────────────────────────────────
        [Serializable] private class StartResponse { public string session_id; }
        [Serializable] private class StepResponse  { public float t; }
    }
}
