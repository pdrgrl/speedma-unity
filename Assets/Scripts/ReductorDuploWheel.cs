using Chamusca.Simulation;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Speedma;


/// <summary>
/// Drag-to-adjust selector for the battery discharge side of the double reductor.
/// 
/// Physical mapping:
/// - Lower disk / blue handles: discharge selector used in Scenario A.
/// - Discrete steps: each step maps to one tap/cell index sent to the FMU as reductor_pos.
/// - Visual rotation is smoothed, but the simulation input remains discrete.
/// </summary>
public class ReductorDuploWheel : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Optional FMU/scene link if you want to mirror the value elsewhere in Unity.")]
    public FmuSceneLink fmuLink;

    [Tooltip("Optional simulation controller. If assigned, currentCell is used directly by ChamuscaSimController.")]
    public BatteryCellSelector selector;

    [Tooltip("The rotating disk/handle assembly for the discharge selector.")]
    public Transform wheel;

    [Tooltip("The orbit camera that should lock while the wheel is being dragged.")]
    public InspectionCamera orbitCamera;

    [Header("Selector Range")]
    [Tooltip("Minimum cell/tap index for the discharge side.")]
    public int minCell = 40;

    [Tooltip("Maximum cell/tap index for the discharge side.")]
    public int maxCell = 60;

    [Tooltip("Initial cell/tap index.")]
    public int initialCell = 40;

    [Header("Calibration")]
    public float baseAngleOffset = 0f;

    [Header("Angle Mapping")]
    [Tooltip("Wheel angle for minCell.")]
    public float angleAtMinCell = -104f;

    [Tooltip("Wheel angle for maxCell.")]
    public float angleAtMaxCell = 104f;

    [Header("Drag Interaction")]
    [Tooltip("How many pixels of horizontal drag are required to change one step.")]
    public float pixelsPerStep = 18f;

    [Tooltip("Flip drag direction if the wheel turns opposite to the intended direction.")]
    public bool invertDrag = false;

    [Tooltip("Visual interpolation speed for the wheel.")]
    public float rotationSpeed = 12f;

    [Header("UI Feedback (optional)")]
    public TextMeshProUGUI grabHintText;

    [Header("Interaction Range")]
    [Tooltip("Maximum distance at which the wheel can be grabbed.")]
    public float maxInteractDistance = 2.0f;

    private bool _grabbed;
    private float _lastMouseX;
    private int _currentCell;
    private float _currentAngle;
    private float _dragAccumulator;

    private void Awake()
    {
        _currentCell = Mathf.Clamp(initialCell, minCell, maxCell);
    }

    private void Start()
    {
        SyncSelectorToVisual();
        _currentAngle = CellToAngle(_currentCell);
        ApplyAngle(_currentAngle);
        SetHintVisible(false);
        Debug.Log($"Cell {selector.currentCell} -> angle {CellToAngle(selector.currentCell)}");
    }

    private void Update()
    {
        if (wheel == null)
            return;

        if (Mouse.current == null)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        bool clicked = Mouse.current.leftButton.wasPressedThisFrame;

        if (!_grabbed)
        {
            if (clicked && Camera.main != null)
            {
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
            float deltaX = mousePos.x - _lastMouseX;
            _lastMouseX = mousePos.x;

            if (invertDrag)
                deltaX = -deltaX;

            _dragAccumulator += deltaX;

            while (_dragAccumulator >= pixelsPerStep)
            {
                Step(+1);
                _dragAccumulator -= pixelsPerStep;
            }

            while (_dragAccumulator <= -pixelsPerStep)
            {
                Step(-1);
                _dragAccumulator += pixelsPerStep;
            }

            bool escape =
                Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
            if (clicked || escape)
                Release();
        }

        float rawTarget = CellToAngle(_currentCell);
        _currentAngle = Mathf.LerpAngle(_currentAngle, rawTarget, Time.deltaTime * rotationSpeed);
        ApplyAngle(_currentAngle);
    }

    public void SetCell(int cell)
    {
        _currentCell = Mathf.Clamp(cell, minCell, maxCell);
        SyncSelectorToVisual();
    }

    public int GetCell()
    {
        return _currentCell;
    }

    private void Step(int delta)
    {
        _currentCell = Mathf.Clamp(_currentCell + delta, minCell, maxCell);
        SyncSelectorToVisual();
    }

    private void SyncSelectorToVisual()
    {
        if (selector != null)
            selector.currentCell = _currentCell;

        if (fmuLink != null)
        {
            // Kept for parity with the rheostat pattern; the actual FMU input is read by the
            // simulation controller via selector.currentCell.
        }
    }

    private void Grab(float mouseX)
    {
        _grabbed = true;
        _lastMouseX = mouseX;
        _dragAccumulator = 0f;

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

    private float CellToAngle(int cell)
    {
        float t = Mathf.InverseLerp(minCell, maxCell, cell);
        return Mathf.Lerp(angleAtMinCell, angleAtMaxCell, t);
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
