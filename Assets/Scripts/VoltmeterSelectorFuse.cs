using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Click to cycle the plug-in wooden voltmeter selector bar between vertical slots:
/// - Slot 0 (Top): Descarga (Discharge batteries voltage)
/// - Slot 1 (Middle): Carga (Charge batteries voltage)
/// - Slot 2 (Bottom): Dínamo (Dynamo voltage)
/// </summary>
public class VoltmeterSelectorFuse : MonoBehaviour
{
    [Header("Slots Setup")]
    [Tooltip("The child Transform that slides between slots. If null, slides the root.")]
    public Transform selectorHandle;

    [Tooltip("The local offset between each slot. Typically downwards.")]
    public Vector3 slotOffset = new Vector3(0f, -0.07f, 0f);

    [Tooltip("Visual transition speed between slots.")]
    public float slideSpeed = 8f;

    [Tooltip("Maximum distance at which the selector can be clicked.")]
    public float maxInteractDistance = 2.5f;

    [Header("Current Selection")]
    [Range(0, 2)]
    public int selectedSlot = 0; // 0 = Descarga, 1 = Carga, 2 = Dynamo

    private Vector3 _startLocalPos;
    private Vector3 _targetLocalPos;

    private void Start()
    {
        // Default to self if no handle assigned
        if (selectorHandle == null) selectorHandle = transform;

        _startLocalPos = selectorHandle.localPosition;
        _targetLocalPos = _startLocalPos;

        // Auto-attach BoxCollider if missing
        // Collider col = GetComponent<Collider>();
        // if (col == null)
        // {
        //     gameObject.AddComponent<BoxCollider>();
        // }
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && Camera.main != null)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.distance <= maxInteractDistance)
            {
                if (hit.transform == transform || hit.transform.IsChildOf(transform))
                {
                    CycleSlot();
                }
            }
        }

        // Smooth visual slide animation on target handle
        selectorHandle.localPosition = Vector3.Lerp(selectorHandle.localPosition, _targetLocalPos, Time.deltaTime * slideSpeed);
    }

    private void CycleSlot()
    {
        selectedSlot = (selectedSlot + 1) % 3;
        _targetLocalPos = _startLocalPos + (slotOffset * selectedSlot);
        Debug.Log($"[VoltmeterSelector] Switched to Slot {selectedSlot} ({(selectedSlot == 0 ? "Descarga" : selectedSlot == 1 ? "Carga" : "Dínamo")})");
    }
}
