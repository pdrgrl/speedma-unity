// Assets/Speedma/Scripts/FmuSceneLink.cs
// ============================================================
// Bridges SpeedmaSimManager to the 3D scene instruments.
//
// - Reads i_c_out / v_c_out from SpeedmaSimManager each frame
// - Drives DynamoAmp  (positive i_c = charging)
// - Drives BatteryAmp (negative i_c = discharging)
// - Exposes SetRCarga / SetSwitches for FmuDebugController
//   and future RheostatInput / ReducerInput scripts
// ============================================================

using UnityEngine;

namespace Speedma
{
    public class FmuSceneLink : MonoBehaviour
    {
        [Header("Simulation Backend")]
        [SerializeField] private SpeedmaSimManager sim;

        [Header("Instruments")]
        [SerializeField] private AmpController dynamoAmp;
        [SerializeField] private AmpController batteryAmp;

        [Header("FMU Output Names")]
        [SerializeField] private string outputCurrent = "i_c_out";
        [SerializeField] private string outputVoltage = "v_c_out";

        [Header("FMU Input Names")]
        [SerializeField] private string inputRCarga  = "R_carga";
        [SerializeField] private string inputSwCarga = "sw_carga";
        [SerializeField] private string inputSwDesc  = "sw_descarga";

        // Public read for debug HUD
        public float CurrentAmps { get; private set; }
        public float VoltageCap  { get; private set; }

        private void Update()
        {
            if (sim == null || !sim.IsSessionActive) return;

            CurrentAmps = sim.GetOutput(outputCurrent);
            VoltageCap  = sim.GetOutput(outputVoltage);

            if (dynamoAmp  != null) dynamoAmp.SetValue( Mathf.Max(0f,  CurrentAmps));
            if (batteryAmp != null) batteryAmp.SetValue(Mathf.Max(0f, -CurrentAmps));
        }

        // ── Called by FmuDebugController / RheostatInput / ReducerInput ──
        public void SetRCarga(float value)              => sim?.SetInput(inputRCarga,  value);
        public void SetSwitches(bool carga, bool desc)  { sim?.SetInput(inputSwCarga, carga); sim?.SetInput(inputSwDesc, desc); }
        public void SetVFonte(float value)              => sim?.SetInput("V_fonte",    value);
    }
}
