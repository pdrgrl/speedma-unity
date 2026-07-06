// Assets/Scripts/RheostatWheel.cs
using Chamusca.Simulation;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Click the wheel to grab it — camera orbit locks, mouse X drives resistance.
/// Click again or Escape to release and restore camera orbit.
/// Angle is clamped to [0°, 334°] on the Z axis.
/// </summary>
public class RheostatWheel : MonoBehaviour
{
    [Header("References")]
    public ChamuscaSimController controller;

    [Tooltip("The 'wheel' child Transform of SM_Rheostat.")]
    public Transform wheel;

    [Tooltip("The orbit camera — locked while wheel is grabbed.")]
    public InspectionCamera orbitCamera;

    [Header("Angle Mapping")]
    public float angleAtMaxResistance = 0f; // Z=0°   → high resistance, low amps (rheostat_pos = 1.0)
    public float angleAtMinResistance = 334f; // Z=334° → low resistance, high amps (rheostat_pos = 0.0)

    [Header("Drag Interaction")]
    [Tooltip("Sensitivity of mouse drag. Change per pixel of horizontal mouse movement.")]
    public float sensitivity = 0.005f;

    [Tooltip("Flip drag direction if needed.")]
    public bool invertDrag = false;

    [Tooltip("Visual lerp speed.")]
    public float rotationSpeed = 10f;

    [Header("UI Feedback (optional)")]
    public TextMeshProUGUI grabHintText;

    [Header("Interaction Range")]
    [Tooltip("Maximum distance (world units) at which the wheel can be grabbed.")]
    public float maxInteractDistance = 2.0f;

    // ── State ──────────────────────────────────────────────────────────────
    private bool _grabbed = false;
    private float _currentAngle;
    private float _lastMouseX;

    private void Start()
    {
        if (controller == null || wheel == null)
            return;
        _currentAngle = PosToAngle(controller.rheostat_pos);
        ApplyAngle(_currentAngle);
        SetHintVisible(false);
    }

    private void Update()
    {
        if (controller == null || wheel == null)
            return;
        if (Mouse.current == null)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        bool clicked = Mouse.current.leftButton.wasPressedThisFrame;

        if (!_grabbed)
        {
            // ── Wait for a click on the wheel ─────────────────────────────
            if (clicked && Camera.main != null)
            {
                // Verify we are not clicking UI
                if (UnityEngine.EventSystems.EventSystem.current != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    return;

                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.distance <= maxInteractDistance)
                {
                    bool overWheel = hit.transform == wheel || hit.transform.IsChildOf(wheel);
                    if (overWheel)
                        Grab(mousePos.x);
                }
            }
        }
        else
        {
            // ── Grabbed: horizontal delta → resistance position ───────────
            float deltaX = mousePos.x - _lastMouseX;
            _lastMouseX = mousePos.x;

            if (invertDrag)
                deltaX = -deltaX;

            // Notice: drag left (negative deltaX) should turn wheel clockwise (increase angle, decrease resistance position)
            // drag right (positive deltaX) should turn wheel counter-clockwise (decrease angle, increase resistance position)
            controller.rheostat_pos += deltaX * sensitivity;
            controller.rheostat_pos = Mathf.Clamp01(controller.rheostat_pos);

            bool escape =
                Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
            if (clicked || escape)
                Release();
        }

        // ── Visual ────────────────────────────────────────────────────────
        float rawTarget = PosToAngle(controller.rheostat_pos);
        _currentAngle = Mathf.LerpAngle(_currentAngle, rawTarget, Time.deltaTime * rotationSpeed);

        // Hard clamp: wheel cannot go outside [0°, 334°]
        _currentAngle = Mathf.Clamp(_currentAngle, 0f, 334f);

        ApplyAngle(_currentAngle);
    }

    // ── Grab / Release ─────────────────────────────────────────────────────

    private void Grab(float mouseX)
    {
        _grabbed = true;
        _lastMouseX = mouseX;
        if (orbitCamera != null)
            orbitCamera.IsLocked = true;
        SetHintVisible(true);
    }

    private void Release()
    {
        _grabbed = false;
        if (orbitCamera != null)
            orbitCamera.IsLocked = false;
        SetHintVisible(false);
    }

    // ── Conversion helpers ─────────────────────────────────────────────────

    /// Rheostat position (0.0 to 1.0) → Z angle (334.0 to 0.0)
    private float PosToAngle(float pos)
    {
        return (1f - pos) * 334f;
    }

    private void ApplyAngle(float zAngle)
    {
        Vector3 e = wheel.localEulerAngles;
        wheel.localEulerAngles = new Vector3(e.x, e.y, zAngle);
    }

    private void SetHintVisible(bool visible)
    {
        if (grabHintText != null)
            grabHintText.text = visible ? "← Drag to adjust  •  Click to release →" : "";
    }
}
