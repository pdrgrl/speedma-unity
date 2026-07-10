using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Speedma;

namespace Chamusca.Simulation
{
    /// <summary>
    /// Click-to-rearm breaker control.
    ///
    /// Physical mapping:
    /// - The breaker handle moves between open and closed positions.
    /// - When the FMU reports breakerState = 1, the breaker is visually open.
    /// - Clicking the breaker while open requests a protection reset.
    /// </summary>
    public class FmuBreakerSwitch : MonoBehaviour
    {
        [Header("Backend")]
        public SpeedmaSimManager simManager;
        public ChamuscaSimController controller;

        [Tooltip("FMU output that is 0 when the breaker is closed and 1 when open/tripped.")]
        public string breakerStateOutput = "breakerState";

        [Header("Interaction")]
        public float maxInteractDistance = 2.0f;
        public Transform breakerHandle;

        [Tooltip("Local rotation when the breaker is closed.")]
        public Vector3 closedRotation = new Vector3(0f, 0f, 0f);

        [Tooltip("Local rotation when the breaker is open.")]
        public Vector3 openRotation = new Vector3(0f, 0f, 35f);

        [Tooltip("How fast the handle moves.")]
        public float rotationSpeed = 10f;

        [Header("State")]
        [SerializeField]
        private bool isOpen = false;
        private bool _debugOverridden = false;

        public bool IsDebugTripped => _debugOverridden;

        private void Start()
        {
            if (breakerHandle != null)
                breakerHandle.localRotation = Quaternion.Euler(closedRotation);
        }

        private void Update()
        {
            // Debug trigger: Press T to toggle visual trip override
            if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
            {
                _debugOverridden = !_debugOverridden;
                isOpen = _debugOverridden;
                Debug.Log($"[Debug Breaker] Toggled breaker visually. Override active: {_debugOverridden}");
            }

            if (!_debugOverridden && simManager != null && simManager.IsSessionActive)
            {
                isOpen = simManager.GetOutput(breakerStateOutput) > 0.5f;
            }

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && Camera.main != null)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;

                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (
                    Physics.Raycast(ray, out RaycastHit hit)
                    && hit.distance <= maxInteractDistance
                    && (hit.transform == breakerHandle || hit.transform.IsChildOf(breakerHandle))
                )
                {
                    TryRearmBreaker();
                }
            }

            if (breakerHandle == null)
                return;

            Quaternion target = Quaternion.Euler(isOpen ? openRotation : closedRotation);
            breakerHandle.localRotation = Quaternion.Lerp(
                breakerHandle.localRotation,
                target,
                Time.deltaTime * rotationSpeed
            );
        }

        public void TryRearmBreaker()
        {
            if (!isOpen)
                return;

            _debugOverridden = false; // Stop debug override on rearm
            if (controller != null)
                controller.RequestProtectionReset();
        }

        public void ResetBreaker()
        {
            _debugOverridden = false;
            isOpen = false;
        }
    }
}
