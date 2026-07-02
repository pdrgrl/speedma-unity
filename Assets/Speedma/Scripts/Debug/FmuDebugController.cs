// Assets/Speedma/Scripts/Debug/FmuDebugController.cs
// ============================================================
// Runtime FMU debug HUD.
// Shows the current scenario, session state, and the active FMU outputs.
// ============================================================

using UnityEngine;
using UnityEngine.InputSystem;
using Chamusca.Simulation;

namespace Speedma.Debug
{
    public class FmuDebugController : MonoBehaviour
    {
        private const float HudWidth = 300f;
        private const float HudHeight = 290f;
        private const float HudPadding = 16f;
        private const float HudLineHeight = 26f;
        private const float HudLabelWidth = 350f;

        [Header("References")]
        [SerializeField]
        private SpeedmaSimManager sim;

        [SerializeField]
        private ScenarioManager scenarioManager;

        [SerializeField]
        private ChamuscaSimController chamuscaController;

        private bool _showHud = true;

        private GUIStyle _boxStyle,
            _labelStyle,
            _onStyle,
            _offStyle;
        private bool _stylesReady;

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.backquoteKey.wasPressedThisFrame)
                _showHud = !_showHud;
        }

        private void OnGUI()
        {
            if (!_showHud)
                return;
            EnsureStyles();

            GUI.Box(new Rect(10, 10, HudWidth, HudHeight), string.Empty, _boxStyle);

            float x = 22f;
            float y = 20f;

            Label(x, y, "Chamusca Digital Twin");
            y += HudLineHeight + 2f;

            string scenarioName = scenarioManager != null
                ? scenarioManager.currentScenario.ToString()
                : "Unknown";
            Label(x, y, $"Scenario     : {scenarioName}");
            y += HudLineHeight + 2f;

            bool active = sim != null && sim.IsSessionActive;
            LabelColored(x, y, $"Session      : {(active ? "ACTIVE" : "INACTIVE")}", active);
            y += HudLineHeight;

            Label(x, y, $"Sim time     : {(sim != null ? sim.SimTime.ToString("F3") : "0.000")} s");
            y += HudLineHeight;

            float vLine = sim != null ? sim.GetOutput("v_line") : 0f;
            float iBat = sim != null ? sim.GetOutput("i_bat") : 0f;
            float soc = sim != null ? sim.GetOutput("soc") : 0f;
            float houseCurrent = sim != null ? sim.GetOutput("houseCurrent") : 0f;
            float houseLightIntensity = sim != null ? sim.GetOutput("houseLightIntensity") : 0f;
            float fuseState = sim != null ? sim.GetOutput("fuseState") : 0f;
            float breakerState = sim != null ? sim.GetOutput("breakerState") : 0f;
            float batteryVoltage = sim != null ? sim.GetOutput("batteryVoltage") : 0f;
            float effectiveCells = sim != null ? sim.GetOutput("effectiveCells") : 0f;

            Label(x, y, $"v_line       : {vLine, 8:F3} V");
            y += HudLineHeight;
            Label(x, y, $"batteryVolt  : {batteryVoltage, 8:F3} V");
            y += HudLineHeight;
            Label(x, y, $"houseCurrent : {houseCurrent, 8:F3} A");
            y += HudLineHeight;
            Label(x, y, $"i_bat        : {iBat, 8:F3} A");
            y += HudLineHeight;
            Label(x, y, $"soc          : {soc, 8:F5}");
            y += HudLineHeight;
            Label(x, y, $"houseLight   : {houseLightIntensity, 8:F3}");
            y += HudLineHeight;
            LabelColored(x, y, $"fuseState    : {OnOff(fuseState < 0.5f)}", fuseState < 0.5f);
            y += HudLineHeight;
            LabelColored(x, y, $"breakerState : {OnOff(breakerState < 0.5f)}", breakerState < 0.5f);
            y += HudLineHeight;
            Label(x, y, $"tap/cell     : {effectiveCells, 8:F0}");
            y += HudLineHeight + 4f;
            Label(x, y, "[`] toggle HUD");
        }

        private static string OnOff(bool v) => v ? "ON  ✓" : "OFF ✗";

        private void Label(float x, float y, string text) =>
            GUI.Label(new Rect(x, y, HudLabelWidth, HudLineHeight), text, _labelStyle);

        private void LabelColored(float x, float y, string text, bool on) =>
            GUI.Label(
                new Rect(x, y, HudLabelWidth, HudLineHeight),
                text,
                on ? _onStyle : _offStyle
            );

        private void EnsureStyles()
        {
            if (_stylesReady)
                return;
            _stylesReady = true;
            _boxStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.75f)) },
            };
            _labelStyle = new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = Color.white },
                fontSize = 16,
            };
            _onStyle = new GUIStyle(_labelStyle)
            {
                normal = { textColor = new Color(0.2f, 0.95f, 0.3f) },
            };
            _offStyle = new GUIStyle(_labelStyle)
            {
                normal = { textColor = new Color(0.7f, 0.7f, 0.7f) },
            };
        }

        private static Texture2D MakeTex(int w, int h, Color col)
        {
            var t = new Texture2D(w, h);
            var p = new Color[w * h];
            for (int i = 0; i < p.Length; i++)
                p[i] = col;
            t.SetPixels(p);
            t.Apply();
            return t;
        }
    }
}
