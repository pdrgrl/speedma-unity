#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;

public class CheckSimControllerState : EditorWindow
{
    [MenuItem("Chamusca/Check Sim Controller State")]
    public static void CheckState()
    {
        ChamuscaSimController controller = Object.FindFirstObjectByType<ChamuscaSimController>();
        if (controller == null)
        {
            Debug.LogError("[Check] Could not find ChamuscaSimController in the scene!");
            return;
        }

        string status = "=== SIMULATION CONTROLLER STATE ===\n" +
                        $"Active Scenario: {controller.scenarioManager?.currentScenario}\n" +
                        $"Is FMU Active: {(controller.simManager != null && controller.simManager.IsSessionActive ? "YES" : "NO")}\n" +
                        $"Engine RPM (Input): {controller.engine_rpm}\n" +
                        $"Rheostat Pos (Input): {controller.rheostat_pos}\n" +
                        "\n" +
                        "=== INSPECTOR REFERENCES ===\n" +
                        $"Line Voltmeter: {(controller.lineVoltmeter != null ? controller.lineVoltmeter.name : "NULL!")}\n" +
                        $"Dynamo Ammeter: {(controller.dynamoAmmeter != null ? controller.dynamoAmmeter.name : "NULL!")}\n" +
                        $"Battery Ammeter: {(controller.batteryAmmeter != null ? controller.batteryAmmeter.name : "NULL!")}\n" +
                        $"Voltmeter Selector: {(controller.voltmeterSelector != null ? controller.voltmeterSelector.name : "NULL!")}\n" +
                        $"Dynamo Switch (Dínamo LUZ): {(controller.dynamoLuzSwitch != null ? controller.dynamoLuzSwitch.name : "NULL!")}\n" +
                        $"Charge Switch (Carga bat): {(controller.chargeBatSwitch != null ? controller.chargeBatSwitch.name : "NULL!")}\n" +
                        $"Battery Switch (Bateria LUZ): {(controller.batLuzLeverSwitch != null ? controller.batLuzLeverSwitch.name : "NULL!")}\n";
        
        if (controller.simManager != null && controller.simManager.IsSessionActive)
        {
            status += "\n=== ACTIVE FMU OUTPUT VALUES ===\n" +
                      $"v_line: {controller.simManager.GetOutput("v_line")} V\n" +
                      $"i_bat: {controller.simManager.GetOutput("i_bat")} A\n" +
                      $"i_din: {controller.simManager.GetOutput("i_din")} A\n" +
                      $"batteryVoltage: {controller.simManager.GetOutput("batteryVoltage")} V\n" +
                      $"batteryVoltageCarga: {controller.simManager.GetOutput("batteryVoltageCarga")} V\n" +
                      $"dynamo_voltage: {controller.simManager.GetOutput("dynamo_voltage")} V\n";
        }

        Debug.Log(status);
    }
}
#endif
