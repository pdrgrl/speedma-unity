// Assets/Speedma/Scripts/Debug/FmuDebugController.cs
// ============================================================
// Runtime FMU debug HUD.
// Shows the current scenario, session state, and the active FMU outputs.
// ============================================================

using UnityEngine;
using UnityEngine.InputSystem;
using Chamusca.Simulation;

namespace Speedma.Debug
{
    public class FmuDebugController : MonoBehaviour
    {
        private const float HudWidth = 300f;
        private const float HudHeight = 290f;
        private const float HudPadding = 16f;
        private const float HudLineHeight = 26f;
        private const float HudLabelWidth = 350f;

        [Header("References")]
        [SerializeField]
        private SpeedmaSimManager sim;

        [SerializeField]
        private ScenarioManager scenarioManager;

        [SerializeField]
        private ChamuscaSimController chamuscaController;

        [Header("Canvas HUD References")]
        [SerializeField]
        private GameObject hudPanel;
        [SerializeField]
        private TMPro.TextMeshProUGUI telemetryText;
        [SerializeField]
        private UnityEngine.UI.Button toggleButton;
        [SerializeField]
        private UnityEngine.UI.Button btnScenarioA;
        [SerializeField]
        private UnityEngine.UI.Button btnScenarioB;
        [SerializeField]
        private UnityEngine.UI.Button btnScenarioC;
        [SerializeField]
        private UnityEngine.UI.Slider rpmSlider;

        private bool _showHud = false; // Hidden by default now

        private void Start()
        {
            if (toggleButton != null)
            {
                toggleButton.onClick.AddListener(ToggleHud);
            }
            if (rpmSlider != null && chamuscaController != null)
            {
                rpmSlider.minValue = 0f;
                rpmSlider.maxValue = 500f;
                rpmSlider.value = chamuscaController.engine_rpm;
                rpmSlider.onValueChanged.AddListener((val) => {
                    chamuscaController.engine_rpm = val;
                });
            }
            LinkScenarioButtons();
            UpdateHudVisibility();
        }

        private void Update()
        {
            // Allow toggling with backquote keyboard shortcut too
            if (Keyboard.current != null && Keyboard.current.backquoteKey.wasPressedThisFrame)
            {
                ToggleHud();
            }

            if (_showHud)
            {
                if (telemetryText != null)
                {
                    UpdateTelemetryText();
                }

                // Show/hide RPM slider depending on whether we are in Scenario B or C
                if (rpmSlider != null && scenarioManager != null)
                {
                    bool showSlider = scenarioManager.currentScenario == SimulationScenario.ScenarioB ||
                                      scenarioManager.currentScenario == SimulationScenario.ScenarioC;
                    GameObject container = rpmSlider.transform.parent.gameObject;
                    if (container.activeSelf != showSlider)
                    {
                        container.SetActive(showSlider);
                    }
                }
            }
        }

        public void SetCanvasUIReferences(
            GameObject panel, 
            TMPro.TextMeshProUGUI text, 
            UnityEngine.UI.Button button,
            UnityEngine.UI.Button btnA,
            UnityEngine.UI.Button btnB,
            UnityEngine.UI.Button btnC,
            UnityEngine.UI.Slider slider
        ) {
            hudPanel = panel;
            telemetryText = text;
            toggleButton = button;
            btnScenarioA = btnA;
            btnScenarioB = btnB;
            btnScenarioC = btnC;
            rpmSlider = slider;
            
            if (toggleButton != null)
            {
                toggleButton.onClick.RemoveAllListeners();
                toggleButton.onClick.AddListener(ToggleHud);
            }
            if (rpmSlider != null && chamuscaController != null)
            {
                rpmSlider.minValue = 0f;
                rpmSlider.maxValue = 500f;
                rpmSlider.value = chamuscaController.engine_rpm;
                rpmSlider.onValueChanged.RemoveAllListeners();
                rpmSlider.onValueChanged.AddListener((val) => {
                    chamuscaController.engine_rpm = val;
                });
            }
            LinkScenarioButtons();
            UpdateHudVisibility();
        }

        private void LinkScenarioButtons()
        {
            if (scenarioManager == null) return;

            if (btnScenarioA != null)
            {
                btnScenarioA.onClick.RemoveAllListeners();
                btnScenarioA.onClick.AddListener(() => scenarioManager.SetScenario(SimulationScenario.ScenarioA));
            }
            if (btnScenarioB != null)
            {
                btnScenarioB.onClick.RemoveAllListeners();
                btnScenarioB.onClick.AddListener(() => scenarioManager.SetScenario(SimulationScenario.ScenarioB));
            }
            if (btnScenarioC != null)
            {
                btnScenarioC.onClick.RemoveAllListeners();
                btnScenarioC.onClick.AddListener(() => scenarioManager.SetScenario(SimulationScenario.ScenarioC));
            }
        }

