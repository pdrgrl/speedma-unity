using TMPro;
using UnityEngine;
using UnityEngine.UI;
// WebGL-only extern declarations (compiled away in Editor/standalone builds)
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

/// <summary>
/// Manages the in-scene chat UI AND notifies the embedding web page when a
/// component is focused.  The landing-page chat widget listens to
/// window.onUnityComponentSelected / window.onUnityComponentDeselected and
/// calls the RAG backend directly – no round-trip through Unity.
/// </summary>
public class ChamuscaUIManager : MonoBehaviour
{
    // ── jslib bridge (WebGL only) ─────────────────────────────────────────
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void NotifyComponentSelected(string componentId, string displayName);

    [DllImport("__Internal")]
    private static extern void NotifyComponentDeselected();
#endif

    // ── Inspector refs ────────────────────────────────────────────────────
    public ChamuscaAPIClient apiClient;
    public TMP_InputField inputField;
    public Button sendButton;
    public TextMeshProUGUI answerText;
    public Button[] followUpButtons;

    /// Optional label that shows what component is currently focused.
    public TextMeshProUGUI focusLabelText;

    // ── State ─────────────────────────────────────────────────────────────
    public string currentFocusId = "";
    public string currentFocusDisplayName = "";

    // ── Unity lifecycle ───────────────────────────────────────────────────
    void Start()
    {
        sendButton.onClick.AddListener(OnSendClicked);
        foreach (var btn in followUpButtons)
        {
            btn.gameObject.SetActive(false);
            btn.onClick.AddListener(() =>
                OnFollowUpClicked(btn.GetComponentInChildren<TextMeshProUGUI>().text)
            );
        }
    }

    // ── Public API ────────────────────────────────────────────────────────

    /// Called by ChamuscaInteractionManager when the user clicks a component.
    public void SetFocus(string componentId, string displayName)
    {
        currentFocusId = componentId;
        currentFocusDisplayName = displayName;

        // Update the in-scene label (if wired up)
        if (focusLabelText != null)
            focusLabelText.text = string.IsNullOrEmpty(componentId) ? "" : "Focus: " + displayName;
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

    // ── Private helpers ───────────────────────────────────────────────────

    void OnSendClicked()
    {
        if (string.IsNullOrEmpty(inputField.text))
            return;
        string query = inputField.text;
        inputField.text = "";
        SendToBackend(query);
    }

    void OnFollowUpClicked(string query) => SendToBackend(query);

    void SendToBackend(string query)
    {
        answerText.text = "Thinking…";
        sendButton.interactable = false;
        foreach (var btn in followUpButtons)
            btn.gameObject.SetActive(false);

        apiClient.SendQuery(
            query,
            currentFocusId,
            onSuccess: (res) =>
            {
                answerText.text = res.answer;
                sendButton.interactable = true;
                if (res.follow_ups != null)
                {
                    for (int i = 0; i < followUpButtons.Length && i < res.follow_ups.Length; i++)
                    {
                        followUpButtons[i].gameObject.SetActive(true);
                        followUpButtons[i].GetComponentInChildren<TextMeshProUGUI>().text =
                            res.follow_ups[i];
                    }
                }
            },
            onError: (err) =>
            {
                answerText.text = "Error: " + err;
                sendButton.interactable = true;
            },
            displayName: currentFocusDisplayName
        );
    }
}
