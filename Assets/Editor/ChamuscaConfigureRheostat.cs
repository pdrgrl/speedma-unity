#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;
using Speedma;

public class ChamuscaConfigureRheostat : EditorWindow
{
    [MenuItem("Chamusca/Configure Rheostat")]
    public static void ConfigureRheostat()
    {
        GameObject parent = GameObject.Find("Scenario_B_CrossleyEngine");
        if (parent == null)
        {
            Debug.LogError("[Configure] Could not find Scenario_B_CrossleyEngine in the scene!");
            return;
        }

        Transform target = null;
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "_SM_Rheostat_Pivot")
            {
                target = child;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError("[Configure] Could not find _SM_Rheostat_Pivot GameObject!");
            return;
        }

        ChamuscaSimController controller = Object.FindFirstObjectByType<ChamuscaSimController>();
        InspectionCamera camera = Object.FindFirstObjectByType<InspectionCamera>();

        RheostatWheel rw = target.GetComponent<RheostatWheel>();
        if (rw == null)
        {
            rw = target.gameObject.AddComponent<RheostatWheel>();
            Debug.Log("[Configure] Attached RheostatWheel component.");
        }

        // Connect references
        rw.controller = controller;
        rw.orbitCamera = camera;

        // Try to find the child wheel mesh (typically the first child or the pivot itself)
        Transform wheelMesh = target.Find("Wheel");
        if (wheelMesh == null)
        {
            // Look recursively or default to the pivot
            foreach (Transform c in target.GetComponentsInChildren<Transform>(true))
            {
                if (c.name.ToLower().Contains("wheel") || c.name.ToLower().Contains("knob"))
                {
                    wheelMesh = c;
                    break;
                }
            }
            if (wheelMesh == null) wheelMesh = target;
        }

        rw.wheel = wheelMesh;
        Debug.Log($"[Configure] Assigned wheel mesh target: '{wheelMesh.name}'");

        // Add BoxCollider to wheel if missing
        Collider col = wheelMesh.GetComponent<Collider>();
        if (col == null)
        {
            wheelMesh.gameObject.AddComponent<BoxCollider>();
            Debug.Log($"[Configure] Added BoxCollider to '{wheelMesh.name}' for click/drag interaction.");
        }

        // Apply calibrated angles
        rw.angleAtMaxResistance = 256f;
        rw.angleAtMinResistance = -76f;

        EditorUtility.SetDirty(target.gameObject);
        if (wheelMesh != target) EditorUtility.SetDirty(wheelMesh.gameObject);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("[Configure] Rheostat configuration completed successfully!");
    }
}
#endif
