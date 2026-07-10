#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;

public class ChamuscaConfigureEngineAnimators : EditorWindow
{
    [MenuItem("Chamusca/Configure Engine Animators")]
    public static void AttachAnimators()
    {
        GameObject parent = GameObject.Find("Scenario_B_CrossleyEngine");
        if (parent == null)
        {
            Debug.LogError("[Configure] Could not find Scenario_B_CrossleyEngine in the scene!");
            return;
        }

        ChamuscaSimController controller = Object.FindFirstObjectByType<ChamuscaSimController>();

        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
        int configuredCount = 0;

        foreach (var child in allChildren)
        {
            if (child.name == "_SM_Crossley_Motor" || child.name == "SM_Dynamo")
            {
                EngineRpmAnimator animatorSpeedCtrl = child.GetComponent<EngineRpmAnimator>();
                if (animatorSpeedCtrl == null)
                {
                    animatorSpeedCtrl = child.gameObject.AddComponent<EngineRpmAnimator>();
                    Debug.Log($"[Configure] Attached EngineRpmAnimator to '{child.name}'.");
                }

                animatorSpeedCtrl.controller = controller;
                animatorSpeedCtrl.nominalRPM = 350f;
                animatorSpeedCtrl.baseAnimationSpeed = 1.0f;

                EditorUtility.SetDirty(child.gameObject);
                configuredCount++;
            }
        }

        if (configuredCount > 0)
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            Debug.Log($"[Configure] Successfully configured {configuredCount} engine animator controllers!");
        }
        else
        {
            Debug.LogWarning("[Configure] Could not find '_SM_Crossley_Motor' or 'SM_Dynamo' to configure!");
        }
    }
}
#endif
