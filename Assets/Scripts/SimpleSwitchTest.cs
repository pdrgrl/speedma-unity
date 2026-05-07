using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tests a simple boolean switch wired to the remote FMU simulation.
/// Drop a RemoteFmuSimulation component anywhere in the scene and drag
/// it into the <see cref="sim"/> slot in the Inspector.
/// No FMI2 / native code – fully WebGL-safe.
/// </summary>
public class SimpleSwitchTest : MonoBehaviour
{
    [Header("Simulation backend")]
    public RemoteFmuSimulation sim;

    [Header("Input")]
    public bool switchOn = false;
    public Button activateButton;

    [Header("Output (read from simulation)")]
    public float outputVoltage = 0f;
    public Light lightComponent;
    public float voltageThreshold = 0.5f;

    [Header("Switch Visuals")]
    [Tooltip("Drag Sphere.024_Baked here")]
    public Transform switchHandle;
    public Vector3 rotationOff = new Vector3(0f, 0f, 0f);
    public Vector3 rotationOn = new Vector3(0f, 0f, 45f);
    public float rotationSpeed = 5f; // smooth lerp speed

    private Quaternion targetRotation;

    void Start()
    {
        if (activateButton != null)
            activateButton.onClick.AddListener(ToggleSwitch);

        // Set initial rotation
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
        // Smoothly rotate the handle towards the target
        if (switchHandle != null)
            switchHandle.localRotation = Quaternion.Lerp(
                switchHandle.localRotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
    }

    void FixedUpdate()
    {
        if (sim == null || !sim.IsReady)
            return;

        sim.SetBoolean("switchOn", switchOn);
        sim.Step(Time.fixedDeltaTime);
        outputVoltage = sim.GetReal("outputVoltage");

        if (lightComponent != null)
            lightComponent.enabled = outputVoltage > voltageThreshold;
    }
}
