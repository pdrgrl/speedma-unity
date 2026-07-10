#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class CheckEngineAnimators : EditorWindow
{
    [MenuItem("Chamusca/Check Engine Animators")]
    public static void CheckAnimators()
    {
        GameObject parent = GameObject.Find("Scenario_B_CrossleyEngine");
        if (parent == null)
        {
            Debug.LogError("[Check] Could not find Scenario_B_CrossleyEngine in the scene!");
            return;
        }

        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);
        Debug.Log("=== SCENARIO B ENGINE ANIMATION CHECK ===");

        foreach (var child in allChildren)
        {
            if (child.name.Contains("Crossley") || child.name.Contains("Dynamo"))
            {
                Animator anim = child.GetComponent<Animator>();
                Animation legacyAnim = child.GetComponent<Animation>();
                
                string status = $"GameObject: '{child.name}'\n" +
                                $"  - Has Animator: {(anim != null ? "YES (" + anim.runtimeAnimatorController?.name + ")" : "no")}\n" +
                                $"  - Has Legacy Animation: {(legacyAnim != null ? "YES" : "no")}\n";
                Debug.Log(status);
            }
        }
    }
}
#endif
