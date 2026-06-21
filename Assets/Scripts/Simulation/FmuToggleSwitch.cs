using UnityEngine;
using UnityEngine.InputSystem;
using Speedma;

namespace Chamusca.Simulation
{
    /// <summary>
    /// Generic click-to-toggle switch for Scenario A marble-board controls.
    /// 
    /// Physical mapping:
    /// - switchHandle: the lever or button visual that animates when clicked.
    /// - inputName: the FMU input set to true/false.
    /// - local visual state is kept in sync with the simulated state.
    /// </summary>
    public class FmuToggleSwitch : MonoBehaviour
    {
        [Header("Backend")]
        public SpeedmaSimManager simManager;

        [Tooltip("FMU input name to drive.")]
        public string inputName = "sw_casa_luz";

        [Header("Interaction")]
        public float maxInteractDistance = 2.5f;
        public Transform switchHandle;
        public Vector3 rotationOff = new Vector3(0f, 0f, 0f);
        public Vector3 rotationOn = new Vector3(0f, 0f, 45f);
        public float rotationSpeed = 8f;

        [Header("State")]
        public bool switchOn = false;

        private Quaternion _targetRotation;

        private void Start()
        {
            _targetRotation = Quaternion.Euler(rotationOff);
            if (switchHandle != null)
                switchHandle.localRotation = _targetRotation;
        }

        private void Update()
        {
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
            switchOn = !switchOn;
            _targetRotation = Quaternion.Euler(switchOn ? rotationOn : rotationOff);
            if (simManager != null)
                simManager.SetInput(inputName, switchOn);
        }

        public void SetSwitch(bool on)
        {
            switchOn = on;
            _targetRotation = Quaternion.Euler(switchOn ? rotationOn : rotationOff);
            if (simManager != null)
                simManager.SetInput(inputName, switchOn);
        }
    }
}
