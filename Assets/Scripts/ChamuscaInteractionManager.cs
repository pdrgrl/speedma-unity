using UnityEngine;
using UnityEngine.InputSystem;

public class ChamuscaInteractionManager : MonoBehaviour
{
    public ChamuscaUIManager uiManager;

    [Header("Interaction Range")]
    [Tooltip("Maximum distance (world units) at which components can be hovered or clicked.")]
    public float maxInteractDistance = 3.0f;

    private ChamuscaInteractable currentHovered;

    void Update()
    {
        if (Mouse.current == null || Camera.main == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // 1. Hover logic
        if (Physics.Raycast(ray, out hit) && hit.distance <= maxInteractDistance)
        {
            ChamuscaInteractable interactable = hit.collider.GetComponent<ChamuscaInteractable>();

            if (interactable != currentHovered)
            {
                if (currentHovered != null)
                    currentHovered.SetHover(false);
                currentHovered = interactable;
                if (currentHovered != null)
                    currentHovered.SetHover(true);
            }
        }
        else
        {
            if (currentHovered != null)
            {
                currentHovered.SetHover(false);
                currentHovered = null;
            }
        }

        // 2. Click logic
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentHovered != null && uiManager != null)
                uiManager.SetFocus(currentHovered.componentId, currentHovered.displayName);
        }
    }
}
