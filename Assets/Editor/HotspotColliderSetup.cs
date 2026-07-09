using UnityEditor;
using UnityEngine;

public class HotspotColliderSetup
{
    [MenuItem("Tools/Setup Hotspot Mesh Colliders")]
    static void SetupColliders()
    {
        int collidersAddedCount = 0;
        int collidersUpdatedCount = 0;

        foreach (
            ChamuscaInteractable hotspot in Object.FindObjectsByType<ChamuscaInteractable>(
                FindObjectsSortMode.None
            )
        )
        {
            foreach (MeshFilter mf in hotspot.GetComponentsInChildren<MeshFilter>())
            {
                MeshCollider existing = mf.GetComponent<MeshCollider>();
                if (existing != null)
                {
                    if (!existing.convex)
                    {
                        Undo.RecordObject(existing, "Make MeshCollider Convex");
                        existing.convex = true;
                        collidersUpdatedCount++;
                    }
                }
                else
                {
                    MeshCollider mc = Undo.AddComponent<MeshCollider>(mf.gameObject);
                    mc.sharedMesh = mf.sharedMesh;
                    mc.convex = true;
                    collidersAddedCount++;
                }
            }
        }
        Debug.Log($"Hotspot mesh colliders set up. Added: {collidersAddedCount}, Updated: {collidersUpdatedCount}");
    }
}
