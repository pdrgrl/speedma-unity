using Speedma;
using UnityEngine;

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
            public HouseLightController houseLightController;
            public HouseLightController dependencyLightController;
            public HouseLightController panelLightController;
            public HouseLightController batteryRoomLightController;
            public BatteryCellSelector redutorCarga;
            public BatteryCellSelector redutorDescarga;
            public FmuToggleSwitch houseLightSwitch;
            public FmuToggleSwitch dependencyLightSwitch;
            public FmuToggleSwitch panelLightSwitch;
            public FmuToggleSwitch batteryRoomLightSwitch;

            [Header("FMU Variable Names")]
            public string varLineVoltage = "v_line";
            public string varDynamoCurrent = "i_din";
            public string varBatteryCurrent = "i_bat";
            public string varBatterySOC = "soc";
            public string varHouseLightIntensity = "houseLightIntensity";
            public string varDependencyLightIntensity = "depLightIntensity";
            public string varPanelLightIntensity = "panelLightIntensity";
            public string varBatteryRoomLightIntensity = "batteryRoomLightIntensity";
            public string varHouseLoad = "sw_casa_luz";
            public string varDependencyLoad = "sw_dep_luz";
            public string varPanelLight = "sw_quadro_luz";
            public string varBatteryRoomLight = "sw_bat_luz";
            public string varResetProtection = "resetProtection";

            [Header("Simulation State")]
            public float rheostat_pos = 0.2f;
            public float engine_rpm = 350f;
            public bool sw_dinamo_luz = false;
            public bool sw_carga_bat = false;
            public bool sw_bat_luz = false;
            public bool sw_bat_luz_lever = false;
            public bool sw_casa_luz = false;

            [SerializeField]
            private bool resetProtectionRequested = false;

            private SimulationScenario _lastScenario;

            private void Start()
            {
                if (scenarioManager != null && simManager != null)
                {
                    _lastScenario = scenarioManager.currentScenario;
                    string initialFmu = GetFmuNameForScenario(_lastScenario);
                    simManager.RestartSession(initialFmu);
                }
            }

            private void Update()
            {
                if (simManager == null || !simManager.IsSessionActive)
                    return;

                // Detect scenario changes at runtime
                if (scenarioManager != null && scenarioManager.currentScenario != _lastScenario)
                {
                    _lastScenario = scenarioManager.currentScenario;
                    string newFmu = GetFmuNameForScenario(_lastScenario);
                    simManager.RestartSession(newFmu);
                    return; // Wait for the new session to start before running updates
                }

                // 1. Send Inputs based on current scenario
                UpdateInputs();

                // 2. Read Outputs and Update Visuals
                float v = simManager.GetOutput(varLineVoltage);
                float i_din = simManager.GetOutput(varDynamoCurrent);
                float i_bat = simManager.GetOutput(varBatteryCurrent);
                float soc = simManager.GetOutput(varBatterySOC);
                float houseIntensity = simManager.GetOutput(varHouseLightIntensity);
                float depIntensity = simManager.GetOutput(varDependencyLightIntensity);
                float panelIntensity = simManager.GetOutput(varPanelLightIntensity);
                float batteryRoomIntensity = simManager.GetOutput(varBatteryRoomLightIntensity);

                if (lineVoltmeter != null)
                    lineVoltmeter.SetValue(v);
                if (dynamoAmmeter != null)
                    dynamoAmmeter.SetValue(i_din);
                if (batteryAmmeter != null)
                    batteryAmmeter.SetValue(i_bat);
                if (houseLightController != null)
                    houseLightController.UpdateFromIntensity(houseIntensity);
                if (dependencyLightController != null)
                    dependencyLightController.UpdateFromIntensity(depIntensity);
                if (panelLightController != null)
                    panelLightController.UpdateFromIntensity(panelIntensity);
                if (batteryRoomLightController != null)
                    batteryRoomLightController.UpdateFromIntensity(batteryRoomIntensity);
            }

            private void UpdateInputs()
            {
                bool houseOn = sw_casa_luz;
                bool batteryLightOn = sw_bat_luz;
                bool dependencyOn = false;
                bool panelLightOn = false;

                if (houseLightSwitch != null)
                    houseOn = houseLightSwitch.switchOn;
                if (batteryRoomLightSwitch != null)
                    batteryLightOn = batteryRoomLightSwitch.switchOn;
                if (dependencyLightSwitch != null)
                    dependencyOn = dependencyLightSwitch.switchOn;
                if (panelLightSwitch != null)
                    panelLightOn = panelLightSwitch.switchOn;

                simManager.SetInput(varHouseLoad, houseOn);
                simManager.SetInput(varDependencyLoad, dependencyOn);
                simManager.SetInput(varPanelLight, panelLightOn);
                simManager.SetInput(varBatteryRoomLight, batteryLightOn);

                if (redutorCarga != null)
                    simManager.SetInput("reductor_carga_pos", redutorCarga.currentCell - 40);
                if (redutorDescarga != null)
                    simManager.SetInput("reductor_descarga_pos", redutorDescarga.currentCell - 40);

                simManager.SetInput("rheostat_pos", rheostat_pos);
                simManager.SetInput("sw_bat_luz_lever", sw_bat_luz_lever);

                // Scenario specific logic
                switch (scenarioManager.currentScenario)
                {
                    case SimulationScenario.ScenarioA:
                        simManager.SetInput(varResetProtection, resetProtectionRequested);
                        simManager.SetInput("sw_engine", false);
                        simManager.SetInput("engine_rpm", 0f);
                        simManager.SetInput("sw_ac_mains", false);
                        simManager.SetInput("sw_carga_bat", false);
                        simManager.SetInput("sw_dinamo_luz", false);
                        resetProtectionRequested = false;
                        break;

                    case SimulationScenario.ScenarioB:
                        simManager.SetInput("sw_engine", true); // Crossley ON
                        simManager.SetInput("engine_rpm", engine_rpm);
                        simManager.SetInput("sw_ac_mains", false); // ASEA Motor OFF
                        simManager.SetInput("sw_dinamo_luz", sw_dinamo_luz);
                        simManager.SetInput("sw_carga_bat", sw_carga_bat);
                        break;

                    case SimulationScenario.ScenarioC:
                        simManager.SetInput("sw_engine", false); // Crossley OFF
                        simManager.SetInput("engine_rpm", 0f);
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

            public void ToggleBatteryLuzLever() => sw_bat_luz_lever = !sw_bat_luz_lever;

            public void RequestProtectionReset() => resetProtectionRequested = true;

            private string GetFmuNameForScenario(SimulationScenario scenario)
            {
                switch (scenario)
                {
                    case SimulationScenario.ScenarioA:
                        return "CenarioA.fmu";
                    case SimulationScenario.ScenarioB:
                    case SimulationScenario.ScenarioC:
                        return "CenarioB.fmu";
                    default:
                        return "CenarioB.fmu";
                }
            }
    }
}
