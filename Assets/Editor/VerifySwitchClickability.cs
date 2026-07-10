#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;

public class VerifySwitchClickability : EditorWindow
{
    [MenuItem("Chamusca/Verify Switch Clickability")]
    public static void VerifyClickability()
    {
        FmuToggleSwitch[] allSwitches = Object.FindObjectsByType<FmuToggleSwitch>(FindObjectsSortMode.None);
        Debug.Log($"=== VERIFYING SWITCH CLICKABILITY FOR {allSwitches.Length} SWITCHES ===");
        
        foreach (var toggle in allSwitches)
        {
            if (toggle.switchHandle == null)
            {
                Debug.LogError($"[Verification] '{toggle.name}' has no switchHandle assigned!");
                continue;
            }

            Collider[] colliders = toggle.GetComponentsInChildren<Collider>(true);
            bool clickIsPossible = false;
            string colliderList = "";

            foreach (var col in colliders)
            {
                if (col.transform == toggle.switchHandle || col.transform.IsChildOf(toggle.switchHandle))
                {
                    clickIsPossible = true;
                    colliderList += $"{col.name} (Valid), ";
                }
                else
                {
                    colliderList += $"{col.name} (INVALID - Sibling or Parent), ";
                }
            }

            if (clickIsPossible)
            {
                Debug.Log($"[OK] '{toggle.name}' (Input: {toggle.inputName}) can be clicked. Target handle: '{toggle.switchHandle.name}'. Found valid colliders: {colliderList.TrimEnd(' ', ',')}");
            }
            else
            {
                Debug.LogError($"[FAIL] '{toggle.name}' (Input: {toggle.inputName}) CANNOT BE CLICKED! Clicked meshes will be ignored by FmuToggleSwitch. Found colliders: {colliderList.TrimEnd(' ', ',')}");
            }
        }
    }
}
#endif
