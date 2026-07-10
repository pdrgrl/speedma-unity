#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;

public class VerifyScenarioAFuses : EditorWindow
{
    [MenuItem("Chamusca/Verify Scenario A Fuses")]
    public static void VerifyFuses()
    {
        GameObject parent = GameObject.Find("Scenario_A_BatteryOnly");
        if (parent == null)
        {
            Debug.LogError("[Verify] Could not find Scenario_A_BatteryOnly in the scene!");
            return;
        }

        Debug.Log("=== SCENARIO A FUSES VERIFICATION ===");
        
        int foundCount = 0;
        FmuReplaceableFuse[] fuses = parent.GetComponentsInChildren<FmuReplaceableFuse>(true);
        
        foreach (var fuse in fuses)
        {
            foundCount++;
            string status = $"Fuse #{foundCount} on GameObject: '{fuse.name}'\n" +
                            $"  - FMU Output Variable: '{fuse.fuseStateOutput}'\n" +
                            $"  - Has Body Reference: {(fuse.fuseBody != null ? fuse.fuseBody.name : "MISSING")}\n" +
                            $"  - Has Collider: {(fuse.GetComponent<Collider>() != null || (fuse.fuseBody != null && fuse.fuseBody.GetComponent<Collider>() != null) ? "Yes" : "NO COLLIDER!")}\n" +
                            $"  - Intact Material: {(fuse.intactMaterial != null ? fuse.intactMaterial.name : "None")}\n" +
                            $"  - Blown Material: {(fuse.blownMaterial != null ? fuse.blownMaterial.name : "None")}\n" +
                            $"  - Installed Pos: {fuse.installedLocalPosition}, Blown Pos: {fuse.blownLocalPosition}\n";
            Debug.Log(status);
        }

        if (foundCount == 0)
        {
            Debug.LogWarning("[Verify] No FmuReplaceableFuse scripts found attached under Scenario_A_BatteryOnly!");
        }
    }
}
#endif
