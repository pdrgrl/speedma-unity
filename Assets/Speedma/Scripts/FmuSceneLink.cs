// Assets/Speedma/Scripts/FmuSceneLink.cs
// ============================================================
// Bridges the running FMU session (via SpeedmaSimManager) to
// the 3D scene instruments.
//
// - Reads i_c_out and v_c_out from the FMU every frame
// - Drives the two AmpControllers
// - Exposes R_carga and sw_* as properties so RheostatInput
//   and ReducerInput can push values into the FMU
// ============================================================

using UnityEngine;

namespace Speedma
{
    public class FmuSceneLink : MonoBehaviour
    {
        [Header("Instruments")]
        [SerializeField] private AmpController dynamoAmp;   // amperímetro do dinamo
        [SerializeField] private AmpController batteryAmp;  // amperímetro da bateria

        [Header("FMU Output Names")]
        [SerializeField] private string outputCurrent = "i_c_out";
        [SerializeField] private string outputVoltage = "v_c_out";

        [Header("FMU Input Names")]
        [SerializeField] private string inputRCarga    = "R_carga";
        [SerializeField] private string inputSwCarga   = "sw_carga";
        [SerializeField] private string inputSwDesc    = "sw_descarga";

        // ── Runtime state (read by RheostatInput / ReducerInput) ──────
        private float _rCarga    = 50f;
        private bool  _swCarga   = false;
        private bool  _swDesc    = false;

        // Last read values (public for UI / debug)
        public float CurrentAmps   { get; private set; }
        public float VoltageCap    { get; private set; }

        // ── FMU session reference ─────────────────────────────────────
        // We look for SpeedmaSimManager in the scene; it owns the HTTP loop.
        private SpeedmaSimManager _sim;

        private void Start()
        {
            _sim = FindFirstObjectByType<SpeedmaSimManager>();
            if (_sim == null)
                Debug.LogWarning("[FmuSceneLink] SpeedmaSimManager not found in scene.");
        }

        private void Update()
        {
            if (_sim == null || !_sim.IsSessionActive) return;

            // ── Read outputs from last FMU step ──────────────────────
            CurrentAmps = _sim.GetOutput(outputCurrent);
            VoltageCap  = _sim.GetOutput(outputVoltage);

            // Dynamo amp: charging current (positive i_c)
            if (dynamoAmp  != null) dynamoAmp.SetValue( Mathf.Max(0f,  CurrentAmps));

            // Battery amp: discharge current (negative i_c, shown as positive)
            if (batteryAmp != null) batteryAmp.SetValue(Mathf.Max(0f, -CurrentAmps));
        }

        // ── Called by RheostatInput ───────────────────────────────────
        public void SetRCarga(float value)
        {
            _rCarga = value;
            _sim?.SetInput(inputRCarga, _rCarga);
        }

        // ── Called by ReducerInput ────────────────────────────────────
        public void SetSwitches(bool carga, bool descarga)
        {
            _swCarga = carga;
            _swDesc  = descarga;
            _sim?.SetInput(inputSwCarga, _swCarga);
            _sim?.SetInput(inputSwDesc,  _swDesc);
        }
    }
}
