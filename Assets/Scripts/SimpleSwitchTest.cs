using UnityEngine;
using UnityEngine.UI;
using Speedma;

/// <summary>
/// Tests a simple boolean switch via SpeedmaSimManager.
/// SpeedmaSimManager drives its own step loop — this script only
/// writes inputs and reads outputs each frame.
/// </summary>
public class SimpleSwitchTest : MonoBehaviour
{
    [Header("Simulation backend")]
    public SpeedmaSimManager sim;

    [Header("Input")]
    public bool   switchOn = false;
    public Button activateButton;

    [Header("Output (read from simulation)")]
    public float outputVoltage = 0f;
    public Light lightComponent;
    public float voltageThreshold = 0.5f;

    [Header("Switch Visuals")]
    [Tooltip("Drag Sphere.024_Baked here")]
    public Transform switchHandle;
    public Vector3 rotationOff = new Vector3(0f, 0f,  0f);
    public Vector3 rotationOn  = new Vector3(0f, 0f, 45f);
    public float   rotationSpeed = 5f;

    private Quaternion targetRotation;

    void Start()
    {
        if (activateButton != null)
            activateButton.onClick.AddListener(ToggleSwitch);

        targetRotation = Quaternion.Euler(rotationOff);
        if (switchHandle != null)
            switchHandle.localRotation = targetRotation;
    }

    public void ToggleSwitch()
    {
        switchOn = !switchOn;
        targetRotation = Quaternion.Euler(switchOn ? rotationOn : rotationOff);
    }

    void Update()
    {
        if (switchHandle != null)
            switchHandle.localRotation = Quaternion.Lerp(
                switchHandle.localRotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (sim == null || !sim.IsSessionActive) return;

        sim.SetInput("switchOn", switchOn);
        outputVoltage = sim.GetOutput("outputVoltage");

        if (lightComponent != null)
            lightComponent.enabled = outputVoltage > voltageThreshold;
    }
}
