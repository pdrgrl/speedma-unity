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
        // 1. Destroy old Canvas if exists to prevent duplicates
        GameObject oldCanvas = GameObject.Find("Chamusca_Canvas");
        if (oldCanvas != null)
        {
            DestroyImmediate(oldCanvas);
        }

        // 2. Create Canvas
        GameObject canvasGO = new GameObject("Chamusca_Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGO.AddComponent<GraphicRaycaster>();

        // 3. Create Event System if missing
        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // 4. API Client & UI Manager setup
        GameObject managerGO = GameObject.Find("Chamusca_Managers");
        if (managerGO == null)
        {
            managerGO = new GameObject("Chamusca_Managers");
        }
        ChamuscaAPIClient apiClient = managerGO.GetComponent<ChamuscaAPIClient>();
        if (apiClient == null) apiClient = managerGO.AddComponent<ChamuscaAPIClient>();
        
        ChamuscaUIManager uiManager = managerGO.GetComponent<ChamuscaUIManager>();
        if (uiManager == null) uiManager = managerGO.AddComponent<ChamuscaUIManager>();

        // 5. Create Floating Top Header Panel (Premium minimal pill)
        GameObject headerPanel = new GameObject("HeaderPanel");
        headerPanel.transform.SetParent(canvasGO.transform, false);
        RectTransform headerRT = headerPanel.AddComponent<RectTransform>();
        headerRT.anchorMin = new Vector2(0.5f, 1f);
        headerRT.anchorMax = new Vector2(0.5f, 1f);
        headerRT.pivot = new Vector2(0.5f, 1f);
        headerRT.anchoredPosition = new Vector2(0f, -15f);
        headerRT.sizeDelta = new Vector2(400f, 32f); // Shorter height, narrower width

        Image headerImg = headerPanel.AddComponent<Image>();
        headerImg.color = new Color(0.04f, 0.04f, 0.04f, 0.65f); // Deep dark, semi-transparent

        // 6. Header Text components (Status + Focus Label)
        GameObject statusTextGO = new GameObject("StatusText");
        statusTextGO.transform.SetParent(headerPanel.transform, false);
        RectTransform statusRT = statusTextGO.AddComponent<RectTransform>();
        statusRT.anchorMin = new Vector2(0f, 0f);
        statusRT.anchorMax = new Vector2(0.22f, 1f);
        statusRT.offsetMin = new Vector2(12f, 0f);
        statusRT.offsetMax = new Vector2(0f, 0f);
        
        TextMeshProUGUI statusText = statusTextGO.AddComponent<TextMeshProUGUI>();
        statusText.text = "● Ready";
        statusText.fontSize = 11; // Professional, smaller font
        statusText.fontStyle = FontStyles.Bold;
        statusText.color = new Color(0.35f, 0.95f, 0.5f); // Soft green
        statusText.alignment = TextAlignmentOptions.MidlineLeft;
        
        // Wire up SimulationStatusUI references if present
        SimulationStatusUI simStatusUI = managerGO.GetComponent<SimulationStatusUI>();
        if (simStatusUI == null) simStatusUI = managerGO.AddComponent<SimulationStatusUI>();
        simStatusUI.statusText = statusText;

        GameObject focusLabelGO = new GameObject("FocusLabel");
        focusLabelGO.transform.SetParent(headerPanel.transform, false);
        RectTransform focusRT = focusLabelGO.AddComponent<RectTransform>();
        focusRT.anchorMin = new Vector2(0.22f, 0f);
        focusRT.anchorMax = new Vector2(1f, 1f);
        focusRT.offsetMin = new Vector2(10f, 0f);
        focusRT.offsetMax = new Vector2(-12f, 0f);

        TextMeshProUGUI focusLabel = focusLabelGO.AddComponent<TextMeshProUGUI>();
        focusLabel.text = "SELECT COMPONENT";
        focusLabel.fontSize = 11; // Clean small font
        focusLabel.fontStyle = FontStyles.UpperCase | FontStyles.Normal;
        focusLabel.color = new Color(0.85f, 0.85f, 0.85f, 0.85f); // Muted gray
        focusLabel.alignment = TextAlignmentOptions.MidlineLeft;
        uiManager.focusLabelText = focusLabel;

        // 7. Toggle Debug Panel Button (Bottom Right) - Minimalist transparent square
        GameObject debugBtnGO = new GameObject("DebugHUD_ToggleBtn");
        debugBtnGO.transform.SetParent(canvasGO.transform, false);
        RectTransform debugBtnRT = debugBtnGO.AddComponent<RectTransform>();
        debugBtnRT.anchorMin = new Vector2(1f, 0f);
        debugBtnRT.anchorMax = new Vector2(1f, 0f);
        debugBtnRT.pivot = new Vector2(1f, 0f);
        debugBtnRT.anchoredPosition = new Vector2(-15f, 15f);
        debugBtnRT.sizeDelta = new Vector2(30f, 30f); // Small 30px button

        Image debugBtnImg = debugBtnGO.AddComponent<Image>();
        debugBtnImg.color = new Color(0.04f, 0.04f, 0.04f, 0.7f);
        Button debugButton = debugBtnGO.AddComponent<Button>();

        GameObject debugBtnTextGO = new GameObject("Text");
        debugBtnTextGO.transform.SetParent(debugBtnGO.transform, false);
        RectTransform debugBtnTxtRT = debugBtnTextGO.AddComponent<RectTransform>();
        debugBtnTxtRT.anchorMin = Vector2.zero;
        debugBtnTxtRT.anchorMax = Vector2.one;
        debugBtnTxtRT.offsetMin = Vector2.zero;
        debugBtnTxtRT.offsetMax = Vector2.zero;
        TextMeshProUGUI debugBtnTxt = debugBtnTextGO.AddComponent<TextMeshProUGUI>();
        debugBtnTxt.text = "HUD"; // Text instead of emoji for a scientific look
        debugBtnTxt.fontSize = 9;
        debugBtnTxt.fontStyle = FontStyles.Bold;
        debugBtnTxt.color = new Color(0.7f, 0.7f, 0.7f);
        debugBtnTxt.alignment = TextAlignmentOptions.Center;

        // 8. Debug HUD Overlay Panel (Anchored to Right Edge) - Sleek sidebar
        GameObject debugHudGO = new GameObject("DebugHUD_Panel");
        debugHudGO.transform.SetParent(canvasGO.transform, false);
        RectTransform hudRT = debugHudGO.AddComponent<RectTransform>();
        hudRT.anchorMin = new Vector2(1f, 0.5f);
        hudRT.anchorMax = new Vector2(1f, 0.5f);
        hudRT.pivot = new Vector2(1f, 0.5f);
        hudRT.anchoredPosition = new Vector2(-15f, 0f);
        hudRT.sizeDelta = new Vector2(200f, 260f); // Compacted HUD

        Image hudImg = debugHudGO.AddComponent<Image>();
        hudImg.color = new Color(0.04f, 0.04f, 0.04f, 0.8f); // Translucent Dark sidebar

        // Title for Debug Panel
        GameObject titleGO = new GameObject("Title");
        titleGO.transform.SetParent(debugHudGO.transform, false);
        RectTransform titleRT = titleGO.AddComponent<RectTransform>();
        titleRT.anchorMin = new Vector2(0f, 1f);
        titleRT.anchorMax = new Vector2(1f, 1f);
        titleRT.pivot = new Vector2(0.5f, 1f);
        titleRT.anchoredPosition = new Vector2(0f, -10f);
        titleRT.sizeDelta = new Vector2(-20f, 20f);
        
        TextMeshProUGUI titleTxt = titleGO.AddComponent<TextMeshProUGUI>();
        titleTxt.text = "TELEMETRY";
        titleTxt.fontSize = 10;
        titleTxt.fontStyle = FontStyles.Bold | FontStyles.UpperCase;
        titleTxt.color = new Color(0.5f, 0.5f, 0.5f); // Slate grey title
        titleTxt.alignment = TextAlignmentOptions.Left;

        // Telemetry details text label (formatted strings)
        GameObject telemetryGO = new GameObject("TelemetryText");
        telemetryGO.transform.SetParent(debugHudGO.transform, false);
        RectTransform teleRT = telemetryGO.AddComponent<RectTransform>();
        teleRT.anchorMin = new Vector2(0f, 0f);
        teleRT.anchorMax = new Vector2(1f, 1f);
        teleRT.offsetMin = new Vector2(10f, 40f); // Lifted bottom slightly to fit scenario buttons
        teleRT.offsetMax = new Vector2(-10f, -30f);

        TextMeshProUGUI teleText = telemetryGO.AddComponent<TextMeshProUGUI>();
        teleText.text = "CONNECTING...";
        teleText.fontSize = 9; // High density, small, crisp text layout
        teleText.lineSpacing = 4f;
        teleText.color = new Color(0.9f, 0.9f, 0.9f);
        teleText.alignment = TextAlignmentOptions.TopLeft;

        // Create Scenario Selection Bar at the bottom of the HUD panel
        GameObject scenarioBarGO = new GameObject("ScenarioSelectionBar");
        scenarioBarGO.transform.SetParent(debugHudGO.transform, false);
        RectTransform barRT = scenarioBarGO.AddComponent<RectTransform>();
        barRT.anchorMin = new Vector2(0f, 0f);
        barRT.anchorMax = new Vector2(1f, 0f);
        barRT.pivot = new Vector2(0.5f, 0f);
        barRT.anchoredPosition = new Vector2(0f, 10f);
        barRT.sizeDelta = new Vector2(-20f, 22f); // Height of 22px

        HorizontalLayoutGroup layoutGroup = scenarioBarGO.AddComponent<HorizontalLayoutGroup>();
        layoutGroup.spacing = 8f;
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = true;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childForceExpandHeight = true;

        Button btnA = CreateHUDButton(scenarioBarGO.transform, "BtnA", "SCENARIO A");
        Button btnB = CreateHUDButton(scenarioBarGO.transform, "BtnB", "SCENARIO B");
        Button btnC = CreateHUDButton(scenarioBarGO.transform, "BtnC", "SCENARIO C");

        // Add a modern Canvas-based Debug HUD controller to run telemetry updates
        Speedma.Debug.FmuDebugController hudController = managerGO.GetComponent<Speedma.Debug.FmuDebugController>();
        if (hudController == null) hudController = managerGO.AddComponent<Speedma.Debug.FmuDebugController>();
        hudController.SetCanvasUIReferences(debugHudGO, teleText, debugButton, btnA, btnB, btnC);

        Selection.activeGameObject = canvasGO;
        Debug.Log("Chamusca Minimal RAG UI created successfully!");
    }

    private static Button CreateHUDButton(Transform parent, string name, string label)
    {
        GameObject btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent, false);
        
        Image img = btnGO.AddComponent<Image>();
        img.color = new Color(0.12f, 0.12f, 0.12f, 0.85f);
        
        Button btn = btnGO.AddComponent<Button>();
        
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(btnGO.transform, false);
        RectTransform textRT = textGO.AddComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;
        
        TextMeshProUGUI txt = textGO.AddComponent<TextMeshProUGUI>();
        txt.text = label;
        txt.fontSize = 8;
        txt.fontStyle = FontStyles.Bold;
        txt.color = new Color(0.8f, 0.8f, 0.8f);
        txt.alignment = TextAlignmentOptions.Center;

        return btn;
    }
}
#endif
