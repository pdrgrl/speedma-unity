// Assets/Speedma/Scripts/Debug/FmuDebugController.cs  v2
// ============================================================
// Keyboard-driven FMU input controller.
// Uses FmuSceneLink (which talks to RemoteFmuSimulation).
//
// Controls:
//   C        → toggle sw_carga
//   D        → toggle sw_descarga
//   R / F    → R_carga +10 / -10 Ohm
//   T / G    → V_fonte  +10 / -10 V
//   `        → toggle HUD
// ============================================================

using UnityEngine;

namespace Speedma.Debug
{
    public class FmuDebugController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FmuSceneLink      sceneLink;
        [SerializeField] private RemoteFmuSimulation sim;

        [Header("Initial values")]
        [SerializeField] private float rCarga = 50f;
        [SerializeField] private float vFonte = 110f;

        private bool _swCarga = false;
        private bool _swDesc  = false;
        private bool _showHud = true;

        private GUIStyle _boxStyle, _labelStyle, _onStyle, _offStyle;
        private bool _stylesReady;

        private void Start() => PushAll();

        private void Update()
        {
            bool changed = false;

            if (Input.GetKeyDown(KeyCode.C))        { _swCarga = !_swCarga; changed = true; }
            if (Input.GetKeyDown(KeyCode.D))        { _swDesc  = !_swDesc;  changed = true; }
            if (Input.GetKeyDown(KeyCode.R))        { rCarga   = Mathf.Clamp(rCarga + 10f, 1f, 10000f); changed = true; }
            if (Input.GetKeyDown(KeyCode.F))        { rCarga   = Mathf.Clamp(rCarga - 10f, 1f, 10000f); changed = true; }
            if (Input.GetKeyDown(KeyCode.T))        { vFonte   = Mathf.Clamp(vFonte + 10f, 0f, 500f);   changed = true; }
            if (Input.GetKeyDown(KeyCode.G))        { vFonte   = Mathf.Clamp(vFonte - 10f, 0f, 500f);   changed = true; }
            if (Input.GetKeyDown(KeyCode.BackQuote)) _showHud  = !_showHud;

            if (changed) PushAll();
        }

        private void PushAll()
        {
            sceneLink?.SetSwitches(_swCarga, _swDesc);
            sceneLink?.SetRCarga(rCarga);
            sceneLink?.SetVFonte(vFonte);
        }

        private void OnGUI()
        {
            if (!_showHud) return;
            EnsureStyles();

            float w = 260f, h = 230f;
            GUI.Box(new Rect(10, 10, w, h), "", _boxStyle);

            float x = 18f, y = 18f, dy = 22f;

            Label(x, y, "── FMU Debug Controller ──");                                     y += dy;
            LabelColored(x, y, $"[C]  sw_carga    : {OnOff(_swCarga)}", _swCarga);  y += dy;
            LabelColored(x, y, $"[D]  sw_descarga : {OnOff(_swDesc)}",  _swDesc);   y += dy;
            Label(x, y, $"[R/F] R_carga   : {rCarga:F0} Ω");                          y += dy;
            Label(x, y, $"[T/G] V_fonte   : {vFonte:F0} V");                          y += dy;
            y += 6;

            float i_c = sceneLink != null ? sceneLink.CurrentAmps : 0f;
            float v_c = sceneLink != null ? sceneLink.VoltageCap  : 0f;
            bool ready = sim != null && sim.IsReady;

            Label(x, y, $"i_c  : {i_c:F4} A");  y += dy;
            Label(x, y, $"v_c  : {v_c:F2} V");  y += dy;
            Label(x, y, $"sim  : {(ready ? "READY" : "waiting..." )}"); y += dy;
            y += 4;
            Label(x, y, "[`] toggle HUD");
        }

        private static string OnOff(bool v) => v ? "ON  ✓" : "OFF ✗";

        private void Label(float x, float y, string text)
            => GUI.Label(new Rect(x, y, 240f, 20f), text, _labelStyle);

        private void LabelColored(float x, float y, string text, bool on)
        {
            var style = on ? _onStyle : _offStyle;
            GUI.Label(new Rect(x, y, 240f, 20f), text, style);
        }

        private void EnsureStyles()
        {
            if (_stylesReady) return;
            _stylesReady = true;

            _boxStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.75f)) }
            };
            _labelStyle = new GUIStyle(GUI.skin.label)
                { normal = { textColor = Color.white }, fontSize = 13 };
            _onStyle = new GUIStyle(_labelStyle)
                { normal = { textColor = new Color(0.2f, 0.95f, 0.3f) } };
            _offStyle = new GUIStyle(_labelStyle)
                { normal = { textColor = new Color(0.7f, 0.7f, 0.7f) } };
        }

        private static Texture2D MakeTex(int w, int h, Color col)
        {
            var t = new Texture2D(w, h);
            var p = new Color[w * h];
            for (int i = 0; i < p.Length; i++) p[i] = col;
            t.SetPixels(p); t.Apply(); return t;
        }
    }
}
