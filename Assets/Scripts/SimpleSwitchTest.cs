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
    [Tooltip("Toggle in Play Mode to switch the circuit on/off.")]
    public bool switchOn = false;
    public Button activateButton;

    [Header("Output (read from simulation)")]
    public float outputVoltage = 0f;
    public Light lightComponent;
    public float voltageThreshold = 0.5f; // Turn light on when voltage exceeds this

    void Start()
    {
        // Wire the button to toggle the switch
        if (activateButton != null)
        {
            activateButton.onClick.AddListener(ToggleSwitch);
        }
    }

    void ToggleSwitch()
    {
        switchOn = !switchOn;
    }

    void FixedUpdate()
    {
        if (sim == null || !sim.IsReady)
            return;

        sim.SetBoolean("switchOn", switchOn);
        sim.Step(Time.fixedDeltaTime);
        outputVoltage = sim.GetReal("outputVoltage");

        // Turn light on/off based on voltage
        if (lightComponent != null)
        {
            lightComponent.enabled = outputVoltage > voltageThreshold;
        }
    }
}
