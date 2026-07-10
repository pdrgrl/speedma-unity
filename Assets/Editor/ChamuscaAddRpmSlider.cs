#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Speedma.Debug;

public class ChamuscaAddRpmSlider : EditorWindow
{
    [MenuItem("Chamusca/Add RPM Slider to HUD")]
    public static void CreateRpmSlider()
    {
        // 1. Locate the Debug HUD Panel
        GameObject hudPanel = GameObject.Find("DebugHUD_Panel");
        if (hudPanel == null)
        {
            Debug.LogError("[HUD Setup] Could not find 'DebugHUD_Panel' GameObject in the active scene!");
            return;
        }

        FmuDebugController debugController = Object.FindFirstObjectByType<FmuDebugController>();
        if (debugController == null)
        {
            Debug.LogError("[HUD Setup] Could not find FmuDebugController in the scene!");
            return;
        }

        // 2. Remove old slider if it exists to allow clean re-builds
        Transform oldSlider = hudPanel.transform.Find("RPMSlider_Container");
        if (oldSlider != null)
        {
            DestroyImmediate(oldSlider.gameObject);
            Debug.Log("[HUD Setup] Removed legacy RPM Slider container.");
        }

        // 3. Create Container
        GameObject container = new GameObject("RPMSlider_Container", typeof(RectTransform));
        container.transform.SetParent(hudPanel.transform, false);
        
        RectTransform containerRt = container.GetComponent<RectTransform>();
        containerRt.anchorMin = new Vector2(0f, 0f);
        containerRt.anchorMax = new Vector2(1f, 0f);
        containerRt.pivot = new Vector2(0.5f, 0f);
        containerRt.anchoredPosition = new Vector3(0f, 60f, 0f); // Position it above the scenario buttons
        containerRt.sizeDelta = new Vector2(-30f, 40f); // Leave padding on sides matching other layout elements

        // 4. Create Label text (Styled like other telemetry headers)
        GameObject labelGo = new GameObject("RPMSlider_Label", typeof(RectTransform), typeof(TMPro.TextMeshProUGUI));
        labelGo.transform.SetParent(container.transform, false);
        
        RectTransform labelRt = labelGo.GetComponent<RectTransform>();
        labelRt.anchorMin = new Vector2(0f, 1f);
        labelRt.anchorMax = new Vector2(1f, 1f);
        labelRt.pivot = new Vector2(0.5f, 1f);
        labelRt.anchoredPosition = new Vector3(0f, 0f, 0f);
        labelRt.sizeDelta = new Vector2(0f, 18f);

        TMPro.TextMeshProUGUI labelText = labelGo.GetComponent<TMPro.TextMeshProUGUI>();
        labelText.text = "ENGINE / MOTOR SPEED (RPM)";
        labelText.fontSize = 10f;
        labelText.fontStyle = TMPro.FontStyles.Bold;
        labelText.color = new Color(0.6f, 0.7f, 0.8f); // Blue-grey header color
        labelText.alignment = TMPro.TextAlignmentOptions.Left;

        // 5. Create Default UI Slider
        DefaultControls.Resources uiResources = new DefaultControls.Resources();
        GameObject sliderGo = DefaultControls.CreateSlider(uiResources);
        sliderGo.name = "RPMSlider";
        sliderGo.transform.SetParent(container.transform, false);

        RectTransform sliderRt = sliderGo.GetComponent<RectTransform>();
        sliderRt.anchorMin = new Vector2(0f, 0f);
        sliderRt.anchorMax = new Vector2(1f, 0f);
        sliderRt.pivot = new Vector2(0.5f, 0f);
        sliderRt.anchoredPosition = new Vector3(0f, 0f, 0f);
        sliderRt.sizeDelta = new Vector2(0f, 18f);

        // Apply dark glassmorphic styling colors to the Slider elements
        Image bgImg = sliderGo.GetComponent<Image>();
        if (bgImg != null) bgImg.color = new Color(0.15f, 0.15f, 0.15f, 0.9f); // Dark background

        Transform fillArea = sliderGo.transform.Find("Fill Area");
        if (fillArea != null)
        {
            Transform fill = fillArea.Find("Fill");
            if (fill != null)
            {
                Image fillImg = fill.GetComponent<Image>();
                if (fillImg != null) fillImg.color = new Color(0.2f, 0.6f, 0.9f, 0.85f); // Sleek electric blue
            }
        }

        Transform handleArea = sliderGo.transform.Find("Handle Slide Area");
        if (handleArea != null)
        {
            Transform handle = handleArea.Find("Handle");
            if (handle != null)
            {
                Image handleImg = handle.GetComponent<Image>();
                if (handleImg != null) handleImg.color = new Color(0.7f, 0.7f, 0.7f, 1f); // Matte handle
            }
        }

        // Configure Slider range
        Slider slider = sliderGo.GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 500f;
        slider.value = 350f;

        // Wire references in FmuDebugController
        SerializedObject so = new SerializedObject(debugController);
        SerializedProperty sp = so.FindProperty("rpmSlider");
        if (sp != null)
        {
            sp.objectReferenceValue = slider;
            so.ApplyModifiedProperties();
            Debug.Log("[HUD Setup] Wired rpmSlider to FmuDebugController field.");
        }

        EditorUtility.SetDirty(debugController.gameObject);
        EditorUtility.SetDirty(hudPanel);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(hudPanel.scene);

        Debug.Log("[HUD Setup] RPM Slider styled and configured successfully!");
    }
}
#endif
