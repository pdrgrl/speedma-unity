// Assets/Speedma/Scripts/Debug/FmuDebugController.cs
// ============================================================
// Temporary debug controller — keyboard drives FMU inputs.
// Remove or disable when Rheostat/Reducer scripts are ready.
//
// Controls:
//   C        → toggle sw_carga   (charge switch)
//   D        → toggle sw_descarga (discharge switch)
//   R / F    → R_carga +10 / -10 Ohm
//   T / G    → V_fonte  +10 / -10 V
//   Space    → reset simulation
//   Backquote (`) → show/hide HUD
// ============================================================

using UnityEngine;

namespace Speedma.Debug
{
    public class FmuDebugController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FmuSceneLink sceneLink;
        [SerializeField] private SpeedmaSimManager simManager;

        [Header("Initial values")]
        [SerializeField] private float rCarga   = 50f;
        [SerializeField] private float vFonte   = 110f;

        private bool  _swCarga    = false;
        private bool  _swDesc     = false;
        private bool  _showHud    = true;

        // HUD style
        private GUIStyle _boxStyle;
        private GUIStyle _labelStyle;
        private bool     _stylesReady;

        private void Start()
        {
            PushAll();
        }

        private void Update()
        {
            bool changed = false;

            if (Input.GetKeyDown(KeyCode.C))       { _swCarga = !_swCarga; changed = true; }
            if (Input.GetKeyDown(KeyCode.D))       { _swDesc  = !_swDesc;  changed = true; }
            if (Input.GetKeyDown(KeyCode.R))       { rCarga   = Mathf.Clamp(rCarga + 10f, 1f, 10000f); changed = true; }
            if (Input.GetKeyDown(KeyCode.F))       { rCarga   = Mathf.Clamp(rCarga - 10f, 1f, 10000f); changed = true; }
            if (Input.GetKeyDown(KeyCode.T))       { vFonte   = Mathf.Clamp(vFonte + 10f, 0f, 500f);   changed = true; }
            if (Input.GetKeyDown(KeyCode.G))       { vFonte   = Mathf.Clamp(vFonte - 10f, 0f, 500f);   changed = true; }
            if (Input.GetKeyDown(KeyCode.BackQuote)) _showHud = !_showHud;

            if (changed) PushAll();
        }

        private void PushAll()
        {
            sceneLink?.SetSwitches(_swCarga, _swDesc);
            sceneLink?.SetRCarga(rCarga);
            simManager?.SetInput("V_fonte", vFonte);
        }

        private void OnGUI()
        {
            if (!_showHud) return;
            EnsureStyles();

            float w = 260f, h = 220f;
            Rect bg = new Rect(10, 10, w, h);
            GUI.Box(bg, "", _boxStyle);

            float x = 18f, y = 18f, dy = 22f;

            Label(x, y, "── FMU Debug Controller ──");     y += dy;
            Label(x, y, $"[C]  sw_carga    : {Bool(_swCarga)}");  y += dy;
            Label(x, y, $"[D]  sw_descarga : {Bool(_swDesc)}");   y += dy;
            Label(x, y, $"[R/F] R_carga   : {rCarga:F0} Ω");      y += dy;
            Label(x, y, $"[T/G] V_fonte   : {vFonte:F0} V");      y += dy;
            y += 6;
            Label(x, y, $"i_c  : {(simManager != null ? simManager.GetOutput("i_c_out") : 0f):F4} A"); y += dy;
            Label(x, y, $"v_c  : {(simManager != null ? simManager.GetOutput("v_c_out") : 0f):F2} V"); y += dy;
            Label(x, y, $"t    : {(simManager != null ? simManager.SimTime : 0f):F3} s");              y += dy;
            y += 4;
            Label(x, y, "[`] toggle HUD");
        }

        private static string Bool(bool v) => v ? "ON  ✓" : "OFF ✗";

        private void Label(float x, float y, string text)
            => GUI.Label(new Rect(x, y, 240f, 20f), text, _labelStyle);

        private void EnsureStyles()
        {
            if (_stylesReady) return;
            _stylesReady = true;

            _boxStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.72f)) }
            };
            _labelStyle = new GUIStyle(GUI.skin.label)
            {
                normal  = { textColor = Color.white },
                fontSize = 13,
            };
        }

        private static Texture2D MakeTex(int w, int h, Color col)
        {
            var tex = new Texture2D(w, h);
            var pix = new Color[w * h];
            for (int i = 0; i < pix.Length; i++) pix[i] = col;
            tex.SetPixels(pix);
            tex.Apply();
            return tex;
        }
    }
}
