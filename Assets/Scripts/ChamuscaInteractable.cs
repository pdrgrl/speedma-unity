using UnityEngine;

public class ChamuscaInteractable : MonoBehaviour
{
    [Tooltip("The internal ID used by the backend RAG (e.g., crossley_engine)")]
    public string componentId;

    [Tooltip("The human-readable name displayed in the UI")]
    public string displayName;

    [Header("Hover Target (choose one)")]
    [Tooltip(
        "If set, this GameObject's first Renderer (including children) will be used for hover visuals."
    )]
    public GameObject hoverTargetObject;

    [Tooltip(
        "If set, this specific Renderer will be used for hover visuals. Overrides Hover Target Object."
    )]
    public Renderer hoverTargetRenderer;

    [Tooltip("Color used when hovered.")]
    public Color hoverColor = Color.yellow;

    [Header("Hover Label overrides")]
    [Tooltip("Optional: Custom display name for hover. Falls back to displayName if empty.")]
    public string customHoverName = "";

    private Color originalColor;
    private Renderer rend;
    private bool isHovered = false;

    void Start()
    {
        // Priority: explicit renderer -> object (find child renderer) -> this object's child renderer
        if (hoverTargetRenderer != null)
        {
            rend = hoverTargetRenderer;
        }
        else if (hoverTargetObject != null)
        {
            rend = hoverTargetObject.GetComponentInChildren<Renderer>();
        }
        else
        {
            rend = GetComponentInChildren<Renderer>();
        }

        if (rend != null)
            originalColor = rend.material.color;
    }

    public void SetHover(bool hover)
    {
        if (isHovered == hover)
            return; // Prevent unnecessary material updates
        isHovered = hover;

        if (rend != null)
        {
            if (isHovered)
            {
                rend.material.color = hoverColor;
            }
            else
            {
                rend.material.color = originalColor;
            }
        }
    }

    public string GetHoverText()
    {
        return !string.IsNullOrEmpty(customHoverName) ? customHoverName : displayName;
    }
}
