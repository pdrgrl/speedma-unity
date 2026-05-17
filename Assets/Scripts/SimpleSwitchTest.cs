// Assets/Scripts/SimpleSwitchTest.cs
using Speedma;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Clicks the switch handle collider within range to toggle sw_luz.
/// Animates the handle between rotationOff and rotationOn.
/// </summary>
public class SimpleSwitchTest : MonoBehaviour
{
    [Header("Simulation Backend")]
    public FmuSceneLink fmuLink;

    [Header("Interaction Range")]
    [Tooltip("Maximum world-space distance at which the switch can be clicked.")]
    public float maxInteractDistance = 2.5f;

    [Header("Switch Visuals")]
    [Tooltip("Drag the clickable switch handle here — must have a Collider on it or a child.")]
    public Transform switchHandle;
    public Vector3 rotationOff = new Vector3(0f, 0f, 0f);
    public Vector3 rotationOn = new Vector3(0f, 0f, 45f);
    public float rotationSpeed = 5f;

    private bool _switchOn = false;
    private Quaternion _targetRotation;

    private void Start()
    {
        _targetRotation = Quaternion.Euler(rotationOff);
        if (switchHandle != null)
            switchHandle.localRotation = _targetRotation;
    }

    private void Update()
    {
        // ── Click detection ───────────────────────────────────────────
        if (
            Mouse.current != null
            && Mouse.current.leftButton.wasPressedThisFrame
            && Camera.main != null
        )
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (
                Physics.Raycast(ray, out RaycastHit hit)
                && hit.distance <= maxInteractDistance
                && (hit.transform == switchHandle || hit.transform.IsChildOf(switchHandle))
            )
            {
                ToggleSwitch();
            }
        }

        // ── Handle animation ──────────────────────────────────────────
        if (switchHandle == null)
            return;
        switchHandle.localRotation = Quaternion.Lerp(
            switchHandle.localRotation,
            _targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    public void ToggleSwitch()
    {
        _switchOn = !_switchOn;
        _targetRotation = Quaternion.Euler(_switchOn ? rotationOn : rotationOff);
        fmuLink?.SetSwLuz(_switchOn);
    }
}
