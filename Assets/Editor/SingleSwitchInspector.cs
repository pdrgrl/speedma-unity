#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SingleSwitchInspector : EditorWindow
{
    [MenuItem("Chamusca/Inspect Single Switches")]
    public static void InspectSwitches()
    {
        GameObject parent = GameObject.Find("Scenario_A_BatteryOnly");
        if (parent == null)
        {
            Debug.LogError("[Inspector] Could not find Scenario_A_BatteryOnly in the scene!");
            return;
        }

        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
        int idx = 1;
        foreach (var child in allChildren)
        {
            if (child.name == "_SM_SingleSwitch")
            {
                Debug.Log($"[Inspector] Found Switch #{idx} -> Path: {GetFullPath(child)}, Position: {child.position}");
                idx++;
            }
        }
    }

    private static string GetFullPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}
#endif
