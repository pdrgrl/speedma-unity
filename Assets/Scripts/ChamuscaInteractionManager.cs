using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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

        // Clear hover states and exit early if clicking or hovering over UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            ClearHover();
            return;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // 1. Hover logic
        if (Physics.Raycast(ray, out hit) && hit.distance <= maxInteractDistance)
        {
            ChamuscaInteractable interactable =
                hit.collider.GetComponentInParent<ChamuscaInteractable>();
            if (interactable != currentHovered)
            {
                if (currentHovered != null)
                    currentHovered.SetHover(false);
                
                currentHovered = interactable;
                
                if (currentHovered != null)
                {
                    currentHovered.SetHover(true);
                    
                    // Show hovered name on UI focus label
                    if (uiManager != null && uiManager.focusLabelText != null)
                    {
                        uiManager.focusLabelText.text = currentHovered.GetHoverText();
                    }
                }
            }
        }
        else
        {
            ClearHover();
        }

        // 2. Click logic
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentHovered != null && uiManager != null)
                uiManager.SetFocus(currentHovered.componentId, currentHovered.displayName);
        }
    }

    private void ClearHover()
    {
        if (currentHovered != null)
        {
            currentHovered.SetHover(false);
            currentHovered = null;
            
            // Restore focus label back to selected component name or empty
            if (uiManager != null && uiManager.focusLabelText != null)
            {
                uiManager.focusLabelText.text = string.IsNullOrEmpty(uiManager.currentFocusId) 
                    ? "" 
                    : "Focus: " + uiManager.currentFocusDisplayName;
            }
        }
    }
}
