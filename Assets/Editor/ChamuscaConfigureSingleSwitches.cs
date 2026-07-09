#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;
using Speedma;

public class ChamuscaConfigureSingleSwitches : EditorWindow
{
    [MenuItem("Chamusca/Configure Single Switches")]
    public static void ConfigureSwitches()
    {
        GameObject parent = GameObject.Find("Scenario_A_BatteryOnly");
        if (parent == null)
        {
            Debug.LogError("[Configure] Could not find Scenario_A_BatteryOnly in the scene!");
            return;
        }

        SpeedmaSimManager sim = Object.FindFirstObjectByType<SpeedmaSimManager>();
        if (sim == null)
        {
            Debug.LogError("[Configure] Could not find SpeedmaSimManager in the scene!");
            return;
        }

        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
        
        // Let's collect them and sort them by their X position from left to right
        var switchList = new System.Collections.Generic.List<Transform>();
        foreach (var child in allChildren)
        {
            if (child.name == "_SM_SingleSwitch")
            {
                switchList.Add(child);
            }
        }

        switchList.Sort((a, b) => a.position.x.CompareTo(b.position.x));

        if (switchList.Count < 4)
        {
            Debug.LogError($"[Configure] Found only {switchList.Count} switches. Expected 4!");
            return;
        }

        // Mapping array based on X-axis sorting (Left to Right)
        string[] inputs = new string[] {
            "sw_dinamo_luz",        // Leftmost: Switch #1 (X = -2.36)
            "sw_carga_bat",         // Second: Switch #4 (X = -2.16)
            "sw_bat_luz_lever",     // Third: Switch #3 (X = -1.93)
            "sw_reserva"            // Rightmost: Switch #2 (X = -1.74)
        };

        for (int i = 0; i < switchList.Count; i++)
        {
            Transform sw = switchList[i];
            
            // 1. Find Sphere.010 child
            Transform handle = sw.Find("Sphere.010");
            if (handle == null)
            {
                // Fallback: look recursively in case of path differences
                foreach (Transform c in sw.GetComponentsInChildren<Transform>(true))
                {
                    if (c.name == "Sphere.010")
                    {
                        handle = c;
                        break;
                    }
                }
            }

            if (handle == null)
            {
                Debug.LogWarning($"[Configure] Could not find Sphere.010 child on switch at X: {sw.position.x}");
                continue;
            }

            // 2. Add BoxCollider to the handle if it doesn't exist
            Collider col = handle.GetComponent<Collider>();
            if (col == null)
            {
                col = handle.gameObject.AddComponent<BoxCollider>();
                Debug.Log($"[Configure] Added BoxCollider to {handle.name} on switch at X: {sw.position.x}");
            }

            // 3. Attach FmuToggleSwitch
            FmuToggleSwitch toggle = sw.GetComponent<FmuToggleSwitch>();
            if (toggle == null)
            {
                toggle = sw.gameObject.AddComponent<FmuToggleSwitch>();
            }

            // 4. Configure parameters
            toggle.simManager = sim;
            toggle.inputName = inputs[i];
            toggle.switchHandle = handle;
            
            // Set standard local rotation ranges for the lever rotation (tweakable in inspector)
            toggle.rotationOff = new Vector3(0f, 0f, 0f);
            toggle.rotationOn = new Vector3(-45f, 0f, 0f); // typical vertical lever flip rotation
            toggle.maxInteractDistance = 3.0f;

            EditorUtility.SetDirty(sw.gameObject);
            EditorUtility.SetDirty(handle.gameObject);
            Debug.Log($"[Configure] Successfully mapped Switch at X: {sw.position.x:F2} to input '{inputs[i]}'");
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("[Configure] Single switch auto-configuration completed!");
    }
}
#endif
