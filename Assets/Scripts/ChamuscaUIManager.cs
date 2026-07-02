using TMPro;
using UnityEngine;

/// <summary>
/// Manages the in-scene focus label AND notifies the embedding web page when a
/// component is focused. The landing-page chat widget listens to
/// window.onUnityComponentSelected / window.onUnityComponentDeselected and
/// calls the RAG backend directly.
/// </summary>
public class ChamuscaUIManager : MonoBehaviour
{
    // ── jslib bridge (WebGL only) ─────────────────────────────────────────
#if UNITY_WEBGL && !UNITY_EDITOR
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void NotifyComponentSelected(string componentId, string displayName);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void NotifyComponentDeselected();
#endif

    [Header("UI References")]
    /// Optional label that shows what component is currently focused.
    public TextMeshProUGUI focusLabelText;

    [Header("State")]
    public string currentFocusId = "";
    public string currentFocusDisplayName = "";

    // ── Public API ────────────────────────────────────────────────────────

    /// Called by ChamuscaInteractionManager when the user clicks a component.
    public void SetFocus(string componentId, string displayName)
    {
        currentFocusId = componentId;
        currentFocusDisplayName = displayName;

        // Update the in-scene label (if wired up)
        if (focusLabelText != null)
            focusLabelText.text = string.IsNullOrEmpty(componentId) ? "Select a component to inspect" : "Focus: " + displayName;
        else
            Debug.Log("[UIManager] Focus → " + displayName + " (" + componentId + ")");

        // Notify the host web page
#if UNITY_WEBGL && !UNITY_EDITOR
        if (string.IsNullOrEmpty(componentId))
            NotifyComponentDeselected();
        else
            NotifyComponentSelected(componentId, displayName);
#endif
    }

    /// Call this to clear the current focus (e.g. click on empty space).
    public void ClearFocus() => SetFocus("", "");
}
