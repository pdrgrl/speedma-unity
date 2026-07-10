#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;

public class FindAllSceneSwitches : EditorWindow
{
    [MenuItem("Chamusca/Find All Scene Switches")]
    public static void FindSwitches()
    {
        FmuToggleSwitch[] switches = Object.FindObjectsByType<FmuToggleSwitch>(FindObjectsSortMode.None);
        Debug.Log($"=== FOUND {switches.Length} FMU TOGGLE SWITCHES IN SCENE ===");
        
        foreach (var sw in switches)
        {
            Debug.Log($"Switch GameObject Name: '{sw.name}', Parent: '{(sw.transform.parent != null ? sw.transform.parent.name : "None")}', Position: {sw.transform.position}");
        }
    }
}
#endif
