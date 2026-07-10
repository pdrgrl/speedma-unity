using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Speedma;

namespace Chamusca.Simulation
{
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

        [Header("Materials")]
        [Tooltip("The MeshRenderer of the main fuse body only.")]
        public MeshRenderer fuseRenderer;
        [Tooltip("Material used when the fuse is working normally.")]
        public Material intactMaterial;
        [Tooltip("Material used when the fuse is blown.")]
        public Material blownMaterial;

        [Header("Positions & Rotations")]
        public Vector3 installedLocalPosition;
        public Vector3 blownLocalPosition;
        public Vector3 installedLocalRotation;
        public Vector3 blownLocalRotation;
        public float animationSpeed = 10f;

        [Header("State")]
        [SerializeField] private bool installed = true;
        [SerializeField] private bool blown = false;
        private bool _debugOverridden = false;

        private bool lastBlownState;

        private void Start()
        {
            if (fuseBody != null)
            {
                fuseBody.localPosition = installedLocalPosition;
                fuseBody.localRotation = Quaternion.Euler(installedLocalRotation);
                
                // Se não foi arrastado no Inspector, pega APENAS o renderer do próprio fuseBody
                if (fuseRenderer == null)
                {
                    fuseRenderer = fuseBody.GetComponent<MeshRenderer>();
                }
            }

            lastBlownState = blown;
            UpdateVisualState(blown);
        }

        private void Update()
        {
            // Debug trigger: Press F to toggle visual fuse blow override
            if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
            {
                _debugOverridden = !_debugOverridden;
                blown = _debugOverridden;
                installed = !_debugOverridden;
                Debug.Log($"[Debug Fuse] Toggled fuse '{name}' visually. Blown: {blown}");
            }

            if (!_debugOverridden && simManager != null && simManager.IsSessionActive)
            {
                blown = simManager.GetOutput(fuseStateOutput) > 0.5f;
                installed = !blown;
            }

            // Monitoriza a mudança de estado frame a frame
            if (blown != lastBlownState)
            {
                lastBlownState = blown;
                UpdateVisualState(blown);
            }

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && Camera.main != null)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;

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

            if (fuseBody == null) return;

            Vector3 targetPos = installed ? installedLocalPosition : blownLocalPosition;
            Vector3 targetRot = installed ? installedLocalRotation : blownLocalRotation;

            fuseBody.localPosition = Vector3.Lerp(fuseBody.localPosition, targetPos, Time.deltaTime * animationSpeed);
            fuseBody.localRotation = Quaternion.Lerp(fuseBody.localRotation, Quaternion.Euler(targetRot), Time.deltaTime * animationSpeed);
        }

        private void UpdateVisualState(bool isBlown)
        {
            if (fuseRenderer == null) return;

            Material targetMaterial = isBlown ? blownMaterial : intactMaterial;
            if (targetMaterial != null)
            {
                // Altera apenas o material do componente principal
                fuseRenderer.material = targetMaterial;
            }
        }

        public void TryReplaceFuse()
        {
            if (!blown) return;

            _debugOverridden = false; // Reset debug override on replacement
            if (simManager == null || !simManager.IsSessionActive)
            {
                blown = false;
                installed = true;
                lastBlownState = false;
                UpdateVisualState(false);
            }

            if (controller != null)
                controller.RequestProtectionReset();
        }
    }
}
