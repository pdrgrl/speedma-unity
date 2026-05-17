// Assets/Scripts/RheostatWheel.cs
using Speedma;
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
    public FmuSceneLink fmuLink;

    [Tooltip("The 'wheel' child Transform of SM_Rheostat.")]
    public Transform wheel;

    [Tooltip("The orbit camera — locked while wheel is grabbed.")]
    public InspectionCamera orbitCamera;

    [Header("Angle Mapping")]
    public float angleAtMaxResistance = 0f; // Z=0°   → high resistance, low amps
    public float angleAtMinResistance = 334f; // Z=334° → low resistance, high amps

    [Header("Drag Interaction")]
    [Tooltip("Ohms changed per pixel of horizontal mouse movement. Raise for more sensitivity.")]
    public float ohmPerPixel = 0.1f;

    [Tooltip("Flip drag direction if needed.")]
    public bool invertDrag = false;

    [Tooltip("Visual lerp speed.")]
    public float rotationSpeed = 10f;

    [Header("UI Feedback (optional)")]
    public TextMeshProUGUI grabHintText;

    // ── State ──────────────────────────────────────────────────────────────
    private bool _grabbed = false;
    private float _currentAngle;
    private float _lastMouseX;

    private void Start()
    {
        if (fmuLink == null || wheel == null)
            return;
        _currentAngle = ResistanceToAngle(fmuLink.RheostatValue);
        ApplyAngle(_currentAngle);
        SetHintVisible(false);
    }

    private void Update()
    {
        if (fmuLink == null || wheel == null)
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
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    bool overWheel = hit.transform == wheel || hit.transform.IsChildOf(wheel);
                    if (overWheel)
                        Grab(mousePos.x);
                }
            }
        }
        else
        {
            // ── Grabbed: horizontal delta → resistance ────────────────────
            float deltaX = mousePos.x - _lastMouseX;
            _lastMouseX = mousePos.x;

            if (invertDrag)
                deltaX = -deltaX;
            fmuLink.RheostatValue -= deltaX * ohmPerPixel;

            bool escape =
                Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
            if (clicked || escape)
                Release();
        }

        // ── Visual ────────────────────────────────────────────────────────
        float rawTarget = ResistanceToAngle(fmuLink.RheostatValue);
        _currentAngle = Mathf.LerpAngle(_currentAngle, rawTarget, Time.deltaTime * rotationSpeed);

        // Hard clamp: wheel cannot go outside [0°, 334°]
        _currentAngle = Mathf.Clamp(_currentAngle, 0f, 334f);

        // Keep resistance in sync with the clamped angle
        fmuLink.RheostatValue = AngleToResistance(_currentAngle);

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

    /// Resistance → Z angle
    private float ResistanceToAngle(float resistance)
    {
        float t = Mathf.InverseLerp(fmuLink.RheostatMax, fmuLink.RheostatMin, resistance);
        return Mathf.Lerp(angleAtMaxResistance, angleAtMinResistance, t);
    }

    /// Z angle → Resistance (inverse of above)
    private float AngleToResistance(float angle)
    {
        float t = Mathf.InverseLerp(angleAtMaxResistance, angleAtMinResistance, angle);
        return Mathf.Lerp(fmuLink.RheostatMax, fmuLink.RheostatMin, t);
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
