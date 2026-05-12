// Assets/Speedma/Scripts/FmuSceneLink.cs
// ============================================================
// Bridges SpeedmaSimManager to the 3D scene instruments.
// Modelo: ChamuscaDigitalTwin (Dínamo ASEA + Bateria Tudor)
//
// Outputs lidos do FMU:
//   amp_dinamo_out  → DynamoAmp
//   amp_bateria_out → BatteryAmp
//   v_bat_out       → (futuro VoltController)
//
// Inputs enviados ao FMU:
//   sw_dinamo       → liga/desliga dínamo
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
        [SerializeField] private string outputAmpDinamo  = "amp_dinamo_out";
        [SerializeField] private string outputAmpBateria = "amp_bateria_out";
        [SerializeField] private string outputVBat       = "v_bat_out";

        [Header("FMU Input Names")]
        [SerializeField] private string inputSwDinamo = "sw_dinamo";

        // Public read for debug HUD
        public float AmpDinamo  { get; private set; }
        public float AmpBateria { get; private set; }
        public float VBat       { get; private set; }

        private void Update()
        {
            if (sim == null || !sim.IsSessionActive) return;

            AmpDinamo  = sim.GetOutput(outputAmpDinamo);
            AmpBateria = sim.GetOutput(outputAmpBateria);
            VBat       = sim.GetOutput(outputVBat);

            if (dynamoAmp  != null) dynamoAmp.SetValue(Mathf.Max(0f, AmpDinamo));
            if (batteryAmp != null) batteryAmp.SetValue(Mathf.Max(0f, AmpBateria));
        }

        // ── Called by FmuDebugController ──────────────────────────────────
        public void SetSwDinamo(bool on) => sim?.SetInput(inputSwDinamo, on);
    }
}
