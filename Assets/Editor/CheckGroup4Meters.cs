#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;

public class CheckGroup4Meters : EditorWindow
{
    [MenuItem("Chamusca/Check Group 4 Meters")]
    public static void CheckMeters()
    {
        string[] targetNames = new string[] {
            "_SM_Meter_Siemens_Halske_VOLT_Pivot",
            "_SM_Meter_Siemens_Halske_AMP10_Pivot",
            "_SM_Meter_Siemens_Halske_AMP30_Pivot"
        };

        GameObject parent = GameObject.Find("Scenario_A_BatteryOnly");
        if (parent == null)
        {
            Debug.LogError("[Check] Could not find Scenario_A_BatteryOnly in the scene!");
            return;
        }

        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
        
        Debug.Log("=== GROUP 4 METERS CHECK-UP ===");
        foreach (var name in targetNames)
        {
            Transform meter = null;
            foreach (var child in allChildren)
            {
                if (child.name == name)
                {
                    meter = child;
                    break;
                }
            }

            if (meter == null)
            {
                Debug.LogWarning($"[Check] Could not find GameObject: '{name}'");
                continue;
            }

            string status = $"Found '{meter.name}' at position {meter.position}\n";

            // Check for VoltmeterPointer (or general meter pointer script)
            VoltmeterPointer pointer = meter.GetComponent<VoltmeterPointer>();
            if (pointer != null)
            {
                status += $"  - [OK] VoltmeterPointer attached.\n";
                status += $"  - Needle transform: {(pointer.needle != null ? pointer.needle.name : "<None>")}\n";
                status += $"  - Range: {pointer.minValue} to {pointer.maxValue}\n";
            }
            else
            {
                status += "  - [MISSING] VoltmeterPointer script is not attached!\n";
            }

            // Find potential needle child meshes
            string childrenList = "";
            foreach (Transform c in meter.GetComponentsInChildren<Transform>(true))
            {
                if (c.name.ToLower().Contains("needle") || c.name.ToLower().Contains("pointer") || c.name.ToLower().Contains("ponteiro"))
                {
                    childrenList += $"{c.name}, ";
                }
            }
            if (!string.IsNullOrEmpty(childrenList))
            {
                status += $"  - Potential needle meshes found in hierarchy: {childrenList.TrimEnd(' ', ',')}\n";
            }
            else
            {
                status += "  - [WARNING] No child meshes named 'needle' or 'pointer' found in hierarchy!\n";
            }

            Debug.Log(status);
        }
    }
}
#endif
