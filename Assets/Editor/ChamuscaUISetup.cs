#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class ChamuscaUISetup : EditorWindow
{
    private static readonly string PathA = @"Y:\OneDrive - Instituto Politécnico de Lisboa\LEIM\3Ano\2Sem\PRJ\CenarioA.mo";
    private static readonly string PathB = @"Y:\OneDrive - Instituto Politécnico de Lisboa\LEIM\3Ano\2Sem\PRJ\CenarioB.mo";

    public class ModelicaVar
    {
        public string Name;
        public string Description;
        public string Type;
        public string Causality; // input or output
    }

    [MenuItem("Chamusca/Generate Responsive HUD")]
    public static void GenerateUI()
    {
        // 1. Read and parse Modelica files to extract interesting telemetry
        List<ModelicaVar> varsA = ParseModelicaFile(PathA);
        List<ModelicaVar> varsB = ParseModelicaFile(PathB);

        Debug.Log($"[UISetup] Parsed {varsA.Count} variables from Scenario A, {varsB.Count} from Scenario B.");

        // 2. Clear old Canvas to prevent duplicates
        GameObject oldCanvas = GameObject.Find("Chamusca_Canvas");
        if (oldCanvas != null)
        {
            DestroyImmediate(oldCanvas);
        }

        // 3. Create Root Canvas
        GameObject canvasGO = new GameObject("Chamusca_Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f; // Responsive scaling for mobile and web
        
        canvasGO.AddComponent<GraphicRaycaster>();

        // 4. Ensure EventSystem exists
        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // 5. Locate Managers Object
        GameObject managerGO = GameObject.Find("Chamusca_Managers");
        if (managerGO == null)
        {
            managerGO = new GameObject("Chamusca_Managers");
        }
        ChamuscaUIManager uiManager = managerGO.GetComponent<ChamuscaUIManager>();
        if (uiManager == null) uiManager = managerGO.AddComponent<ChamuscaUIManager>();

        // 6. Create Top Header Bar (● Ready / Select Component)
        GameObject headerPanel = new GameObject("HeaderPanel");
        headerPanel.transform.SetParent(canvasGO.transform, false);
        RectTransform headerRT = headerPanel.AddComponent<RectTransform>();
        headerRT.anchorMin = new Vector2(0.5f, 1f);
        headerRT.anchorMax = new Vector2(0.5f, 1f);
        headerRT.pivot = new Vector2(0.5f, 1f);
        headerRT.anchoredPosition = new Vector2(0f, -15f);
        headerRT.sizeDelta = new Vector2(450f, 36f);

        Image headerImg = headerPanel.AddComponent<Image>();
        headerImg.color = new Color(0.06f, 0.06f, 0.06f, 0.75f); // Deep glassmorphic tone

        GameObject statusGO = new GameObject("StatusText");
        statusGO.transform.SetParent(headerPanel.transform, false);
        RectTransform statusRT = statusGO.AddComponent<RectTransform>();
        statusRT.anchorMin = new Vector2(0f, 0f);
        statusRT.anchorMax = new Vector2(0.25f, 1f);
        statusRT.offsetMin = new Vector2(15f, 0f);
        statusRT.offsetMax = new Vector2(0f, 0f);
        
        TextMeshProUGUI statusText = statusGO.AddComponent<TextMeshProUGUI>();
        statusText.text = "● Ready";
        statusText.fontSize = 12;
        statusText.fontStyle = FontStyles.Bold;
        statusText.color = new Color(0.2f, 0.85f, 0.4f);
        statusText.alignment = TextAlignmentOptions.MidlineLeft;

        SimulationStatusUI simStatusUI = managerGO.GetComponent<SimulationStatusUI>();
        if (simStatusUI == null) simStatusUI = managerGO.AddComponent<SimulationStatusUI>();
        simStatusUI.statusText = statusText;

        GameObject focusGO = new GameObject("FocusLabel");
        focusGO.transform.SetParent(headerPanel.transform, false);
        RectTransform focusRT = focusGO.AddComponent<RectTransform>();
        focusRT.anchorMin = new Vector2(0.25f, 0f);
        focusRT.anchorMax = new Vector2(1f, 1f);
        focusRT.offsetMin = new Vector2(10f, 0f);
        focusRT.offsetMax = new Vector2(-15f, 0f);

        TextMeshProUGUI focusLabel = focusGO.AddComponent<TextMeshProUGUI>();
        focusLabel.text = "SELECT COMPONENT TO INSPECT";
        focusLabel.fontSize = 11;
        focusLabel.fontStyle = FontStyles.Bold;
        focusLabel.color = new Color(0.8f, 0.8f, 0.8f);
        focusLabel.alignment = TextAlignmentOptions.MidlineLeft;
        uiManager.focusLabelText = focusLabel;

        // 7. Create Mobile Friendly Telemetry Panel (Left Sidebar)
        GameObject debugHudGO = new GameObject("DebugHUD_Panel");
        debugHudGO.transform.SetParent(canvasGO.transform, false);
        RectTransform hudRT = debugHudGO.AddComponent<RectTransform>();
        
        // Anchored to left edge, vertically stretched (highly responsive on mobile/tablet)
        hudRT.anchorMin = new Vector2(0f, 0f);
        hudRT.anchorMax = new Vector2(0f, 1f);
        hudRT.pivot = new Vector2(0f, 0.5f);
        hudRT.anchoredPosition = new Vector2(15f, 0f);
        hudRT.sizeDelta = new Vector2(250f, -60f); // Width 250px, padded top/bottom

        Image hudImg = debugHudGO.AddComponent<Image>();
        hudImg.color = new Color(0.08f, 0.08f, 0.08f, 0.85f); // Translucent Dark gray

        // Title Element
        GameObject titleGO = new GameObject("Title");
        titleGO.transform.SetParent(debugHudGO.transform, false);
        RectTransform titleRT = titleGO.AddComponent<RectTransform>();
        titleRT.anchorMin = new Vector2(0f, 1f);
        titleRT.anchorMax = new Vector2(1f, 1f);
        titleRT.pivot = new Vector2(0.5f, 1f);
        titleRT.anchoredPosition = new Vector2(0f, -15f);
        titleRT.sizeDelta = new Vector2(-20f, 25f);
        
        TextMeshProUGUI titleTxt = titleGO.AddComponent<TextMeshProUGUI>();
        titleTxt.text = "TELEMETRY MONITOR";
        titleTxt.fontSize = 11;
        titleTxt.fontStyle = FontStyles.Bold;
        titleTxt.color = new Color(0.6f, 0.7f, 0.8f);
        titleTxt.alignment = TextAlignmentOptions.Left;

        // Telemetry details text label
        GameObject telemetryGO = new GameObject("TelemetryText");
        telemetryGO.transform.SetParent(debugHudGO.transform, false);
        RectTransform teleRT = telemetryGO.AddComponent<RectTransform>();
        teleRT.anchorMin = new Vector2(0f, 0f);
        teleRT.anchorMax = new Vector2(1f, 1f);
        teleRT.offsetMin = new Vector2(15f, 50f);
        teleRT.offsetMax = new Vector2(-15f, -45f);

        TextMeshProUGUI teleText = telemetryGO.AddComponent<TextMeshProUGUI>();
        teleText.text = "Awaiting connection...";
        teleText.fontSize = 10;
        teleText.lineSpacing = 5f;
        teleText.color = new Color(0.95f, 0.95f, 0.95f);
        teleText.alignment = TextAlignmentOptions.TopLeft;

        // Scenario Selection buttons at the bottom of the telemetry panel
        GameObject scenarioBarGO = new GameObject("ScenarioBar");
        scenarioBarGO.transform.SetParent(debugHudGO.transform, false);
        RectTransform barRT = scenarioBarGO.AddComponent<RectTransform>();
        barRT.anchorMin = new Vector2(0f, 0f);
        barRT.anchorMax = new Vector2(1f, 0f);
        barRT.pivot = new Vector2(0.5f, 0f);
        barRT.anchoredPosition = new Vector2(0f, 15f);
        barRT.sizeDelta = new Vector2(-30f, 26f);

        HorizontalLayoutGroup layoutGroup = scenarioBarGO.AddComponent<HorizontalLayoutGroup>();
        layoutGroup.spacing = 6f;
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = true;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childForceExpandHeight = true;

        Button btnA = CreateHUDButton(scenarioBarGO.transform, "BtnA", "A");
        Button btnB = CreateHUDButton(scenarioBarGO.transform, "BtnB", "B");
        Button btnC = CreateHUDButton(scenarioBarGO.transform, "BtnC", "C");

        // 8. Toggle HUD Panel Button (Bottom Right)
        GameObject debugBtnGO = new GameObject("DebugHUD_ToggleBtn");
        debugBtnGO.transform.SetParent(canvasGO.transform, false);
        RectTransform debugBtnRT = debugBtnGO.AddComponent<RectTransform>();
        debugBtnRT.anchorMin = new Vector2(1f, 0f);
        debugBtnRT.anchorMax = new Vector2(1f, 0f);
        debugBtnRT.pivot = new Vector2(1f, 0f);
        debugBtnRT.anchoredPosition = new Vector2(-20f, 20f);
        debugBtnRT.sizeDelta = new Vector2(40f, 40f);

        Image debugBtnImg = debugBtnGO.AddComponent<Image>();
        debugBtnImg.color = new Color(0.08f, 0.08f, 0.08f, 0.85f);
        Button debugButton = debugBtnGO.AddComponent<Button>();

        GameObject debugBtnTextGO = new GameObject("Text");
        debugBtnTextGO.transform.SetParent(debugBtnGO.transform, false);
        RectTransform debugBtnTxtRT = debugBtnTextGO.AddComponent<RectTransform>();
        debugBtnTxtRT.anchorMin = Vector2.zero;
        debugBtnTxtRT.anchorMax = Vector2.one;
        debugBtnTxtRT.offsetMin = Vector2.zero;
        debugBtnTxtRT.offsetMax = Vector2.zero;
        TextMeshProUGUI debugBtnTxt = debugBtnTextGO.AddComponent<TextMeshProUGUI>();
        debugBtnTxt.text = "HUD";
        debugBtnTxt.fontSize = 11;
        debugBtnTxt.fontStyle = FontStyles.Bold;
        debugBtnTxt.color = new Color(0.85f, 0.85f, 0.85f);
        debugBtnTxt.alignment = TextAlignmentOptions.Center;

        // 9. Attach and configure the FmuDebugController
        Speedma.Debug.FmuDebugController hudController = managerGO.GetComponent<Speedma.Debug.FmuDebugController>();
        if (hudController == null) hudController = managerGO.AddComponent<Speedma.Debug.FmuDebugController>();
        
        hudController.SetCanvasUIReferences(debugHudGO, teleText, debugButton, btnA, btnB, btnC, null);

        Selection.activeGameObject = canvasGO;
        Debug.Log("[UISetup] Responsive RAG UI Canvas successfully generated!");
    }

    private static Button CreateHUDButton(Transform parent, string name, string label)
    {
        GameObject btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent, false);
        
        Image img = btnGO.AddComponent<Image>();
        img.color = new Color(0.15f, 0.15f, 0.15f, 0.9f);
        
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
        txt.fontSize = 9;
        txt.fontStyle = FontStyles.Bold;
        txt.color = new Color(0.9f, 0.9f, 0.9f);
        txt.alignment = TextAlignmentOptions.Center;

        return btn;
    }

    private static List<ModelicaVar> ParseModelicaFile(string path)
    {
        List<ModelicaVar> list = new List<ModelicaVar>();
        if (!File.Exists(path))
        {
            Debug.LogWarning($"[UISetup] Modelica file not found at path: {path}");
            return list;
        }

        try
        {
            string content = File.ReadAllText(path);
            
            // Regex match input declarations: input [Type] [Name] "[Desc]";
            MatchCollection inputMatches = Regex.Matches(content, @"input\s+(Boolean|Real|Integer)\s+(\w+)\s*""([^""]*)""");
            foreach (Match m in inputMatches)
            {
                list.Add(new ModelicaVar {
                    Type = m.Groups[1].Value,
                    Name = m.Groups[2].Value,
                    Description = m.Groups[3].Value,
                    Causality = "input"
                });
            }

            // Regex match output declarations: output [Type] [Name] "[Desc]";
            // Or optional output declarations that might not have custom start params
            MatchCollection outputMatches = Regex.Matches(content, @"output\s+(Boolean|Real|Integer)\s+(\w+)(?:\([^)]*\))?\s*""([^""]*)""");
            foreach (Match m in outputMatches)
            {
                list.Add(new ModelicaVar {
                    Type = m.Groups[1].Value,
                    Name = m.Groups[2].Value,
                    Description = m.Groups[3].Value,
                    Causality = "output"
                });
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[UISetup] Error parsing Modelica file: {ex.Message}");
        }

        return list;
    }
}
#endif
