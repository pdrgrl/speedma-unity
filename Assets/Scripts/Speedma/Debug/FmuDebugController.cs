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

        private bool _showHud = false; // Hidden by default now

        private void Start()
        {
            if (toggleButton != null)
            {
                toggleButton.onClick.AddListener(ToggleHud);
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

            if (_showHud && telemetryText != null)
            {
                UpdateTelemetryText();
            }
        }

        public void SetCanvasUIReferences(
            GameObject panel, 
            TMPro.TextMeshProUGUI text, 
            UnityEngine.UI.Button button,
            UnityEngine.UI.Button btnA,
            UnityEngine.UI.Button btnB,
            UnityEngine.UI.Button btnC
        ) {
            hudPanel = panel;
            telemetryText = text;
            toggleButton = button;
            btnScenarioA = btnA;
            btnScenarioB = btnB;
            btnScenarioC = btnC;
            
            if (toggleButton != null)
            {
                toggleButton.onClick.RemoveAllListeners();
                toggleButton.onClick.AddListener(ToggleHud);
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
            sb.AppendLine($"<b>Scenario</b>: {scenarioName}");
            sb.AppendLine($"<b>Session</b>: {(active ? "<color=#33F34D>ACTIVE</color>" : "<color=#B5B5B5>INACTIVE</color>")}");
            sb.AppendLine($"<b>Sim Time</b>: {simTime:F3} s");
            sb.AppendLine("--------------------------------");
            sb.AppendLine($"<b>V Line</b>: {vLine:F2} V");
            sb.AppendLine($"<b>Bat Voltage (Total)</b>: {batteryVoltage:F2} V");
            sb.AppendLine($"<b>Bat Volt (Carga)</b>: {batteryVoltageCarga:F2} V");
            sb.AppendLine($"<b>i_din (Dynamo)</b>: {iDin:F2} A");
            sb.AppendLine($"<b>i_bat (Battery)</b>: {iBat:F2} A");
            sb.AppendLine($"<b>i_house (Load)</b>: {iHouse:F2} A");
            sb.AppendLine($"<b>i_dep (Load)</b>: {iDep:F2} A");
            sb.AppendLine($"<b>SOC</b>: {soc:P3}");
            sb.AppendLine($"<b>Cells (Chg/Dis)</b>: {cellsCarga:F0} / {cellsDescarga:F0}");
            sb.AppendLine("--------------------------------");
            
            string breakerColor = breakerState < 0.5f ? "#33F34D" : "#FF4F4F";
            string breakerText = breakerState < 0.5f ? "CLOSED" : "TRIPPED";
            sb.AppendLine($"<b>Breaker</b>: <color={breakerColor}>{breakerText}</color>");

            string fDynText = fuseDynamo < 0.5f ? "OK" : "BLOWN";
            string fChgText = fuseCharge < 0.5f ? "OK" : "BLOWN";
            string fDisText = fuseDischarge < 0.5f ? "OK" : "BLOWN";
            string fHseText = fuseHouse < 0.5f ? "OK" : "BLOWN";
            
            sb.AppendLine($"<b>Fuses (D/C/D/H)</b>: {fDynText} / {fChgText} / {fDisText} / {fHseText}");

            telemetryText.text = sb.ToString();
        }
    }
}
