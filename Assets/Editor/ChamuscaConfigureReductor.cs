#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;
using Speedma;

public class ChamuscaConfigureReductor : EditorWindow
{
    [MenuItem("Chamusca/Configure Reductor")]
    public static void AttachReductorController()
    {
        GameObject parent = GameObject.Find("Scenario_A_BatteryOnly");
        if (parent == null)
        {
            Debug.LogError("[Configure] Could not find Scenario_A_BatteryOnly in the scene!");
            return;
        }

        Transform target = null;
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "RedutorDuplo_Pivot")
            {
                target = child;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError("[Configure] Could not find RedutorDuplo_Pivot GameObject!");
            return;
        }

        SpeedmaSimManager sim = Object.FindFirstObjectByType<SpeedmaSimManager>();
        InspectionCamera camera = Object.FindFirstObjectByType<InspectionCamera>();

        // Attach controller
        ReductorDuploController controller = target.GetComponent<ReductorDuploController>();
        if (controller == null)
        {
            controller = target.gameObject.AddComponent<ReductorDuploController>();
        }

        controller.simManager = sim;
        controller.orbitCamera = camera;

        // Auto-wire colliders helper
        AddColliderIfMissing(controller.blueHandle, "Blue Handle");
        AddColliderIfMissing(controller.brownHandle, "Brown Handle");
        AddColliderIfMissing(controller.bluePin, "Blue Pin");
        AddColliderIfMissing(controller.brownPin, "Brown Pin");

        EditorUtility.SetDirty(target.gameObject);
        if (controller.blueHandle != null) EditorUtility.SetDirty(controller.blueHandle.gameObject);
        if (controller.brownHandle != null) EditorUtility.SetDirty(controller.brownHandle.gameObject);
        if (controller.bluePin != null) EditorUtility.SetDirty(controller.bluePin.gameObject);
        if (controller.brownPin != null) EditorUtility.SetDirty(controller.brownPin.gameObject);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        
        Debug.Log("[Configure] Reductor layout colliders configured successfully!");
    }

    private static void AddColliderIfMissing(Transform t, string label)
    {
        if (t == null) return;
        
        Collider col = t.GetComponentInChildren<Collider>(true);
        if (col == null)
        {
            // Attach a BoxCollider to the first renderer object, or the pivot itself
            MeshRenderer renderer = t.GetComponentInChildren<MeshRenderer>(true);
            GameObject targetGo = renderer != null ? renderer.gameObject : t.gameObject;
            
            targetGo.AddComponent<BoxCollider>();
            Debug.Log($"[Configure] Added BoxCollider to '{label}' on GameObject: {targetGo.name}");
        }
    }
}
#endif
