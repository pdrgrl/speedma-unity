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
        if (Camera.main == null)
            return;

        bool hasInput = false;
        Vector2 inputPosition = Vector2.zero;
        bool isClickOrTap = false;

        // 1. Read Mouse input by default
        if (Mouse.current != null)
        {
            inputPosition = Mouse.current.position.ReadValue();
            hasInput = true;
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                isClickOrTap = true;
            }
        }

        // 2. Override with Touch input if active touches are present
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            var touch = Touchscreen.current.touches[0];
            if (touch.press.isPressed)
            {
                inputPosition = touch.position.ReadValue();
                hasInput = true;
                // Treat touch release as the click trigger to prevent click-through issues
            }
            else if (touch.press.wasReleasedThisFrame)
            {
                inputPosition = touch.position.ReadValue();
                hasInput = true;
                isClickOrTap = true;
            }
        }

        if (!hasInput)
            return;

        // Clear hover states and exit early if clicking or hovering over UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            ClearHover();
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
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
        if (isClickOrTap)
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
