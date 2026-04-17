using UnityEngine;
using UnityEngine.Rendering;

public class TimeShiftController : MonoBehaviour
{
    [Header("Environment Roots")]
    public GameObject museumEnvironment;
    public GameObject historicalEnvironment;

    [Header("Museum Render Settings")]
    public Color museumAmbient = new Color(0.7f, 0.75f, 0.8f);
    public bool museumFog = false;

    [Header("Historical Render Settings")]
    public Color historicalAmbient = new Color(0.15f, 0.12f, 0.08f);
    public bool historicalFog = true;
    public Color historicalFogColor = new Color(0.25f, 0.22f, 0.18f);
    public float historicalFogDensity = 0.04f;

    private bool isHistorical = false;

    void Start()
    {
        // Start in Museum mode by default
        SetEnvironment(false);
    }

    void Update()
    {
        // Press 'M' to toggle between 1920s and 2026
        if (Input.GetKeyDown(KeyCode.M))
        {
            isHistorical = !isHistorical;
            SetEnvironment(isHistorical);
        }
    }

    private void SetEnvironment(bool historical)
    {
        // Toggle GameObjects (Lights, Volumes, Particle Dust, etc.)
        museumEnvironment.SetActive(!historical);
        historicalEnvironment.SetActive(historical);

        // Swap global Render Settings
        if (historical)
        {
            RenderSettings.ambientLight = historicalAmbient;
            RenderSettings.fog = historicalFog;
            RenderSettings.fogColor = historicalFogColor;
            RenderSettings.fogDensity = historicalFogDensity;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
        }
        else
        {
            RenderSettings.ambientLight = museumAmbient;
            RenderSettings.fog = museumFog;
        }
    }
}