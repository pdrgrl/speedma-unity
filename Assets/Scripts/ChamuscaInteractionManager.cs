using UnityEngine;
using UnityEngine.InputSystem;

public class ChamuscaInteractionManager : MonoBehaviour {
    public ChamuscaUIManager uiManager;
    private ChamuscaInteractable currentHovered;

    void Update() {
        if (Mouse.current == null || Camera.main == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // 1. Handle Hover Logic (Since OnMouseEnter/Exit don't work reliably with New Input System)
        if (Physics.Raycast(ray, out hit)) {
            ChamuscaInteractable interactable = hit.collider.GetComponent<ChamuscaInteractable>();
            
            // If we hit a new interactable, set it to hover state
            if (interactable != currentHovered) {
                if (currentHovered != null) currentHovered.SetHover(false);
                currentHovered = interactable;
                if (currentHovered != null) currentHovered.SetHover(true);
            }
        } else {
            // If we hit nothing, clear the hover state
            if (currentHovered != null) {
                currentHovered.SetHover(false);
                currentHovered = null;
            }
        }

        // 2. Handle Click Logic
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            if (currentHovered != null && uiManager != null) {
                uiManager.SetFocus(currentHovered.componentId, currentHovered.displayName);
            }
        }
    }
}
