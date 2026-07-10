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
            
            [Header("New Unified Controllers")]
            [Tooltip("The new Double-Selector (Reductor) controller.")]
            public ReductorDuploController reductorController;
            [Tooltip("The new plug-in voltmeter selector bar.")]
            public VoltmeterSelectorFuse voltmeterSelector;

            [Header("Fmu Toggle Switches")]
            public FmuToggleSwitch houseLightSwitch;
            public FmuToggleSwitch dependencyLightSwitch;
            public FmuToggleSwitch panelLightSwitch;
            public FmuToggleSwitch batteryRoomLightSwitch;
            
            [Tooltip("Single knife lever switch: Dynamo LUZ")]
            public FmuToggleSwitch dynamoLuzSwitch;
            [Tooltip("Single knife lever switch: Carga bateria")]
            public FmuToggleSwitch chargeBatSwitch;
            [Tooltip("Single knife lever switch: Bateria LUZ")]
            public FmuToggleSwitch batLuzLeverSwitch;

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
                float i_din = simManager.GetOutput(varDynamoCurrent);
                float i_bat = simManager.GetOutput(varBatteryCurrent);
                float soc = simManager.GetOutput(varBatterySOC);
                float houseIntensity = simManager.GetOutput(varHouseLightIntensity);
                float depIntensity = simManager.GetOutput(varDependencyLightIntensity);
                float panelIntensity = simManager.GetOutput(varPanelLightIntensity);
                float batteryRoomIntensity = simManager.GetOutput(varBatteryRoomLightIntensity);

                // Check if breaker is debug-tripped and override outputs to 0
                FmuBreakerSwitch breaker = Object.FindFirstObjectByType<FmuBreakerSwitch>();
                bool debugTripped = breaker != null && breaker.IsDebugTripped;
                if (debugTripped)
                {
                    i_din = 0f;
                    i_bat = 0f;
                    houseIntensity = 0f;
                    depIntensity = 0f;
                    panelIntensity = 0f;
                    batteryRoomIntensity = 0f;
                }

                // Update Voltmeter depending on voltmeter selector plug slot
                if (lineVoltmeter != null)
                {
                    float targetV = 0f;
                    if (debugTripped)
                    {
                        targetV = 0f;
                    }
                    else if (voltmeterSelector != null)
                    {
                        switch (voltmeterSelector.selectedSlot)
                        {
                            case 0: // Descarga: displays battery voltage (discharge branch)
                                targetV = simManager.GetOutput("batteryVoltage");
                                break;
                            case 1: // Carga: displays battery voltage (charge branch)
                                targetV = simManager.GetOutput("batteryVoltageCarga");
                                break;
                            case 2: // Dínamo: displays dynamo terminal voltage
                                // In Scenario A dynamo is 0V; in B/C it reads dynamo_voltage
                                targetV = (scenarioManager.currentScenario == SimulationScenario.ScenarioA) ? 0f : simManager.GetOutput("dynamo_voltage");
                                break;
                        }
                    }
                    else
                    {
                        // Fallback to general line voltage if selector is not present
                        targetV = simManager.GetOutput(varLineVoltage);
                    }
                    lineVoltmeter.SetValue(targetV);
                }

                if (dynamoAmmeter != null)
                    dynamoAmmeter.SetValue(i_din);
                if (batteryAmmeter != null)
                    batteryAmmeter.SetValue(Mathf.Abs(i_bat)); // Map absolute value for center-zero / positive sweeps
                
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
                bool houseOn = false;
                bool batteryLightOn = false;
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

                // Map single knife levers from their physical components
                bool swDinamoLuz = dynamoLuzSwitch != null ? dynamoLuzSwitch.switchOn : false;
                bool swCargaBat = chargeBatSwitch != null ? chargeBatSwitch.switchOn : false;
                bool swBatLuzLever = batLuzLeverSwitch != null ? batLuzLeverSwitch.switchOn : false;

                simManager.SetInput("sw_dinamo_luz", swDinamoLuz);
                simManager.SetInput("sw_carga_bat", swCargaBat);
                simManager.SetInput("sw_bat_luz_lever", swBatLuzLever);

                // Map Double-Selector cell index taps
                if (reductorController != null)
                {
                    // Map cell indices 41-60 to 1-20 for Modelica
                    simManager.SetInput("reductor_descarga_pos", Mathf.Clamp(reductorController.dischargeCell - 40, 1, 20));
                    simManager.SetInput("reductor_carga_pos", Mathf.Clamp(reductorController.chargeCell - 40, 1, 20));
                    // In Scenario A, map reductor_pos to discharge arm pos
                    simManager.SetInput("reductor_pos", Mathf.Clamp(reductorController.dischargeCell - 40, 1, 20));
                }

                // Map excitation rheostat position
                simManager.SetInput("rheostat_pos", rheostat_pos);

                // Unconditionally map protection reset in all scenarios
                simManager.SetInput(varResetProtection, resetProtectionRequested);
                resetProtectionRequested = false;

                // Scenario specific logic
                switch (scenarioManager.currentScenario)
                {
                    case SimulationScenario.ScenarioA:
                        simManager.SetInput("sw_engine", false);
                        simManager.SetInput("engine_rpm", 0f);
                        simManager.SetInput("sw_ac_mains", false);
                        simManager.SetInput("sw_carga_bat", false);
                        simManager.SetInput("sw_dinamo_luz", false);
                        break;

                    case SimulationScenario.ScenarioB:
                        simManager.SetInput("sw_engine", true); // Crossley ON
                        simManager.SetInput("engine_rpm", engine_rpm);
                        simManager.SetInput("sw_ac_mains", false); // ASEA Motor OFF
                        simManager.SetInput("sw_dinamo_luz", swDinamoLuz);
                        simManager.SetInput("sw_carga_bat", swCargaBat);
                        break;

                    case SimulationScenario.ScenarioC:
                        simManager.SetInput("sw_engine", false); // Crossley OFF
                        simManager.SetInput("engine_rpm", 375f); // AC Motor drives generator at constant grid speed
                        simManager.SetInput("sw_ac_mains", true); // ASEA Motor ON
                        simManager.SetInput("sw_carga_bat", swCargaBat);
                        simManager.SetInput("sw_dinamo_luz", swDinamoLuz);
                        break;
                }
            }

            // Methods for UI / Interaction
            public void ToggleHouseLights()
            {
                if (houseLightSwitch != null)
                    houseLightSwitch.switchOn = !houseLightSwitch.switchOn;
            }

            public void ToggleBatteryCharge()
            {
                if (chargeBatSwitch != null)
                    chargeBatSwitch.switchOn = !chargeBatSwitch.switchOn;
            }

            public void ToggleDynamoLight()
            {
                if (dynamoLuzSwitch != null)
                    dynamoLuzSwitch.switchOn = !dynamoLuzSwitch.switchOn;
            }

            public void ToggleBatteryLuzLever()
            {
                if (batLuzLeverSwitch != null)
                    batLuzLeverSwitch.switchOn = !batLuzLeverSwitch.switchOn;
            }

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
