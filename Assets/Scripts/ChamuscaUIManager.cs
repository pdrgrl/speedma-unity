using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChamuscaUIManager : MonoBehaviour {
    public ChamuscaAPIClient apiClient;
    public TMP_InputField inputField;
    public Button sendButton;
    public TextMeshProUGUI answerText;
    public Button[] followUpButtons;

    public string currentFocusId = "";
    
    // Optional: a UI label to show the user what they selected
    public TextMeshProUGUI focusLabelText; 

    void Start() {
        sendButton.onClick.AddListener(OnSendClicked);
        foreach (var btn in followUpButtons) {
            btn.gameObject.SetActive(false);
            btn.onClick.AddListener(() => OnFollowUpClicked(btn.GetComponentInChildren<TextMeshProUGUI>().text));
        }
    }

    public void SetFocus(string componentId, string displayName) {
        currentFocusId = componentId;
        if (focusLabelText != null) {
            focusLabelText.text = "Current Focus: " + displayName;
        } else {
            Debug.Log("Focus set to: " + displayName);
        }
    }

    void OnSendClicked() {
        if (string.IsNullOrEmpty(inputField.text)) return;
        string query = inputField.text;
        inputField.text = "";
        SendToBackend(query);
    }

    void OnFollowUpClicked(string query) {
        SendToBackend(query);
    }

    void SendToBackend(string query) {
        answerText.text = "Thinking...";
        sendButton.interactable = false;
        foreach (var btn in followUpButtons) btn.gameObject.SetActive(false);

        apiClient.SendQuery(query, currentFocusId,
            onSuccess: (res) => {
                answerText.text = res.answer;
                sendButton.interactable = true;
                if (res.follow_ups != null) {
                    for (int i = 0; i < followUpButtons.Length && i < res.follow_ups.Length; i++) {
                        followUpButtons[i].gameObject.SetActive(true);
                        followUpButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = res.follow_ups[i];
                    }
                }
            },
            onError: (err) => {
                answerText.text = "Error: " + err;
                sendButton.interactable = true;
            }
        );
    }
}
