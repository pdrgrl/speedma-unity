using UnityEditor;
using UnityEngine;

public class PivotEditorWindow : EditorWindow
{
    private enum PivotMethod
    {
        ParentWrapper,
        ModifyMeshVertices
    }

    private PivotMethod selectedMethod = PivotMethod.ParentWrapper;

    // Quick presets for bounds positioning
    private enum PivotPreset
    {
        Center,
        BottomCenter,
        TopCenter,
        Left,
        Right,
        Front,
        Back
    }

    private PivotPreset selectedPreset = PivotPreset.BottomCenter;
    private Vector3 customOffset = Vector3.zero;

    [MenuItem("Tools/Pivot Editor")]
    public static void ShowWindow()
    {
        GetWindow<PivotEditorWindow>("Pivot Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Adjust Object Pivot", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        GameObject activeGo = Selection.activeGameObject;
        if (activeGo == null)
        {
            EditorGUILayout.HelpBox("Select a GameObject in the hierarchy to edit its pivot.", MessageType.Info);
            return;
        }

        selectedMethod = (PivotMethod)EditorGUILayout.EnumPopup("Adjustment Method", selectedMethod);

        EditorGUILayout.Space();
        selectedPreset = (PivotPreset)EditorGUILayout.EnumPopup("Pivot Preset Position", selectedPreset);

        if (selectedPreset == PivotPreset.Center)
        {
            customOffset = Vector3.zero;
        }

        EditorGUILayout.Space();

        if (selectedMethod == PivotMethod.ParentWrapper)
        {
            EditorGUILayout.HelpBox(
                "Parent Wrapper Method (Recommended):\n" +
                "Creates an empty parent GameObject at the target pivot location and makes the selected object its child. " +
                "This is safe, non-destructive, and works on any complex hierarchy or prefab instance.",
                MessageType.Info
            );

            if (GUILayout.Button("Create Pivot Wrapper"))
            {
                CreateParentWrapper(activeGo, selectedPreset);
            }
        }
        else if (selectedMethod == PivotMethod.ModifyMeshVertices)
        {
            EditorGUILayout.HelpBox(
                "Modify Mesh Vertices Method:\n" +
                "Directly offsets the vertices of the Mesh asset and shifts the GameObject's transform. " +
                "WARNING: This modifies the mesh geometry. For shared mesh assets (like imported FBX files), it's recommended to duplicate the mesh first.",
                MessageType.Warning
            );

            MeshFilter filter = activeGo.GetComponent<MeshFilter>();
            if (filter == null || filter.sharedMesh == null)
            {
                EditorGUILayout.HelpBox("Selected GameObject does not have a MeshFilter with a valid mesh.", MessageType.Error);
                return;
            }

            if (GUILayout.Button("Adjust Mesh Vertices Pivot"))
            {
                AdjustMeshPivot(activeGo, filter, selectedPreset);
            }
        }
    }

    private void CreateParentWrapper(GameObject go, PivotPreset preset)
    {
        Undo.IncrementCurrentGroup();
        string groupName = "Create Pivot Wrapper";
        Undo.SetCurrentGroupName(groupName);

        // Get bounds
        Bounds bounds = CalculateBounds(go);
        Vector3 worldPivot = GetPivotPositionFromBounds(bounds, preset);

        // Create wrapper parent
        GameObject wrapper = new GameObject(go.name + "_Pivot");
        Undo.RegisterCreatedObjectUndo(wrapper, "Create Parent Wrapper");
        
        wrapper.transform.position = worldPivot;
        wrapper.transform.rotation = go.transform.rotation;
        
        if (go.transform.parent != null)
        {
            Undo.SetTransformParent(wrapper.transform, go.transform.parent, "Reparent Wrapper");
        }

        Undo.SetTransformParent(go.transform, wrapper.transform, "Reparent Target Object");
        
        Selection.activeGameObject = wrapper;
        Debug.Log($"Created pivot wrapper for '{go.name}' at {preset}.", wrapper);
    }

    private void AdjustMeshPivot(GameObject go, MeshFilter filter, PivotPreset preset)
    {
        Mesh originalMesh = filter.sharedMesh;
        if (originalMesh == null) return;

        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Adjust Mesh Pivot");

        // Calculate offset in local space
        Bounds localBounds = originalMesh.bounds;
        Vector3 targetLocalPivot = GetPivotPositionFromLocalBounds(localBounds, preset);
        Vector3 offset = -targetLocalPivot;

        if (offset == Vector3.zero)
        {
            Debug.Log("Offset is zero, no mesh adjustment needed.");
            return;
        }

        // Duplicate the mesh so we don't overwrite the original asset on disk unexpectedly
        Mesh newMesh = Instantiate(originalMesh);
        newMesh.name = originalMesh.name + "_adjustedPivot";

        Vector3[] vertices = newMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] += offset;
        }
        newMesh.vertices = vertices;
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        newMesh.RecalculateTangents();

        // If there's a collider, we should update it
        MeshCollider meshCollider = go.GetComponent<MeshCollider>();

        Undo.RecordObject(filter, "Modify Mesh Filter");
        filter.sharedMesh = newMesh;

        if (meshCollider != null)
        {
            Undo.RecordObject(meshCollider, "Modify Mesh Collider");
            meshCollider.sharedMesh = newMesh;
        }

        // Offset the transform so the object remains visually in the same place
        Undo.RecordObject(go.transform, "Adjust Transform Position");
        Vector3 worldOffset = go.transform.TransformDirection(targetLocalPivot);
        go.transform.position += worldOffset;

        Debug.Log($"Adjusted mesh pivot for '{go.name}' using preset {preset}.", go);
    }

    private Bounds CalculateBounds(GameObject go)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            return new Bounds(go.transform.position, Vector3.zero);
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        return bounds;
    }

    private Vector3 GetPivotPositionFromBounds(Bounds bounds, PivotPreset preset)
    {
        switch (preset)
        {
            case PivotPreset.Center:
                return bounds.center;
            case PivotPreset.BottomCenter:
                return new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
            case PivotPreset.TopCenter:
                return new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
            case PivotPreset.Left:
                return new Vector3(bounds.min.x, bounds.center.y, bounds.center.z);
            case PivotPreset.Right:
                return new Vector3(bounds.max.x, bounds.center.y, bounds.center.z);
            case PivotPreset.Front:
                return new Vector3(bounds.center.x, bounds.center.y, bounds.max.z);
            case PivotPreset.Back:
                return new Vector3(bounds.center.x, bounds.center.y, bounds.min.z);
            default:
                return bounds.center;
        }
    }

    private Vector3 GetPivotPositionFromLocalBounds(Bounds bounds, PivotPreset preset)
    {
        // For local bounds, center is local center, min/max are local min/max
        switch (preset)
        {
            case PivotPreset.Center:
                return bounds.center;
            case PivotPreset.BottomCenter:
                return new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
            case PivotPreset.TopCenter:
                return new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
            case PivotPreset.Left:
                return new Vector3(bounds.min.x, bounds.center.y, bounds.center.z);
            case PivotPreset.Right:
                return new Vector3(bounds.max.x, bounds.center.y, bounds.center.z);
            case PivotPreset.Front:
                return new Vector3(bounds.center.x, bounds.center.y, bounds.max.z);
            case PivotPreset.Back:
                return new Vector3(bounds.center.x, bounds.center.y, bounds.min.z);
            default:
                return bounds.center;
        }
    }
}
