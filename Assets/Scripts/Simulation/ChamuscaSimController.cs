using UnityEngine;
using Speedma;

namespace Chamusca.Simulation
{
    /// <summary>
    /// Specialized SceneLink that handles the complex logic of the three historical scenarios.
    /// Bridges the UI inputs and FMU outputs.
    /// </summary>
    public class ChamuscaSimController : MonoBehaviour
    {
        [Header("Backend")]
        public SpeedmaSimManager simManager;
        public ScenarioManager scenarioManager;

        [Header("Sub-Controllers")]
        public VoltmeterPointer lineVoltmeter;
        public AmpController dynamoAmmeter;
        public AmpController batteryAmmeter;
        public HouseLightController lightController;
        public BatteryCellSelector redutor;

        [Header("FMU Variable Names")]
        public string varLineVoltage = "v_line";
        public string varDynamoCurrent = "i_din";
        public string varBatteryCurrent = "i_bat";
        public string varBatterySOC = "soc";

        [Header("Simulation State")]
        public bool sw_dinamo_luz = false;
        public bool sw_carga_bat = false;
        public bool sw_bat_luz = false;
        public bool sw_casa_luz = false;

        private void Update()
        {
            if (simManager == null || !simManager.IsSessionActive) return;

            // 1. Send Inputs based on current scenario
            UpdateInputs();

            // 2. Read Outputs and Update Visuals
            float v = simManager.GetOutput(varLineVoltage);
            float i_din = simManager.GetOutput(varDynamoCurrent);
            float i_bat = simManager.GetOutput(varBatteryCurrent);
            float soc = simManager.GetOutput(varBatterySOC);

            if (lineVoltmeter != null) lineVoltmeter.SetValue(v);
            if (dynamoAmmeter != null) dynamoAmmeter.SetValue(i_din);
            if (batteryAmmeter != null) batteryAmmeter.SetValue(i_bat);
            if (lightController != null) lightController.UpdateFromVoltage(v);
        }

        private void UpdateInputs()
        {
            // Common inputs
            simManager.SetInput("sw_casa_luz", sw_casa_luz);
            if (redutor != null) simManager.SetInput("reductor_pos", redutor.currentCell);

            // Scenario specific logic
            switch (scenarioManager.currentScenario)
            {
                case SimulationScenario.ScenarioA:
                    simManager.SetInput("sw_bat_luz", sw_bat_luz);
                    simManager.SetInput("sw_carga_bat", false);
                    simManager.SetInput("sw_dinamo_luz", false);
                    break;

                case SimulationScenario.ScenarioB:
                    simManager.SetInput("sw_engine", true); // Crossley ON
                    simManager.SetInput("sw_dinamo_luz", sw_dinamo_luz);
                    simManager.SetInput("sw_carga_bat", sw_carga_bat);
                    break;

                case SimulationScenario.ScenarioC:
                    simManager.SetInput("sw_ac_mains", true); // ASEA Motor ON
                    simManager.SetInput("sw_carga_bat", sw_carga_bat);
                    simManager.SetInput("sw_dinamo_luz", sw_dinamo_luz);
                    break;
            }
        }

        // Methods for UI / Interaction
        public void ToggleHouseLights() => sw_casa_luz = !sw_casa_luz;
        public void ToggleBatteryCharge() => sw_carga_bat = !sw_carga_bat;
        public void ToggleDynamoLight() => sw_dinamo_luz = !sw_dinamo_luz;
    }
}
