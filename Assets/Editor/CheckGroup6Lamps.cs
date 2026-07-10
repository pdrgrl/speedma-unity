#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;

public class CheckGroup6Lamps : EditorWindow
{
    [MenuItem("Chamusca/Check Group 6 Lamps")]
    public static void CheckLamps()
    {
        string[] targetNames = new string[] {
            "_SM_Lamp_Pivot",
            "_SM_Lamp (1)_Pivot",
            "_SM_Lamp (2)_Pivot"
        };

        GameObject parent = GameObject.Find("Scenario_A_BatteryOnly");
        if (parent == null)
        {
            Debug.LogError("[Check] Could not find Scenario_A_BatteryOnly in the scene!");
            return;
        }

        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
        
        Debug.Log("=== GROUP 6 LAMPS CHECK-UP ===");
        
        // Find existing HouseLightControllers in the scene
        HouseLightController[] controllers = Object.FindObjectsByType<HouseLightController>(FindObjectsSortMode.None);
        Debug.Log($"Found {controllers.Length} HouseLightController component(s) active in the scene.");
        for (int i = 0; i < controllers.Length; i++)
        {
            Debug.Log($"  - Controller #{i+1} on GameObject: '{controllers[i].name}', target lights count: {(controllers[i].targetLights != null ? controllers[i].targetLights.Length : 0)}");
        }

        foreach (var name in targetNames)
        {
            Transform lamp = null;
            foreach (var child in allChildren)
            {
                if (child.name == name)
                {
                    lamp = child;
                    break;
                }
            }

            if (lamp == null)
            {
                Debug.LogWarning($"[Check] Could not find GameObject: '{name}'");
                continue;
            }

            string status = $"Found '{lamp.name}' at position {lamp.position}\n";
            
            // Check for Light component
            Light light = lamp.GetComponentInChildren<Light>(true);
            if (light != null)
            {
                status += $"  - [OK] Light component found: '{light.name}' (Color: {light.color}, Intensity: {light.intensity})\n";
            }
            else
            {
                status += "  - [WARNING] No Light source found in the children of this lamp!\n";
            }

            Debug.Log(status);
        }
    }
}
#endif
