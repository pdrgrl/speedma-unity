#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;

public class ChamuscaSetupSimController : EditorWindow
{
    [MenuItem("Chamusca/Setup Simulation Controller")]
    public static void WireSimulationController()
    {
        ChamuscaSimController controller = Object.FindFirstObjectByType<ChamuscaSimController>();
        if (controller == null)
        {
            Debug.LogError("[Configure] Could not find ChamuscaSimController in the active scene!");
            return;
        }

        // Find Scenario_A_BatteryOnly to inspect its child components
        GameObject parentA = GameObject.Find("Scenario_A_BatteryOnly");
        if (parentA == null)
        {
            Debug.LogWarning("[Configure] Scenario_A_BatteryOnly not found. Searching globally in scene.");
        }

        Transform[] allSceneTransforms = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None);

        foreach (var t in allSceneTransforms)
        {
            if (t.name == "sw_dinamo_luz_Pivot")
            {
                controller.dynamoLuzSwitch = t.GetComponent<FmuToggleSwitch>();
            }
            else if (t.name == "sw_carga_bat_Pivot")
            {
                controller.chargeBatSwitch = t.GetComponent<FmuToggleSwitch>();
            }
            else if (t.name == "sw_bat_luz_Pivot")
            {
                // Note: "sw_bat_luz_Pivot" is the light bulb switch, not the lever.
                // We keep it mapped to batteryRoomLightSwitch.
            }
            else if (t.name == "_SM_SingleSwitch_Pivot")
            {
                float x = t.localPosition.x;
                FmuToggleSwitch toggle = t.GetComponent<FmuToggleSwitch>();

                if (x < -2.00f) // Leftmost -> Dinamo LUZ
                {
                    controller.dynamoLuzSwitch = toggle;
                }
                else if (x < -1.75f && x >= -2.00f) // Mid-left -> Carga Bateria
                {
                    controller.chargeBatSwitch = toggle;
                }
                else if (x >= -1.55f) // Rightmost -> Bateria LUZ
                {
                    controller.batLuzLeverSwitch = toggle;
                }
            }
            else if (t.name == "RedutorDuplo_Pivot")
            {
                controller.reductorController = t.GetComponent<ReductorDuploController>();
            }
            else if (t.name == "_SM_FuzivelMadeira_Pivot")
            {
                controller.voltmeterSelector = t.GetComponent<VoltmeterSelectorFuse>();
            }
        }

        EditorUtility.SetDirty(controller.gameObject);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(controller.gameObject.scene);

        Debug.Log("[Configure] ChamuscaSimController references wired up successfully! Check the Inspector to confirm.");
    }
}
#endif
