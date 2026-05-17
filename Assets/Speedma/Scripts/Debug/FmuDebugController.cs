// Assets/Speedma/Scripts/Debug/FmuDebugController.cs
// ============================================================
// Keyboard-driven FMU input controller.
// Modelo: ChamuscaDigitalTwin v3
//
// Controls:
//   D   → toggle sw_dinamo
//   L   → toggle sw_luz
//   `   → toggle HUD
// ============================================================

using UnityEngine;

namespace Speedma.Debug
{
    public class FmuDebugController : MonoBehaviour
    {
        private const float HudWidth = 430f;
        private const float HudHeight = 330f;
        private const float HudPadding = 18f;
        private const float HudLineHeight = 30f;
        private const float HudLabelWidth = 394f;

        [Header("References")]
        [SerializeField]
        private FmuSceneLink sceneLink;

        private bool _showHud = true;

        private GUIStyle _boxStyle,
            _labelStyle,
            _onStyle,
            _offStyle;
        private bool _stylesReady;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
                sceneLink?.ToggleSwDinamo();
            if (Input.GetKeyDown(KeyCode.L))
                sceneLink?.ToggleSwLuz();
            if (Input.GetKeyDown(KeyCode.BackQuote))
                _showHud = !_showHud;
        }

        private void OnGUI()
        {
            if (!_showHud)
                return;
            EnsureStyles();

            GUI.Box(new Rect(10, 10, HudWidth, HudHeight), string.Empty, _boxStyle);

            float x = 24f;
            float y = 22f;

            Label(x, y, "Chamusca Digital Twin");
            y += HudLineHeight + 4f;

            LabelColored(
                x,
                y,
                $"[D]  sw_dinamo : {OnOff(sceneLink != null && sceneLink.SwDinamo)}",
                sceneLink != null && sceneLink.SwDinamo
            );
            y += HudLineHeight;
            LabelColored(
                x,
                y,
                $"[L]  sw_luz    : {OnOff(sceneLink != null && sceneLink.SwLuz)}",
                sceneLink != null && sceneLink.SwLuz
            );
            y += HudLineHeight + 4f;

            float ampD = sceneLink != null ? sceneLink.AmpDinamo : 0f;
            float ampB = sceneLink != null ? sceneLink.AmpBateria : 0f;
            float vBat = sceneLink != null ? sceneLink.VBat : 0f;
            float lamp = sceneLink != null ? sceneLink.LampOn : 0f;
            float rheo = sceneLink != null ? sceneLink.RheostatValue : 0f;

            Label(x, y, $"amp_dinamo   : {ampD,8:F4} A");
            y += HudLineHeight;
            Label(x, y, $"amp_bateria  : {ampB,8:F4} A");
            y += HudLineHeight;
            Label(x, y, $"v_bat        : {vBat,8:F3} V");
            y += HudLineHeight;
            Label(x, y, $"r_reostato   : {rheo,8:F2} Ω");
            y += HudLineHeight;
            LabelColored(x, y, $"lamp_on      : {OnOff(lamp > 0.5f)}", lamp > 0.5f);
            y += HudLineHeight + 6f;
            Label(x, y, "[`] toggle HUD");
        }

        private static string OnOff(bool v) => v ? "ON  ✓" : "OFF ✗";

        private void Label(float x, float y, string text) =>
            GUI.Label(new Rect(x, y, HudLabelWidth, HudLineHeight), text, _labelStyle);

        private void LabelColored(float x, float y, string text, bool on) =>
            GUI.Label(new Rect(x, y, HudLabelWidth, HudLineHeight), text, on ? _onStyle : _offStyle);

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
                fontSize = 18,
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
