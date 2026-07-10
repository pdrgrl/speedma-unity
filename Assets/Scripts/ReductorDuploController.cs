using Chamusca.Simulation;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Speedma;

/// <summary>
/// Dual handle mechanical selector representing the Tudor "Reductor Duplo".
/// Handles are color-coded:
/// - Blue Handle: Descarga (Discharge, powers the house).
/// - Brown Handle: Carga (Charge, charging path from generator).
/// Enforces lock-pin translation, discrete cell step rotation, and boundary collision:
/// - Blue (Descarga) cell index must always be <= Brown (Carga) cell index.
/// </summary>
public class ReductorDuploController : MonoBehaviour
    {
        [Header("Simulation References")]
        public SpeedmaSimManager simManager;

        [Header("Visual Handles")]
        [Tooltip("The Blue rotating arm representing the discharge path (Descarga).")]
        public Transform blueHandle;

        [Tooltip("The Brown rotating arm representing the charging path (Carga).")]
        public Transform brownHandle;

        [Header("Lock Pins")]
        [Tooltip("The Blue pin/pull-knob that unlocks the Blue Handle.")]
        public Transform bluePin;

        [Tooltip("The Brown pin/pull-knob that unlocks the Brown Handle.")]
        public Transform brownPin;

        [Tooltip("Translation offset when a pin is pulled down (unlocked).")]
        public Vector3 pinUnlockedOffset = new Vector3(0f, -0.04f, 0f);

        [Header("Selector Range")]
        [Tooltip("Minimum cell index (typically 41).")]
        public int minCell = 41;

        [Tooltip("Maximum cell index (typically 60).")]
        public int maxCell = 60;

        [Tooltip("Initial Discharge (Blue) cell index.")]
        public int initialDischargeCell = 41;

        [Tooltip("Initial Charge (Brown) cell index.")]
        public int initialChargeCell = 60;

        [Header("Angle Mapping")]
        public float blueMinAngle = -110f;
        public float blueMaxAngle = 77f;
        public float brownMinAngle = 59f;
        public float brownMaxAngle = 250f;
        public float rotationSpeed = 12f;

        [Header("Drag Interaction")]
        public float pixelsPerStep = 18f;
        public bool invertDrag = false;
        public float maxInteractDistance = 2.5f;

        [Header("UI Feedback")]
        public TextMeshProUGUI grabHintText;

        [Header("Orbit Lock")]
        public InspectionCamera orbitCamera;

        [Header("Current State")]
        public bool blueUnlocked = false;
        public bool brownUnlocked = false;
        public int dischargeCell = 41;
        public int chargeCell = 60;

        private bool _isDraggingBlue = false;
        private bool _isDraggingBrown = false;
        private float _lastMouseX;
        private float _dragAccumulator;

        private Vector3 _bluePinLocalStart;
        private Vector3 _brownPinLocalStart;

        private float _currentBlueAngle;
        private float _currentBrownAngle;

        private void Start()
        {
            dischargeCell = Mathf.Clamp(initialDischargeCell, minCell, maxCell);
            chargeCell = Mathf.Clamp(initialChargeCell, minCell, maxCell);

            // Enforce historical constraint initially
            if (dischargeCell > chargeCell)
                dischargeCell = chargeCell;

            _currentBlueAngle = CellToAngle(dischargeCell, true);
            _currentBrownAngle = CellToAngle(chargeCell, false);

            if (blueHandle != null) blueHandle.localEulerAngles = new Vector3(0f, 0f, _currentBlueAngle);
            if (brownHandle != null) brownHandle.localEulerAngles = new Vector3(0f, 0f, _currentBrownAngle);

            if (bluePin != null) _bluePinLocalStart = bluePin.localPosition;
            if (brownPin != null) _brownPinLocalStart = brownPin.localPosition;

            SetHintVisible(false);
            UpdateSimulationInputs();
        }

        private void Update()
        {
            if (Mouse.current == null) return;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            bool clicked = Mouse.current.leftButton.wasPressedThisFrame;

            if (clicked && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            // Handle Dragging state
            if (_isDraggingBlue || _isDraggingBrown)
            {
                float deltaX = mousePos.x - _lastMouseX;
                _lastMouseX = mousePos.x;

                if (invertDrag) deltaX = -deltaX;
                _dragAccumulator += deltaX;

                while (_dragAccumulator >= pixelsPerStep)
                {
                    StepSelectedHandle(1);
                    _dragAccumulator -= pixelsPerStep;
                }
                while (_dragAccumulator <= -pixelsPerStep)
                {
                    StepSelectedHandle(-1);
                    _dragAccumulator += pixelsPerStep;
                }

                bool escape = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
                if (clicked || escape)
                {
                    ReleaseDrag();
                }
            }
            else
            {
                // Handle Click to Unlock Pins or Click to Drag Handles
                if (clicked && Camera.main != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(mousePos);
                    if (Physics.Raycast(ray, out RaycastHit hit) && hit.distance <= maxInteractDistance)
                    {
                        // Check Pin clicks
                        if (bluePin != null && (hit.transform == bluePin || hit.transform.IsChildOf(bluePin)))
                        {
                            ToggleBluePin();
                        }
                        else if (brownPin != null && (hit.transform == brownPin || hit.transform.IsChildOf(brownPin)))
                        {
                            ToggleBrownPin();
                        }
                        // Check Handle clicks (only allowed if unlocked)
                        else if (blueUnlocked && blueHandle != null && (hit.transform == blueHandle || hit.transform.IsChildOf(blueHandle)))
                        {
                            StartDrag(true, mousePos.x);
                        }
                        else if (brownUnlocked && brownHandle != null && (hit.transform == brownHandle || hit.transform.IsChildOf(brownHandle)))
                        {
                            StartDrag(false, mousePos.x);
                        }
                    }
                }
            }

            // Animate Handle Rotations
            if (blueHandle != null)
            {
                float targetBlue = CellToAngle(dischargeCell, true);
                _currentBlueAngle = Mathf.LerpAngle(_currentBlueAngle, targetBlue, Time.deltaTime * rotationSpeed);
                blueHandle.localEulerAngles = new Vector3(0f, 0f, _currentBlueAngle);
            }

            if (brownHandle != null)
            {
                float targetBrown = CellToAngle(chargeCell, false);
                _currentBrownAngle = Mathf.LerpAngle(_currentBrownAngle, targetBrown, Time.deltaTime * rotationSpeed);
                brownHandle.localEulerAngles = new Vector3(0f, 0f, _currentBrownAngle);
            }
        }

        private void ToggleBluePin()
        {
            blueUnlocked = !blueUnlocked;
            if (bluePin != null)
            {
                bluePin.localPosition = _bluePinLocalStart + (blueUnlocked ? pinUnlockedOffset : Vector3.zero);
            }
            Debug.Log($"[RedutorDuplo] Blue Handle (Descarga) {(blueUnlocked ? "UNLOCKED" : "LOCKED")}");
            
            // If locking while dragging, release drag
            if (!blueUnlocked && _isDraggingBlue) ReleaseDrag();
        }

        private void ToggleBrownPin()
        {
            brownUnlocked = !brownUnlocked;
            if (brownPin != null)
            {
                brownPin.localPosition = _brownPinLocalStart + (brownUnlocked ? pinUnlockedOffset : Vector3.zero);
            }
            Debug.Log($"[RedutorDuplo] Brown Handle (Carga) {(brownUnlocked ? "UNLOCKED" : "LOCKED")}");
            
            if (!brownUnlocked && _isDraggingBrown) ReleaseDrag();
        }

        private void StartDrag(bool isBlue, float mouseX)
        {
            _isDraggingBlue = isBlue;
            _isDraggingBrown = !isBlue;
            _lastMouseX = mouseX;
            _dragAccumulator = 0f;

            if (orbitCamera != null) orbitCamera.IsLocked = true;
            SetHintVisible(true);
            Debug.Log($"[RedutorDuplo] Grabbed {(isBlue ? "Blue" : "Brown")} Handle");
        }

        private void ReleaseDrag()
        {
            _isDraggingBlue = false;
            _isDraggingBrown = false;

            if (orbitCamera != null) orbitCamera.IsLocked = false;
            SetHintVisible(false);
            Debug.Log("[RedutorDuplo] Released Handle");
        }

        private void StepSelectedHandle(int step)
        {
            if (_isDraggingBlue)
            {
                // Discharge (Blue) can go up to Charge (Brown)
                dischargeCell = Mathf.Clamp(dischargeCell + step, minCell, chargeCell);
                Debug.Log($"[RedutorDuplo] Step Blue (Descarga) -> cell {dischargeCell}");
            }
            else if (_isDraggingBrown)
            {
                // Charge (Brown) must stay equal or higher than Discharge (Blue)
                chargeCell = Mathf.Clamp(chargeCell + step, dischargeCell, maxCell);
                Debug.Log($"[RedutorDuplo] Step Brown (Carga) -> cell {chargeCell}");
            }

            UpdateSimulationInputs();
        }

        private void UpdateSimulationInputs()
        {
            if (simManager != null && simManager.IsSessionActive)
            {
                // Send discharge and charge indices to the FMU
                simManager.SetInput("reductor_descarga_pos", dischargeCell);
                simManager.SetInput("reductor_carga_pos", chargeCell);
            }
        }

        private float CellToAngle(int cell, bool isBlue)
        {
            float t = Mathf.InverseLerp(minCell, maxCell, cell);
            if (isBlue)
                return Mathf.Lerp(blueMinAngle, blueMaxAngle, t);
            else
                return Mathf.Lerp(brownMinAngle, brownMaxAngle, t);
        }

        private void SetHintVisible(bool visible)
        {
            if (grabHintText != null)
                grabHintText.text = visible ? "← Drag to rotate dial  •  Click to release →" : "";
        }
    }
