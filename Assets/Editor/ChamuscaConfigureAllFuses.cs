#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;
using Speedma;

public class ChamuscaConfigureAllFuses : EditorWindow
{
    [MenuItem("Chamusca/Configure All Fuses")]
    public static void ConfigureFuses()
    {
        GameObject parent = GameObject.Find("Scenario_A_BatteryOnly");
        if (parent == null)
        {
            Debug.LogError("[Configure] Could not find Scenario_A_BatteryOnly in the scene!");
            return;
        }

        SpeedmaSimManager sim = Object.FindFirstObjectByType<SpeedmaSimManager>();
        ChamuscaSimController controller = Object.FindFirstObjectByType<ChamuscaSimController>();

        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);

        // Fetch shared materials from the existing fuseHouseState_Pivot to maintain visual consistency
        Material intactMat = null;
        Material blownMat = null;
        FmuReplaceableFuse template = parent.GetComponentInChildren<FmuReplaceableFuse>(true);
        if (template != null)
        {
            intactMat = template.intactMaterial;
            blownMat = template.blownMaterial;
        }

        foreach (var child in allChildren)
        {
            string varName = "";

            if (child.name == "fuseHouseState_Pivot")
            {
                varName = "fuseHouseState";
            }
            else if (child.name == "_SM_Fuzivel_Pivot")
            {
                float x = child.localPosition.x;
                float y = child.localPosition.y;

                if (y > 2.2f) // Top Row
                {
                    if (x > -1.30f) varName = "fuseDynamoState";
                    else if (x > -1.50f) varName = "fuseChargeState";
                    else if (x > -1.70f) varName = "fuseMachineHouseState";
                    else if (x > -1.90f) varName = "fuseBatteryState";
                    else if (x > -2.10f) varName = "fuseBatteryRoomState";
                    else if (x > -2.25f) varName = "fuseDepState";
                }
                else // Bottom Row
                {
                    if (x < -2.00f) varName = "fuseDischargeState";
                }
            }

            if (string.IsNullOrEmpty(varName)) continue;

            FmuReplaceableFuse fuse = child.GetComponent<FmuReplaceableFuse>();
            if (fuse == null)
            {
                fuse = child.gameObject.AddComponent<FmuReplaceableFuse>();
            }

            // Assign variables and references
            fuse.simManager = sim;
            fuse.controller = controller;
            fuse.fuseStateOutput = varName;

            // Find or assign body using recursive search
            Transform body = FindChildRecursive(child, "Fuzivel");
            if (body == null) body = child;
            
            fuse.fuseBody = body;

            // Auto collider on the moveable body
            // Collider col = body.GetComponent<Collider>();
            // if (col == null)
            // {
            //     body.gameObject.AddComponent<BoxCollider>();
            // }

            // Copy materials
            fuse.intactMaterial = intactMat;
            fuse.blownMaterial = blownMat;

            // Visual animation positions
            fuse.installedLocalPosition = body.localPosition;
            fuse.blownLocalPosition = body.localPosition + new Vector3(0f, -0.01f, 0.02f);
            fuse.installedLocalRotation = body.localEulerAngles;
            fuse.blownLocalRotation = body.localEulerAngles + new Vector3(-20f, 0f, 0f);
            fuse.animationSpeed = 10f;

            EditorUtility.SetDirty(child.gameObject);
            EditorUtility.SetDirty(body.gameObject);
            Debug.Log($"[Configure] Configured fuse '{child.name}' at X={child.localPosition.x:F2} mapping to output '{varName}'");
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("[Configure] All 8 fuses successfully configured and mapped to their FMU states!");
    }

    private static Transform FindChildRecursive(Transform parent, string name)
    {
        if (parent.name == name) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindChildRecursive(child, name);
            if (result != null) return result;
        }
        return null;
    }
}
#endif
