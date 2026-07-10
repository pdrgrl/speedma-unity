#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;

public class ChamuscaConfigureScenarioCActor : EditorWindow
{
    [MenuItem("Chamusca/Configure Scenario C AC Motor")]
    public static void ConfigureACMotor()
    {
        GameObject parent = GameObject.Find("Scenario_C_ACIntegration");
        if (parent == null)
        {
            Debug.LogError("[Configure] Could not find Scenario_C_ACIntegration in the active scene!");
            return;
        }

        ChamuscaSimController controller = Object.FindFirstObjectByType<ChamuscaSimController>();

        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
        Transform targetMotor = null;

        foreach (var child in allChildren)
        {
            if (child.name == "_SM_Triphase_Pivot")
            {
                targetMotor = child;
                break;
            }
        }

        if (targetMotor == null)
        {
            Debug.LogError("[Configure] Could not find '_SM_Triphase_Pivot' GameObject!");
            return;
        }

        // Attach animator controller script
        EngineRpmAnimator animatorSpeedCtrl = targetMotor.GetComponent<EngineRpmAnimator>();
        if (animatorSpeedCtrl == null)
        {
            animatorSpeedCtrl = targetMotor.gameObject.AddComponent<EngineRpmAnimator>();
            Debug.Log("[Configure] Attached EngineRpmAnimator to _SM_Triphase_Pivot.");
        }

        animatorSpeedCtrl.controller = controller;
        animatorSpeedCtrl.nominalRPM = 375f; // Sync with locked grid speed
        animatorSpeedCtrl.baseAnimationSpeed = 1.0f;

        EditorUtility.SetDirty(targetMotor.gameObject);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(targetMotor.gameObject.scene);

        Debug.Log("[Configure] Scenario C AC Motor successfully configured!");
    }
}
#endif
