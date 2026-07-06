// Assets/Scripts/Speedma/Debug/FmuDebugController.cs
// ============================================================
// Runtime FMU debug HUD.
// Dynamically queries variable metadata from the backend on session start,
// renders color-coded telemetry, and plots outputs in real-time.
// Uses GUILayout.Window for perfect auto-height sizing.
// ============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using Chamusca.Simulation;

namespace Speedma.Debug
{
    public class FmuDebugController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private SpeedmaSimManager sim;

        [SerializeField]
        private ScenarioManager scenarioManager;

        [SerializeField]
        private ChamuscaSimController chamuscaController;

        [Header("Canvas HUD References")]
        [SerializeField]
        private GameObject hudPanel;
        [SerializeField]
        private TMPro.TextMeshProUGUI telemetryText;
        [SerializeField]
        private UnityEngine.UI.Button toggleButton;
        [SerializeField]
        private UnityEngine.UI.Button btnScenarioA;
        [SerializeField]
        private UnityEngine.UI.Button btnScenarioB;
        [SerializeField]
        private UnityEngine.UI.Button btnScenarioC;

        private bool _showHud = false; // Hidden by default now
        private string _lastSessionId = "";
        
        // Discovered variables metadata
        private class VarMeta
        {
            public string Name;
            public string Type; // Real | Boolean | Integer | String
            public string Causality; // input | output | local | parameter
            public string Variability;
            public float? Start;
            public float? Min;
            public float? Max;
            public string Description;
            public string Unit;
        }

        private readonly List<VarMeta> _variables = new();
        private bool _fetchingVariables = false;
        private Vector2 _scrollPos;

        // Plot telemetry data
        private class PlotSeries
        {
            public List<float> Times = new();
            public List<float> Values = new();
        }
        private readonly Dictionary<string, PlotSeries> _plots = new();
        private const int MaxSamples = 200;

        private static readonly Color[] PlotColors =
        {
            new(0.2f, 0.9f, 0.3f), // Green
            new(0.3f, 0.6f, 1.0f), // Blue
            new(1.0f, 0.7f, 0.2f), // Orange
            new(1.0f, 0.3f, 0.4f), // Red
            new(0.8f, 0.3f, 1.0f), // Purple
            new(0.3f, 0.9f, 0.9f), // Cyan
            new(1.0f, 0.5f, 0.8f)  // Pink
        };

        private Rect _windowRect = new Rect(15, 15, 350, 0); // height=0 tells GUILayout to auto-size
        private Texture2D _lineTex;
        private GUIStyle _lineStyle;

        public static FmuDebugController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public bool IsMouseOverGui()
        {
            if (!_showHud) return false;
            if (Keyboard.current == null) return false;
            
            Vector2 mousePos = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
            float imguiMouseY = Screen.height - mousePos.y;
            
            // Checks if pointer is inside the dynamically sized windowRect bounds
            return _windowRect.Contains(new Vector2(mousePos.x, imguiMouseY));
        }

        private void Start()
        {
            if (toggleButton != null)
            {
                toggleButton.onClick.AddListener(ToggleHud);
            }
            LinkScenarioButtons();
            UpdateHudVisibility();
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.backquoteKey.wasPressedThisFrame)
            {
                ToggleHud();
            }

            if (sim != null && sim.IsSessionActive)
            {
                // Detect new simulation sessions
                if (sim.SessionId != _lastSessionId)
                {
                    _lastSessionId = sim.SessionId;
                    _plots.Clear();
                    StartCoroutine(FetchVariablesCoroutine());
                }

                // Record history samples for plotting Real variables
                float t = sim.SimTime;
                foreach (var v in _variables)
                {
                    if ((v.Causality == "output" || v.Causality == "local") && v.Type == "Real")
                    {
                        if (!_plots.TryGetValue(v.Name, out var series))
                        {
                            series = _plots[v.Name] = new PlotSeries();
                        }

                        if (series.Times.Count == 0 || t > series.Times[series.Times.Count - 1])
                        {
                            if (series.Times.Count >= MaxSamples)
                            {
                                series.Times.RemoveAt(0);
                                series.Values.RemoveAt(0);
                            }
                            series.Times.Add(t);
                            series.Values.Add(sim.GetOutput(v.Name));
                        }
                    }
                }
            }

