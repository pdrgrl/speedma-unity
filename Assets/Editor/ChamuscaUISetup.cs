#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChamuscaUISetup : EditorWindow
{
    [MenuItem("Chamusca/Generate RAG UI")]
    public static void GenerateUI()
    {
        // 1. Create Canvas
        GameObject canvasGO = new GameObject("Chamusca_Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler
            .ScaleMode
            .ScaleWithScreenSize;
        canvasGO.AddComponent<GraphicRaycaster>();

        // 2. Create Event System
        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // 3. API Client & UI Manager
        GameObject managerGO = new GameObject("Chamusca_Managers");
        ChamuscaAPIClient apiClient = managerGO.AddComponent<ChamuscaAPIClient>();
        ChamuscaUIManager uiManager = managerGO.AddComponent<ChamuscaUIManager>();
        uiManager.apiClient = apiClient;

        // 4. Panel Background
        GameObject panel = new GameObject("BackgroundPanel");
        panel.transform.SetParent(canvasGO.transform, false);
        RectTransform panelRT = panel.AddComponent<RectTransform>();
        panelRT.anchorMin = new Vector2(0.1f, 0.1f);
        panelRT.anchorMax = new Vector2(0.9f, 0.9f);
        panelRT.offsetMin = Vector2.zero;
        panelRT.offsetMax = Vector2.zero;
        Image panelImg = panel.AddComponent<Image>();
        panelImg.color = new Color(0.15f, 0.15f, 0.15f, 0.9f);

        // 5. Answer Text Block (TMP)
        GameObject answerGO = new GameObject("AnswerText");
        answerGO.transform.SetParent(panel.transform, false);
        RectTransform answerRT = answerGO.AddComponent<RectTransform>();
        answerRT.anchorMin = new Vector2(0.05f, 0.3f);
        answerRT.anchorMax = new Vector2(0.95f, 0.95f);
        answerRT.offsetMin = Vector2.zero;
        answerRT.offsetMax = Vector2.zero;
        TextMeshProUGUI answerText = answerGO.AddComponent<TextMeshProUGUI>();
        answerText.text = "Hello! I am the Chamusca 1920 Docent. Ask me anything...";
        answerText.fontSize = 22;
        answerText.color = Color.white;
        answerText.alignment = TextAlignmentOptions.TopLeft;
        uiManager.answerText = answerText;

        // 6. Input Field Background & Field
        GameObject inputGO = new GameObject("InputField");
        inputGO.transform.SetParent(panel.transform, false);
        RectTransform inputRT = inputGO.AddComponent<RectTransform>();
        inputRT.anchorMin = new Vector2(0.05f, 0.15f);
        inputRT.anchorMax = new Vector2(0.8f, 0.25f);
        inputRT.offsetMin = Vector2.zero;
        inputRT.offsetMax = Vector2.zero;
        Image inputImg = inputGO.AddComponent<Image>();
        inputImg.color = Color.white;

        // Input Field Text Area
        GameObject textAreaGO = new GameObject("Text Area");
        textAreaGO.transform.SetParent(inputGO.transform, false);
        RectTransform textAreaRT = textAreaGO.AddComponent<RectTransform>();
        textAreaRT.anchorMin = Vector2.zero;
        textAreaRT.anchorMax = Vector2.one;
        textAreaRT.offsetMin = new Vector2(10, 10);
        textAreaRT.offsetMax = new Vector2(-10, -10);
        textAreaGO.AddComponent<RectMask2D>();

        // Input Field Text Object
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(textAreaGO.transform, false);
        RectTransform textRT = textGO.AddComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;
        TextMeshProUGUI textTMP = textGO.AddComponent<TextMeshProUGUI>();
        textTMP.color = Color.black;
        textTMP.fontSize = 20;

        TMP_InputField inputField = inputGO.AddComponent<TMP_InputField>();
        inputField.textViewport = textAreaRT;
        inputField.textComponent = textTMP;
        uiManager.inputField = inputField;

        // 7. Send Button
        GameObject sendBtnGO = new GameObject("SendButton");
        sendBtnGO.transform.SetParent(panel.transform, false);
        RectTransform sendBtnRT = sendBtnGO.AddComponent<RectTransform>();
        sendBtnRT.anchorMin = new Vector2(0.82f, 0.15f);
        sendBtnRT.anchorMax = new Vector2(0.95f, 0.25f);
        sendBtnRT.offsetMin = Vector2.zero;
        sendBtnRT.offsetMax = Vector2.zero;
        Image sendBtnImg = sendBtnGO.AddComponent<Image>();
        sendBtnImg.color = new Color(0.2f, 0.6f, 0.2f);
        Button sendButton = sendBtnGO.AddComponent<Button>();

        GameObject sendTxtGO = new GameObject("Text");
        sendTxtGO.transform.SetParent(sendBtnGO.transform, false);
        RectTransform sendTxtRT = sendTxtGO.AddComponent<RectTransform>();
        sendTxtRT.anchorMin = Vector2.zero;
        sendTxtRT.anchorMax = Vector2.one;
        sendTxtRT.offsetMin = Vector2.zero;
        sendTxtRT.offsetMax = Vector2.zero;
        TextMeshProUGUI sendTxtTMP = sendTxtGO.AddComponent<TextMeshProUGUI>();
        sendTxtTMP.text = "Send";
        sendTxtTMP.color = Color.white;
        sendTxtTMP.alignment = TextAlignmentOptions.Center;
        uiManager.sendButton = sendButton;

        // 8. Follow-up Buttons Layout
        GameObject followUpGO = new GameObject("FollowUps");
        followUpGO.transform.SetParent(panel.transform, false);
        RectTransform followUpRT = followUpGO.AddComponent<RectTransform>();
        followUpRT.anchorMin = new Vector2(0.05f, 0.02f);
        followUpRT.anchorMax = new Vector2(0.95f, 0.12f);
        followUpRT.offsetMin = Vector2.zero;
        followUpRT.offsetMax = Vector2.zero;
        HorizontalLayoutGroup hlg = followUpGO.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = 15;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;

        // Instantiate 3 Follow-up buttons
        uiManager.followUpButtons = new Button[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject fuBtnGO = new GameObject("FollowUpBtn_" + i);
            fuBtnGO.transform.SetParent(followUpGO.transform, false);
            Image fuBtnImg = fuBtnGO.AddComponent<Image>();
            fuBtnImg.color = new Color(0.2f, 0.4f, 0.7f);
            Button fuBtn = fuBtnGO.AddComponent<Button>();

            GameObject fuTxtGO = new GameObject("Text");
            fuTxtGO.transform.SetParent(fuBtnGO.transform, false);
            RectTransform fuTxtRT = fuTxtGO.AddComponent<RectTransform>();
            fuTxtRT.anchorMin = Vector2.zero;
            fuTxtRT.anchorMax = Vector2.one;
            fuTxtRT.offsetMin = new Vector2(5, 5);
            fuTxtRT.offsetMax = new Vector2(-5, -5);
            TextMeshProUGUI fuTxtTMP = fuTxtGO.AddComponent<TextMeshProUGUI>();
            fuTxtTMP.text = "Suggestion " + (i + 1);
            fuTxtTMP.color = Color.white;
            fuTxtTMP.alignment = TextAlignmentOptions.Center;
            fuTxtTMP.enableAutoSizing = true;
            fuTxtTMP.fontSizeMin = 12;
            fuTxtTMP.fontSizeMax = 20;

            uiManager.followUpButtons[i] = fuBtn;
        }

        Selection.activeGameObject = canvasGO;
        Debug.Log("Chamusca RAG UI Editor Script executed successfully!");
    }
}
#endif
