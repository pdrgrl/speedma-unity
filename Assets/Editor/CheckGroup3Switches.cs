#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;

public class CheckGroup3Switches : EditorWindow
{
    [MenuItem("Chamusca/Check Group 3 Switches")]
    public static void CheckSwitches()
    {
        string[] targetNames = new string[] {
            "_SM_SingleSwitch_Pivot",
            "sw_casa_luz_Pivot",
            "sw_dep_luz_Pivot",
            "sw_quadro_luz_Pivot",
            "sw_bat_luz_Pivot"
        };

        GameObject parent = GameObject.Find("Scenario_A_BatteryOnly");
        if (parent == null)
        {
            Debug.LogError("[Check] Could not find Scenario_A_BatteryOnly in the scene!");
            return;
        }

        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
        
        Debug.Log("=== GROUP 3 SWITCH CHECK-UP ===");
        foreach (var name in targetNames)
        {
            int count = 0;
            foreach (var child in allChildren)
            {
                if (child.name == name)
                {
                    count++;
                    string status = $"Found '{child.name}' (Instance #{count}) at position {child.position}\n";
                    
                    // Check for interaction script
                    FmuToggleSwitch toggle = child.GetComponent<FmuToggleSwitch>();
                    if (toggle != null)
                    {
                        status += $"  - [OK] FmuToggleSwitch attached. Input: '{toggle.inputName}'\n";
                        status += $"  - Handle: {(toggle.switchHandle != null ? toggle.switchHandle.name : "<None>")}\n";
                        status += $"  - Rotation: Off={toggle.rotationOff}, On={toggle.rotationOn}\n";
                    }
                    else
                    {
                        status += "  - [MISSING] FmuToggleSwitch script is not attached!\n";
                    }

                    // Check for Collider
                    Collider col = child.GetComponentInChildren<Collider>(true);
                    if (col != null)
                    {
                        status += $"  - [OK] Collider found on: {col.gameObject.name} ({col.GetType().Name})\n";
                    }
                    else
                    {
                        status += "  - [MISSING] No Collider found recursively! Clicks will not register.\n";
                    }

                    Debug.Log(status);
                }
            }
            if (count == 0)
            {
                Debug.LogWarning($"[Check] Could not find any GameObjects named '{name}'");
            }
        }
    }
}
#endif