            if (_showHud && telemetryText != null)
            {
                UpdateTelemetryText();
            }
        }

        public void SetCanvasUIReferences(
            GameObject panel, 
            TMPro.TextMeshProUGUI text, 
            UnityEngine.UI.Button button,
            UnityEngine.UI.Button btnA,
            UnityEngine.UI.Button btnB,
            UnityEngine.UI.Button btnC
        ) {
            hudPanel = panel;
            telemetryText = text;
            toggleButton = button;
            btnScenarioA = btnA;
            btnScenarioB = btnB;
            btnScenarioC = btnC;
            
            if (toggleButton != null)
            {
                toggleButton.onClick.RemoveAllListeners();
                toggleButton.onClick.AddListener(ToggleHud);
            }
            LinkScenarioButtons();
            UpdateHudVisibility();
        }

        private void LinkScenarioButtons()
        {
            if (scenarioManager == null) return;

            if (btnScenarioA != null)
            {
                btnScenarioA.onClick.RemoveAllListeners();
                btnScenarioA.onClick.AddListener(() => scenarioManager.SetScenario(SimulationScenario.ScenarioA));
            }
            if (btnScenarioB != null)
            {
                btnScenarioB.onClick.RemoveAllListeners();
                btnScenarioB.onClick.AddListener(() => scenarioManager.SetScenario(SimulationScenario.ScenarioB));
            }
            if (btnScenarioC != null)
            {
                btnScenarioC.onClick.RemoveAllListeners();
                btnScenarioC.onClick.AddListener(() => scenarioManager.SetScenario(SimulationScenario.ScenarioC));
            }
        }

        private void ToggleHud()
        {
            _showHud = !_showHud;
            UpdateHudVisibility();
        }

        private void UpdateHudVisibility()
        {
            if (hudPanel != null)
            {
                hudPanel.SetActive(_showHud);
            }
        }

        private IEnumerator FetchVariablesCoroutine()
        {
            _fetchingVariables = true;
            _variables.Clear();
            
            string backendUrl = "http://localhost:8000";
            if (sim != null)
            {
                var field = sim.GetType().GetField("backendUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (field != null)
                    backendUrl = (string)field.GetValue(sim);
            }

            string url = $"{backendUrl}/sim/variables/{_lastSessionId}";
            using UnityWebRequest webRequest = UnityWebRequest.Get(url);
            webRequest.timeout = 5;

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                ParseVariablesFull(webRequest.downloadHandler.text);
            }
            else
            {
                UnityEngine.Debug.LogWarning($"[FmuDebugController] Failed to fetch variables: {webRequest.error}");
            }
            _fetchingVariables = false;
        }

        private void UpdateTelemetryText()
        {
            if (telemetryText == null) return;

            string scenarioName = scenarioManager != null ? scenarioManager.currentScenario.ToString() : "Unknown";
            bool active = sim != null && sim.IsSessionActive;
            float simTime = sim != null ? sim.SimTime : 0f;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"<b>Scenario</b>: {scenarioName}");
            sb.AppendLine($"<b>Session</b>: {(active ? "<color=#33F34D>ACTIVE</color>" : "<color=#B5B5B5>INACTIVE</color>")}");
            sb.AppendLine($"<b>Sim Time</b>: {simTime:F3} s");
            sb.AppendLine("--------------------------------");

            if (_variables.Count > 0)
            {
                foreach (var v in _variables)
                {
                    if (v.Causality == "output" || v.Causality == "local")
                    {
                        float val = sim.GetOutput(v.Name);
                        string fmt = v.Type == "Boolean" 
                            ? (val > 0.5f ? "TRUE" : "FALSE") 
                            : $"{val:F2}";
                        string unitStr = string.IsNullOrEmpty(v.Unit) ? "" : $" {v.Unit}";
                        sb.AppendLine($"<b>{v.Name}</b>: {fmt}{unitStr}");
                    }
                }
            }
            else
            {
                sb.AppendLine("No variables fetched yet or offline.");
            }

            telemetryText.text = sb.ToString();
        }

        private void OnGUI()
        {
            if (!_showHud) return;

            // Make window background semi-transparent
            GUI.color = new Color(1f, 1f, 1f, 0.92f);
            
            // GUILayout.Window automatically fits its content height (0 value initializes auto-sizing)
            _windowRect = GUILayout.Window(100, _windowRect, DrawMonitorWindow, "Simulation Monitor", GUILayout.Width(350));
        }

        private void DrawMonitorWindow(int windowId)
        {
            string scenarioName = scenarioManager != null ? scenarioManager.currentScenario.ToString() : "Unknown";
            bool active = sim != null && sim.IsSessionActive;
            float simTime = sim != null ? sim.SimTime : 0f;

            GUIStyle sectionStyle = new GUIStyle(GUI.skin.label) { richText = true, fontStyle = FontStyle.Bold };
            GUIStyle inputLabelStyle = new GUIStyle(GUI.skin.label) { richText = true, normal = { textColor = new Color(0.9f, 0.85f, 0.5f) } };
            GUIStyle voltLabelStyle = new GUIStyle(GUI.skin.label) { richText = true, normal = { textColor = new Color(0.4f, 0.8f, 1f) } };
            GUIStyle ampLabelStyle = new GUIStyle(GUI.skin.label) { richText = true, normal = { textColor = new Color(1f, 0.6f, 0.2f) } };
            GUIStyle socLabelStyle = new GUIStyle(GUI.skin.label) { richText = true, normal = { textColor = new Color(0.3f, 0.9f, 0.4f) } };
            GUIStyle textLabelStyle = new GUIStyle(GUI.skin.label) { richText = true };

            GUILayout.Label($"<b>Scenario:</b> <color=#4fd1c5>{scenarioName}</color>", sectionStyle);
            GUILayout.Label($"<b>Sim Time:</b> {simTime:F2}s ({(active ? "Active" : "Offline")})");
            HR();

            if (_fetchingVariables)
            {
                GUILayout.Label("Fetching FMU metadata...", textLabelStyle);
            }
            else if (_variables.Count == 0)
            {
                GUILayout.Label("Start the simulation to display variables dynamically.", textLabelStyle);
            }
            else
            {
                // Taller scroll view to make the window taller
                _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(280));

                // 1. INPUTS
                GUILayout.Label("<b>[ INPUTS ]</b>", sectionStyle);
                foreach (var v in _variables)
                {
                    if (v.Causality != "input") continue;

                    float curVal = 0f;
                    if (v.Type == "Boolean")
                    {
                        bool bVal = false;
                        if (v.Name == "sw_casa_luz" && chamuscaController != null) bVal = chamuscaController.sw_casa_luz;
                        else if (v.Name == "sw_carga_bat" && chamuscaController != null) bVal = chamuscaController.sw_carga_bat;
                        else if (v.Name == "sw_dinamo_luz" && chamuscaController != null) bVal = chamuscaController.sw_dinamo_luz;
                        else if (v.Name == "sw_bat_luz_lever" && chamuscaController != null) bVal = chamuscaController.sw_bat_luz_lever;
                        
                        GUI.enabled = false;
                        GUILayout.Toggle(bVal, $" {v.Name} ({(bVal ? "ON" : "OFF")})");
                        GUI.enabled = true;
                    }
                    else
                    {
                        if (v.Name == "rheostat_pos" && chamuscaController != null) curVal = chamuscaController.rheostat_pos;
                        else if (v.Name == "engine_rpm" && chamuscaController != null) curVal = chamuscaController.engine_rpm;
                        else if (v.Name == "reductor_carga_pos" && chamuscaController != null && chamuscaController.redutorCarga != null) curVal = chamuscaController.redutorCarga.currentCell - 40;
                        else if (v.Name == "reductor_descarga_pos" && chamuscaController != null && chamuscaController.redutorDescarga != null) curVal = chamuscaController.redutorDescarga.currentCell - 40;

                        float min = v.Min ?? 0f;
                        float max = v.Max ?? (v.Name.Contains("rpm") ? 350f : 1f);

                        GUILayout.BeginHorizontal();
                        GUILayout.Label($"{v.Name}: <b>{curVal:F1}</b> {(string.IsNullOrEmpty(v.Unit) ? "" : v.Unit)}", inputLabelStyle, GUILayout.Width(190));
                        GUI.enabled = false;
                        GUILayout.HorizontalSlider(curVal, min, max, GUILayout.Width(100));
                        GUI.enabled = true;
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.Space(8);

                // 2. OUTPUTS
                GUILayout.Label("<b>[ OUTPUTS ]</b>", sectionStyle);
                int plotColorIdx = 0;
                foreach (var v in _variables)
                {
                    if (v.Causality != "output" && v.Causality != "local") continue;
                    if (v.Name == "breakerState" || v.Name.StartsWith("fuse")) continue;

                    float val = sim.GetOutput(v.Name);
                    GUIStyle style = textLabelStyle;
                    float sliderMax = 1f;

                    if (v.Name.StartsWith("v_") || v.Name.Contains("Voltage") || v.Name.Contains("Volt"))
                    {
                        style = voltLabelStyle;
                        sliderMax = 150f;
                    }
                    else if (v.Name.StartsWith("i_") || v.Name.Contains("Current") || v.Name.Contains("house") || v.Name.Contains("dep"))
                    {
                        style = ampLabelStyle;
                        sliderMax = 150f;
                    }
                    else if (v.Name == "soc")
                    {
                        style = socLabelStyle;
                        sliderMax = 1f;
                    }

                    string unitStr = string.IsNullOrEmpty(v.Unit) ? "" : $" {v.Unit}";
                    
                    Color seriesColor = Color.white;
                    if (v.Type == "Real")
                    {
                        seriesColor = PlotColors[plotColorIdx % PlotColors.Length];
                        plotColorIdx++;
                    }

                    GUILayout.BeginHorizontal();
                    if (v.Type == "Real")
                    {
                        // Draw a solid color block inline
                        Rect colorRect = GUILayoutUtility.GetRect(10, 10, GUILayout.Width(10), GUILayout.Height(10));
                        Color oldColor = GUI.color;
                        GUI.color = seriesColor;
                        GUI.DrawTexture(colorRect, _lineTex != null ? _lineTex : Texture2D.whiteTexture);
                        GUI.color = oldColor;
                        GUILayout.Space(4);
                    }
                    GUILayout.Label($"{v.Name}: <b>{val:F1}{unitStr}</b>", style, GUILayout.Width(180));
                    
                    GUI.enabled = false;
                    GUILayout.HorizontalSlider(val, v.Name == "soc" ? 0f : (v.Name.StartsWith("i_bat") ? -25f : 0f), sliderMax, GUILayout.Width(100));
                    GUI.enabled = true;
                    GUILayout.EndHorizontal();
                }
                GUILayout.Space(8);

                // 3. PROTECTIONS
                GUILayout.Label("<b>[ PROTECTIONS ]</b>", sectionStyle);
                GUILayout.BeginHorizontal();
                foreach (var v in _variables)
                {
                    if (v.Name == "breakerState" || v.Name.StartsWith("fuse"))
                    {
                        float state = sim.GetOutput(v.Name);
                        string shortLabel = v.Name.Replace("State", "").ToUpper();
                        
                        if (shortLabel == "FUSEBATTERYROOM") shortLabel = "FUSE RM";
                        else if (shortLabel == "FUSEBATTERY") shortLabel = "FUSE BAT";
                        else if (shortLabel == "FUSECHARGE") shortLabel = "FUSE CHG";
                        else if (shortLabel == "FUSEDISCHARGE") shortLabel = "FUSE DIS";
                        else if (shortLabel == "FUSEDYNAMO") shortLabel = "FUSE DYN";
                        else if (shortLabel == "FUSEHOUSE") shortLabel = "FUSE HSE";

                        DrawIndicatorBox(shortLabel, state > 0.5f);
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.EndScrollView();
            }

            // 4. LIVE GRAPH PLOT (Cleaned of text legend overlaps)
            DrawLivePlot();

            HR();

            // Scenario Switcher
            if (scenarioManager != null)
            {
                GUILayout.Label("<b>[ CHANGE SCENARIO ]</b>", sectionStyle);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Scenario A", GUILayout.Height(22)))
                    scenarioManager.SetScenario(SimulationScenario.ScenarioA);
                if (GUILayout.Button("Scenario B", GUILayout.Height(22)))
                    scenarioManager.SetScenario(SimulationScenario.ScenarioB);
                if (GUILayout.Button("Scenario C", GUILayout.Height(22)))
                    scenarioManager.SetScenario(SimulationScenario.ScenarioC);
                GUILayout.EndHorizontal();
            }

            GUI.DragWindow(); // Allows dragging the window around if desired
        }

        private void DrawLivePlot()
        {
            if (_variables.Count == 0) return;

            // Enforce a texture initialization
            if (_lineTex == null)
            {
                _lineTex = new Texture2D(1, 1);
                _lineTex.SetPixel(0, 0, Color.white);
                _lineTex.Apply();
            }

            Rect plotRect = GUILayoutUtility.GetRect(100, 110, GUILayout.ExpandWidth(true));
            Color oldBg = GUI.backgroundColor;
            
            // Draw plot background box
            GUI.backgroundColor = Color.black;
            GUI.Box(plotRect, "");
            GUI.backgroundColor = oldBg;

            // Find min/max values dynamically from active Real variables
            float minVal = float.MaxValue;
            float maxVal = -float.MaxValue;
            float maxTime = 1f;

            foreach (var kv in _plots)
            {
                var list = kv.Value.Values;
                var times = kv.Value.Times;
                if (times.Count > 0 && times[times.Count - 1] > maxTime)
                    maxTime = times[times.Count - 1];

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] < minVal) minVal = list[i];
                    if (list[i] > maxVal) maxVal = list[i];
                }
            }

            if (minVal >= maxVal || minVal == float.MaxValue)
            {
                minVal = -1f;
                maxVal = 1f;
            }
            float pad = (maxVal - minVal) * 0.1f;
            minVal -= pad;
            maxVal += pad;

            // Draw horizontal lines representing scale
            for (int g = 0; g <= 3; g++)
            {
                float ratio = g / 3f;
                float y = plotRect.y + plotRect.height * (1f - ratio);
                float val = Mathf.Lerp(minVal, maxVal, ratio);
                DrawLine(new Vector2(plotRect.x, y), new Vector2(plotRect.xMax, y), new Color(0.2f, 0.2f, 0.2f, 0.5f), 1f);
            }

            // Plot lines for each output series
            int colorIdx = 0;
            foreach (var kv in _plots)
            {
                string name = kv.Key;
                var times = kv.Value.Times;
                var values = kv.Value.Values;
                if (times.Count < 2) continue;

                Color seriesColor = PlotColors[colorIdx % PlotColors.Length];
                colorIdx++;

                for (int i = 1; i < times.Count; i++)
                {
                    float x0 = plotRect.x + Mathf.InverseLerp(0f, maxTime, times[i - 1]) * plotRect.width;
                    float y0 = plotRect.y + (1f - Mathf.InverseLerp(minVal, maxVal, values[i - 1])) * plotRect.height;
                    
                    float x1 = plotRect.x + Mathf.InverseLerp(0f, maxTime, times[i]) * plotRect.width;
                    float y1 = plotRect.y + (1f - Mathf.InverseLerp(minVal, maxVal, values[i])) * plotRect.height;

                    DrawLine(new Vector2(x0, y0), new Vector2(x1, y1), seriesColor, 2f);
                }
            }
        }

        private void DrawIndicatorBox(string label, bool tripped)
        {
            Color oldBg = GUI.backgroundColor;
            
            // Draw boxes with a slightly smaller font size (10) to make sure words fit
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 9,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            boxStyle.normal.textColor = Color.white;

            GUI.backgroundColor = tripped ? new Color(1f, 0.2f, 0.2f, 1f) : new Color(0.2f, 0.9f, 0.2f, 1f);
            
            GUILayout.Box(label, boxStyle, GUILayout.Width(72), GUILayout.Height(22));
            
            GUI.backgroundColor = oldBg;
        }

        private void DrawLine(Vector2 start, Vector2 end, Color color, float width)
        {
            if (_lineTex == null)
            {
                _lineTex = new Texture2D(1, 1);
                _lineTex.SetPixel(0, 0, Color.white);
                _lineTex.Apply();
            }

            Color oldColor = GUI.color;
            GUI.color = color;

            Vector2 d = end - start;
            float angle = Mathf.Rad2Deg * Mathf.Atan2(d.y, d.x);
            float magnitude = d.magnitude;

            Matrix4x4 matrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(angle, start);
            // Draw solid line utilizing DrawTexture instead of GUI.Box
            GUI.DrawTexture(new Rect(start.x, start.y - width / 2, magnitude, width), _lineTex);
            GUI.matrix = matrix;
            GUI.color = oldColor;
        }

        private void HR()
        {
            GUILayout.Space(4);
            GUIStyle dividerStyle = new GUIStyle();
            dividerStyle.normal.background = Texture2D.whiteTexture;
            Color oldColor = GUI.color;
            GUI.color = new Color(0.35f, 0.35f, 0.35f, 0.5f);
            GUILayout.Box("", dividerStyle, GUILayout.Height(1), GUILayout.ExpandWidth(true));
            GUI.color = oldColor;
            GUILayout.Space(4);
        }

        // Parsing helpers
        private void ParseVariablesFull(string json)
        {
            int start = json.IndexOf("\"variables\"", StringComparison.Ordinal);
            if (start < 0) return;
            start = json.IndexOf('{', start + 11);
            if (start < 0) return;

            int pos = start + 1;
            while (pos < json.Length)
            {
                int q1 = json.IndexOf('"', pos);
                if (q1 < 0) break;
                int q2 = json.IndexOf('"', q1 + 1);
                if (q2 < 0) break;
                string varName = json.Substring(q1 + 1, q2 - q1 - 1);
                if (varName == "}") break;

                int ob = json.IndexOf('{', q2);
                if (ob < 0) break;
                
                int depth = 1;
                int cb = ob + 1;
                while (cb < json.Length && depth > 0)
                {
                    if (json[cb] == '{') depth++;
                    else if (json[cb] == '}') depth--;
                    cb++;
                }
                cb--;
                string inner = json.Substring(ob, cb - ob + 1);

                if (!string.IsNullOrEmpty(varName))
                {
                    _variables.Add(new VarMeta
                    {
                        Name = varName,
                        Type = ExtractQ(inner, "type"),
                        Causality = ExtractQ(inner, "causality"),
                        Variability = ExtractQ(inner, "variability"),
                        Start = ExtractNullableFloat(inner, "start"),
                        Min = ExtractNullableFloat(inner, "min"),
                        Max = ExtractNullableFloat(inner, "max"),
                        Description = ExtractQ(inner, "description"),
                        Unit = ExtractQ(inner, "unit")
                    });
                }

                pos = cb + 1;
                while (pos < json.Length && (json[pos] == ',' || json[pos] == ' ' || json[pos] == '\n' || json[pos] == '\r'))
                    pos++;
                if (pos < json.Length && json[pos] == '}') break;
            }
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
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '.' || json[end] == '-' || json[end] == 'e' || json[end] == 'E' || json[end] == '+'))
                end++;
            if (end == i) return null;
            return float.TryParse(json.Substring(i, end - i), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float v) ? v : (float?)null;
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
    }
}
