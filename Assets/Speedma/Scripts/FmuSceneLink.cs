// Assets/Speedma/Scripts/FmuSceneLink.cs
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

        [Header("FMU Output Names")]
        [SerializeField]
        private string outputAmpDinamo = "amp_dinamo_out";

        [SerializeField]
        private string outputAmpBateria = "amp_bateria_out";

        [SerializeField]
        private string outputVBat = "v_bat_out";

        [Header("FMU Input Names")]
        [SerializeField]
        private string inputSwDinamo = "sw_dinamo";

        [SerializeField]
        private string inputRheostat = "r_reostato";

        [Header("Rheostat")]
        [Tooltip("Resistance in ohms. Written by RheostatWheel at runtime.")]
        [SerializeField]
        private float rheostatValue = 2.0f;

        [SerializeField]
        private float rheostatMin = 0.0f;

        [SerializeField]
        private float rheostatMax = 10.0f;

        public float AmpDinamo { get; private set; }
        public float AmpBateria { get; private set; }
        public float VBat { get; private set; }

        /// <summary>Called every frame by RheostatWheel to drive r_reostato.</summary>
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

            // ── Read outputs ──────────────────────────────────────────
            AmpDinamo = sim.GetOutput(outputAmpDinamo);
            AmpBateria = sim.GetOutput(outputAmpBateria);
            VBat = sim.GetOutput(outputVBat);

            if (dynamoAmp != null)
                dynamoAmp.SetValue(Mathf.Max(0f, AmpDinamo));
            if (batteryAmp != null)
                batteryAmp.SetValue(Mathf.Max(0f, AmpBateria));

            // ── Write inputs ──────────────────────────────────────────
            sim.SetInput(inputRheostat, rheostatValue);
            UnityEngine.Debug.Log($"[Rheostat] sending r_reostato = {rheostatValue:F2}");
        }

        public void SetSwDinamo(bool on) => sim?.SetInput(inputSwDinamo, on);
    }
}
