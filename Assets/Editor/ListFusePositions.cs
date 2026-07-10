#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ListFusePositions : EditorWindow
{
    [MenuItem("Chamusca/List Fuse Positions")]
    public static void PrintPositions()
    {
        GameObject parent = GameObject.Find("Scenario_A_BatteryOnly");
        if (parent == null)
        {
            Debug.LogError("[Configure] Could not find Scenario_A_BatteryOnly in the scene!");
            return;
        }

        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
        Debug.Log("=== FUSE GEOMETRY LOCATIONS ===");

        foreach (var child in allChildren)
        {
            if (child.name == "_SM_Fuzivel_Pivot" || child.name == "fuseHouseState_Pivot")
            {
                Debug.Log($"GameObject: '{child.name}', Local Pos: {child.localPosition}, World Pos: {child.position}");
            }
        }
    }
}
#endif
