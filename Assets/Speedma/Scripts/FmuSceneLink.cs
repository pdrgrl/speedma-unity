// Assets/Speedma/Scripts/FmuSceneLink.cs
// ============================================================
// Bridges SpeedmaSimManager to the 3D scene instruments.
// Modelo: ChamuscaDigitalTwin v3
//
// Outputs lidos do FMU:
//   amp_dinamo_out  → DynamoAmp needle
//   amp_bateria_out → BatteryAmp needle
//   v_bat_out       → (futuro VoltController)
//   lamp_on_out     → lampLight
//
// Inputs enviados ao FMU:
//   sw_dinamo       → liga/desliga dínamo
//   r_reostato      → resistência do reostato (driven by RheostatWheel)
//   sw_luz          → liga/desliga lâmpada
// ============================================================

using UnityEngine;

namespace Speedma
{
    public class FmuSceneLink : MonoBehaviour
    {
        [Header("Simulation Backend")]
        [SerializeField]
        private SpeedmaSimManager sim;

        [Header("Instruments")]
        [SerializeField]
        private AmpController dynamoAmp;

        [SerializeField]
        private AmpController batteryAmp;

        [Header("Lamp")]
        [Tooltip("The Unity Light component on the lamp GameObject.")]
        [SerializeField]
        private Light lampLight;

        [Header("FMU Output Names")]
        [SerializeField]
        private string outputAmpDinamo = "amp_dinamo_out";

        [SerializeField]
        private string outputAmpBateria = "amp_bateria_out";

        [SerializeField]
        private string outputVBat = "v_bat_out";

        [SerializeField]
        private string outputLampOn = "lamp_on_out";

        [Header("FMU Input Names")]
        [SerializeField]
        private string inputSwDinamo = "sw_dinamo";

        [SerializeField]
        private string inputRheostat = "r_reostato";

        [SerializeField]
        private string inputSwLuz = "sw_luz";

        [Header("Rheostat")]
        [Tooltip("Resistance in ohms. Written by RheostatWheel at runtime.")]
        [SerializeField]
        private float rheostatValue = 2.0f;

        [SerializeField]
        private float rheostatMin = 0.0f;

        [SerializeField]
        private float rheostatMax = 10.0f;

        // ── Private state ──────────────────────────────────────────────
        private bool _swDinamo = false;
        private bool _swLuz = false;

        // ── Public reads (for debug HUD) ───────────────────────────────
        public float AmpDinamo { get; private set; }
        public float AmpBateria { get; private set; }
        public float VBat { get; private set; }
        public float LampOn { get; private set; }
        public bool SwDinamo => _swDinamo;
        public bool SwLuz => _swLuz;

        // ── Rheostat property (written by RheostatWheel) ───────────────
        public float RheostatValue
        {
            get => rheostatValue;
            set => rheostatValue = Mathf.Clamp(value, rheostatMin, rheostatMax);
        }
        public float RheostatMin => rheostatMin;
        public float RheostatMax => rheostatMax;

        private void Update()
        {
            if (sim == null || !sim.IsSessionActive)
                return;

            // ── Read outputs ───────────────────────────────────────────
            AmpDinamo = sim.GetOutput(outputAmpDinamo);
            AmpBateria = sim.GetOutput(outputAmpBateria);
            VBat = sim.GetOutput(outputVBat);
            LampOn = sim.GetOutput(outputLampOn);

            if (dynamoAmp != null)
                dynamoAmp.SetValue(Mathf.Max(0f, AmpDinamo));
            if (batteryAmp != null)
                batteryAmp.SetValue(Mathf.Max(0f, AmpBateria));
            if (lampLight != null)
                lampLight.enabled = LampOn > 0.5f;

            // ── Write inputs ───────────────────────────────────────────
            sim.SetInput(inputSwDinamo, _swDinamo);
            sim.SetInput(inputRheostat, rheostatValue);
            sim.SetInput(inputSwLuz, _swLuz);
        }

        // ── Public setters (called by SimpleSwitchTest, FmuDebugController) ──
        public void SetSwDinamo(bool on) => _swDinamo = on;

        public void SetSwLuz(bool on) => _swLuz = on;

        public void ToggleSwDinamo() => _swDinamo = !_swDinamo;

        public void ToggleSwLuz() => _swLuz = !_swLuz;
    }
}
