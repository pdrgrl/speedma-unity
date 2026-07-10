#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ChamuscaConfigureSelectorFuse : EditorWindow
{
    [MenuItem("Chamusca/Configure Voltmeter Selector")]
    public static void ConfigureSelector()
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
            if (child.name == "_SM_FuzivelMadeira_Pivot")
            {
                target = child;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError("[Configure] Could not find _SM_FuzivelMadeira_Pivot GameObject!");
            return;
        }

        VoltmeterSelectorFuse selector = target.GetComponent<VoltmeterSelectorFuse>();
        if (selector == null)
        {
            selector = target.gameObject.AddComponent<VoltmeterSelectorFuse>();
            Debug.Log("[Configure] Attached VoltmeterSelectorFuse script.");
        }

        // Find child selector handle mesh
        Transform handle = target.Find("Selector");
        if (handle == null)
        {
            foreach (Transform c in target.GetComponentsInChildren<Transform>(true))
            {
                if (c.name.ToLower().Contains("selector"))
                {
                    handle = c;
                    break;
                }
            }
        }

        if (handle != null)
        {
            selector.selectorHandle = handle;
            Debug.Log($"[Configure] Found and assigned selector handle child: '{handle.name}'");
        }
        else
        {
            Debug.LogWarning("[Configure] Could not find a child named 'Selector'. Defaulting to root.");
            selector.selectorHandle = target;
        }

        // Set default local translation offset (adjust in inspector if needed)
        selector.slotOffset = new Vector3(0f, -0.06f, 0f);

        // Add BoxCollider to the parent pivot if it doesn't exist so the entire block remains clickable
        Collider col = target.GetComponent<Collider>();
        if (col == null)
        {
            target.gameObject.AddComponent<BoxCollider>();
            Debug.Log("[Configure] Added BoxCollider to _SM_FuzivelMadeira_Pivot.");
        }

        EditorUtility.SetDirty(target.gameObject);
        if (selector.selectorHandle != null) EditorUtility.SetDirty(selector.selectorHandle.gameObject);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        
        Debug.Log("[Configure] Voltmeter Selector Bar configured successfully! Click to cycle in Play Mode.");
    }
}
#endif
