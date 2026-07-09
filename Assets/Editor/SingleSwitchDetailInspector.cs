#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SingleSwitchDetailInspector : EditorWindow
{
    [MenuItem("Chamusca/Inspect Switch Details")]
    public static void InspectDetails()
    {
        GameObject parent = GameObject.Find("Scenario_A_BatteryOnly");
        if (parent == null) return;

        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
        foreach (var child in allChildren)
        {
            if (child.name == "_SM_SingleSwitch" && Mathf.Approximately(child.position.x, -2.36f))
            {
                Debug.Log($"[Inspector] Found Switch #1. Listing all children:");
                PrintChildren(child, "  ");
                break;
            }
        }
    }

    private static void PrintChildren(Transform t, string indent)
    {
        foreach (Transform child in t)
        {
            Debug.Log($"{indent}- {child.name} (Position: {child.localPosition}, Rotation: {child.localEulerAngles})");
            PrintChildren(child, indent + "  ");
        }
    }
}
#endif
