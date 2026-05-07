// Assets/Editor/HotspotColliderSetup.cs
using UnityEditor;
using UnityEngine;

public class HotspotColliderSetup
{
    [MenuItem("Tools/Setup Hotspot Mesh Colliders")]
    static void SetupColliders()
    {
        foreach (
            InteractableHotspot hotspot in Object.FindObjectsByType<InteractableHotspot>(
                FindObjectsSortMode.None
            )
        )
        {
            foreach (MeshFilter mf in hotspot.GetComponentsInChildren<MeshFilter>())
            {
                MeshCollider existing = mf.GetComponent<MeshCollider>();
                if (existing != null)
                {
                    existing.convex = true;
                }
                else
                {
                    MeshCollider mc = Undo.AddComponent<MeshCollider>(mf.gameObject);
                    mc.sharedMesh = mf.sharedMesh;
                    mc.convex = true;
                }
            }
        }
        Debug.Log("Hotspot mesh colliders set up.");
    }
}
