using UnityEngine;
using UnityEngine.InputSystem;
using Speedma;

namespace Chamusca.Simulation
{
    /// <summary>
    /// Click-to-replace fuse control.
    ///
    /// Physical mapping:
    /// - The fuse is shown as installed or removed depending on the FMU state.
    /// - When the fuse is blown, clicking the fuse replaces it and requests a protection reset.
    /// - The visual can move along a local axis to simulate insertion/removal.
    /// </summary>
    public class FmuReplaceableFuse : MonoBehaviour
    {
        [Header("Backend")]
        public SpeedmaSimManager simManager;
        public ChamuscaSimController controller;

        [Tooltip("FMU output that is 0 when fuse is OK and 1 when blown/open.")]
        public string fuseStateOutput = "fuseDischargeState";

        [Header("Interaction")]
        public float maxInteractDistance = 2.0f;
        public Transform fuseBody;

        [Tooltip("Local position when the fuse is installed.")]
        public Vector3 installedLocalPosition;

        [Tooltip("Local position when the fuse is blown/removed.")]
        public Vector3 blownLocalPosition;

        [Tooltip("Local rotation when the fuse is installed.")]
        public Vector3 installedLocalRotation;

        [Tooltip("Local rotation when the fuse is blown/removed.")]
        public Vector3 blownLocalRotation;

        [Tooltip("How fast the fuse visual moves/rotates.")]
        public float animationSpeed = 10f;

        [Header("State")]
        [SerializeField]
        private bool installed = true;

        [SerializeField]
        private bool blown = false;

        private void Start()
        {
            if (fuseBody != null)
            {
                fuseBody.localPosition = installedLocalPosition;
                fuseBody.localRotation = Quaternion.Euler(installedLocalRotation);
            }
        }

        private void Update()
        {
            if (simManager != null && simManager.IsSessionActive)
            {
                blown = simManager.GetOutput(fuseStateOutput) > 0.5f;
                installed = !blown;
            }

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (
                    Physics.Raycast(ray, out RaycastHit hit)
                    && hit.distance <= maxInteractDistance
                    && (hit.transform == fuseBody || hit.transform.IsChildOf(fuseBody))
                )
                {
                    TryReplaceFuse();
                }
            }

            if (fuseBody == null)
                return;

            Vector3 targetPos = installed ? installedLocalPosition : blownLocalPosition;
            Vector3 targetRot = installed ? installedLocalRotation : blownLocalRotation;

            fuseBody.localPosition = Vector3.Lerp(
                fuseBody.localPosition,
                targetPos,
                Time.deltaTime * animationSpeed
            );
            fuseBody.localRotation = Quaternion.Lerp(
                fuseBody.localRotation,
                Quaternion.Euler(targetRot),
                Time.deltaTime * animationSpeed
            );
        }

        public void TryReplaceFuse()
        {
            if (!blown)
                return;

            installed = true;
            blown = false;

            if (controller != null)
                controller.RequestProtectionReset();
        }
    }
}
