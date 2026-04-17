using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.IO;

public class DigitalTwinEnvironmentSetup : EditorWindow
{
    [MenuItem("Tools/Digital Twin/Generate Environments")]
    public static void GenerateEnvironments()
    {
        GameObject root = new GameObject("DigitalTwin_Environments");
        TimeShiftController controller = root.AddComponent<TimeShiftController>();

        // Create folders for Post-Processing profiles
        if (!AssetDatabase.IsValidFolder("Assets/Settings"))
            AssetDatabase.CreateFolder("Assets", "Settings");

        // ==========================================
        // 1. MUSEUM ENVIRONMENT (2026 - Faraday)
        // ==========================================
        GameObject museumRoot = new GameObject("Museum_2026");
        museumRoot.transform.SetParent(root.transform);
        controller.museumEnvironment = museumRoot;

        // Museum Lighting
        Light museumDir = CreateLight("Museum_CeilingFill", museumRoot.transform, LightType.Directional, new Color(0.95f, 0.98f, 1.0f), 1.0f);
        museumDir.transform.rotation = Quaternion.Euler(60, -30, 0);
        museumDir.shadows = LightShadows.Soft;
        museumDir.shadowStrength = 0.5f;

        // Museum Post-Processing Volume
        Volume museumVolume = CreateVolume("MuseumVolume", museumRoot, true);
        VolumeProfile museumProfile = ScriptableObject.CreateInstance<VolumeProfile>();
        
        // Clean, bright post-processing
        Tonemapping mTone = museumProfile.Add<Tonemapping>(true);
        mTone.mode.Override(TonemappingMode.ACES);
        
        Bloom mBloom = museumProfile.Add<Bloom>(true);
        mBloom.intensity.Override(0.2f);
        mBloom.threshold.Override(1.0f);

        AssetDatabase.CreateAsset(museumProfile, "Assets/Settings/MuseumProfile.asset");
        museumVolume.profile = museumProfile;

        // ==========================================
        // 2. HISTORICAL ENVIRONMENT (1920s - Chamusca)
        // ==========================================
        GameObject historicalRoot = new GameObject("Historical_1920s");
        historicalRoot.transform.SetParent(root.transform);
        controller.historicalEnvironment = historicalRoot;

        // Historical Lighting (Warm, dim sunlight + early 110V DC incandescent bulbs)
        Light histSun = CreateLight("1920s_SunShaft", historicalRoot.transform, LightType.Directional, new Color(1.0f, 0.85f, 0.6f), 1.5f);
        histSun.transform.rotation = Quaternion.Euler(25, 45, 0); // Low sun angle for dramatic shadows
        histSun.shadows = LightShadows.Soft;

        // Simulate a 110V DC incandescent bulb near the Tudor batteries [file:97]
        Light histBulb = CreateLight("110V_Incandescent", historicalRoot.transform, LightType.Point, new Color(1.0f, 0.65f, 0.3f), 2.5f);
        histBulb.transform.position = new Vector3(0, 2.5f, 0);
        histBulb.range = 10f;
        histBulb.shadows = LightShadows.Soft;

        // Historical Post-Processing Volume
        Volume historicalVolume = CreateVolume("HistoricalVolume", historicalRoot, true);
        VolumeProfile histProfile = ScriptableObject.CreateInstance<VolumeProfile>();

        Tonemapping hTone = histProfile.Add<Tonemapping>(true);
        hTone.mode.Override(TonemappingMode.ACES);

        ColorAdjustments hColor = histProfile.Add<ColorAdjustments>(true);
        hColor.colorFilter.Override(new Color(1.0f, 0.9f, 0.8f)); // Warm, nostalgic tint
        hColor.contrast.Override(15f);

        Bloom hBloom = histProfile.Add<Bloom>(true);
        hBloom.intensity.Override(1.5f); // High bloom to simulate dusty light scattering
        hBloom.scatter.Override(0.7f);

        Vignette hVignette = histProfile.Add<Vignette>(true);
        hVignette.intensity.Override(0.45f);
        hVignette.smoothness.Override(0.4f);

        FilmGrain hGrain = histProfile.Add<FilmGrain>(true);
        hGrain.type.Override(FilmGrainLookup.Medium1);
        hGrain.intensity.Override(0.6f);

        AssetDatabase.CreateAsset(histProfile, "Assets/Settings/HistoricalProfile.asset");
        historicalVolume.profile = histProfile;

        // Save and refresh
        AssetDatabase.SaveAssets();
        Selection.activeGameObject = root;
        Debug.Log("Digital Twin Environments Generated Successfully!");
    }

    private static Light CreateLight(string name, Transform parent, LightType type, Color color, float intensity)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent);
        Light l = go.AddComponent<Light>();
        l.type = type;
        l.color = color;
        l.intensity = intensity;
        return l;
    }

    private static Volume CreateVolume(string name, GameObject parent, bool isGlobal)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform);
        Volume v = go.AddComponent<Volume>();
        v.isGlobal = isGlobal;
        return v;
    }
}