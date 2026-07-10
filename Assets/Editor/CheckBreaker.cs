#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Chamusca.Simulation;

public class CheckBreaker : EditorWindow
{
    [MenuItem("Chamusca/Check Breaker")]
    public static void VerifyBreaker()
    {
        FmuBreakerSwitch breaker = Object.FindFirstObjectByType<FmuBreakerSwitch>();
        if (breaker == null)
        {
            Debug.LogError("[Breaker] No FmuBreakerSwitch component found in the active scene!");
            return;
        }

        Debug.Log("=== CIRCUIT BREAKER CHECK-UP ===");
        Debug.Log($"Found Breaker on GameObject: '{breaker.name}' at position {breaker.transform.position}");
        
        if (breaker.breakerHandle == null)
        {
            Debug.LogError("[FAIL] breakerHandle is NOT assigned in the FmuBreakerSwitch component!");
            return;
        }

        Debug.Log($"  - [OK] breakerHandle is assigned to: '{breaker.breakerHandle.name}'");

        Collider[] colliders = breaker.GetComponentsInChildren<Collider>(true);
        string colliderList = "";
        bool clickIsPossible = false;

        foreach (var col in colliders)
        {
            if (col.transform == breaker.breakerHandle || col.transform.IsChildOf(breaker.breakerHandle))
            {
                clickIsPossible = true;
                colliderList += $"{col.name} (Valid), ";
            }
            else
            {
                colliderList += $"{col.name} (Sibling/Parent), ";
            }
        }

        if (clickIsPossible)
        {
            Debug.Log($"  - [OK] Breaker CAN be clicked to rearm. Colliders: {colliderList.TrimEnd(' ', ',')}");
        }
        else
        {
            Debug.LogError($"  - [FAIL] Breaker CANNOT be clicked! No colliders found in the handle hierarchy. Colliders: {colliderList.TrimEnd(' ', ',')}");
        }
    }
}
#endif