        private void ToggleHud()
        {
            _showHud = !_showHud;
            UpdateHudVisibility();
        }

        private void UpdateHudVisibility()
        {
            if (hudPanel != null)
            {
                hudPanel.SetActive(_showHud);
            }
        }

        private void UpdateTelemetryText()
        {
            string scenarioName = scenarioManager != null ? scenarioManager.currentScenario.ToString() : "Unknown";
            bool active = sim != null && sim.IsSessionActive;
            float simTime = sim != null ? sim.SimTime : 0f;

            float vLine = sim != null ? sim.GetOutput("v_line") : 0f;
            float iDin = sim != null ? sim.GetOutput("i_din") : 0f;
            float iBat = sim != null ? sim.GetOutput("i_bat") : 0f;
            float soc = sim != null ? sim.GetOutput("soc") : 0f;
            
            float iHouse = sim != null ? sim.GetOutput("i_house") : 0f;
            float iDep = sim != null ? sim.GetOutput("i_dep") : 0f;

            float fuseDynamo = sim != null ? sim.GetOutput("fuseDynamoState") : 0f;
            float fuseCharge = sim != null ? sim.GetOutput("fuseChargeState") : 0f;
            float fuseDischarge = sim != null ? sim.GetOutput("fuseDischargeState") : 0f;
            float fuseHouse = sim != null ? sim.GetOutput("fuseHouseState") : 0f;
            float breakerState = sim != null ? sim.GetOutput("breakerState") : 0f;
            
            float batteryVoltage = sim != null ? sim.GetOutput("batteryVoltage") : 0f;
            float batteryVoltageCarga = sim != null ? sim.GetOutput("batteryVoltageCarga") : 0f;
            
            float cellsCarga = sim != null ? sim.GetOutput("effectiveCellsCarga") : 0f;
            float cellsDescarga = sim != null ? sim.GetOutput("effectiveCellsDescarga") : 0f;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            string modeName = "";
            switch (scenarioName)
            {
                case "ScenarioA": modeName = "Scenario A (1899) - Tudor Battery Discharge"; break;
                case "ScenarioB": modeName = "Scenario B (1920) - Crossley Engine Generation"; break;
                case "ScenarioC": modeName = "Scenario C (1929) - ASEA AC Motor Generation"; break;
                default: modeName = scenarioName; break;
            }

            sb.AppendLine($"<b>Active Era / Mode</b>: {modeName}");
            sb.AppendLine($"<b>Session Status</b>: {(active ? "<color=#33F34D>ACTIVE</color>" : "<color=#B5B5B5>INACTIVE</color>")}");
            sb.AppendLine($"<b>Sim Time</b>: {simTime:F3} s");
            sb.AppendLine("--------------------------------");
            sb.AppendLine($"<b>Bus Distribution Voltage</b>: {vLine:F2} V");
            sb.AppendLine($"<b>Battery Voltage (Discharge Branch)</b>: {batteryVoltage:F2} V");
            sb.AppendLine($"<b>Battery Voltage (Charge Branch)</b>: {batteryVoltageCarga:F2} V");
            sb.AppendLine($"<b>Dynamo Generator Output</b>: {iDin:F2} A");

            string direction = iBat > 0.1f ? " (Charging)" : (iBat < -0.1f ? " (Discharging)" : " (Idle)");
            sb.AppendLine($"<b>Battery Current</b>: {iBat:F2} A{direction}");
            sb.AppendLine($"<b>Main House Load</b>: {iHouse:F2} A");
            sb.AppendLine($"<b>Dependencies Load</b>: {iDep:F2} A");
            sb.AppendLine($"<b>Battery Storage Charge (SOC)</b>: {soc:P3}");
            sb.AppendLine($"<b>Active Cells (Charge / Discharge)</b>: {cellsCarga:F0} / {cellsDescarga:F0}");
            sb.AppendLine("--------------------------------");
            
            string breakerColor = breakerState < 0.5f ? "#33F34D" : "#FF4F4F";
            string breakerText = breakerState < 0.5f ? "CLOSED" : "TRIPPED";
            sb.AppendLine($"<b>Automatic Circuit Breaker (B. Dijunctor)</b>: <color={breakerColor}>{breakerText}</color>");

            string fDynText = fuseDynamo < 0.5f ? "OK" : "BLOWN";
            string fChgText = fuseCharge < 0.5f ? "OK" : "BLOWN";
            string fDisText = fuseDischarge < 0.5f ? "OK" : "BLOWN";
            string fHseText = fuseHouse < 0.5f ? "OK" : "BLOWN";
            
            sb.AppendLine($"<b>Fuses (Dynamo / Charge / Discharge / House)</b>: {fDynText} / {fChgText} / {fDisText} / {fHseText}");

            telemetryText.text = sb.ToString();
        }
    }
}
