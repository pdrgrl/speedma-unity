using UnityEngine;

public class ChamuscaInteractable : MonoBehaviour {
    [Tooltip("The internal ID used by the backend RAG (e.g., crossley_engine)")]
    public string componentId;
    
    [Tooltip("The human-readable name displayed in the UI")]
    public string displayName;

    private Color originalColor;
    private Renderer rend;
    private bool isHovered = false;

    void Start() {
        rend = GetComponent<Renderer>();
        if (rend != null) {
            originalColor = rend.material.color;
        }
    }

    public void SetHover(bool hover) {
        if (isHovered == hover) return; // Prevent unnecessary material updates
        isHovered = hover;
        
        if (rend != null) {
            if (isHovered) {
                // Change to a visible hover color
                rend.material.color = Color.yellow;
            } else {
                // Revert to original
                rend.material.color = originalColor;
            }
        }
    }
}
