using UnityEngine;

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

    [Header("Output (read from simulation)")]
    public float outputVoltage = 0f;

    void FixedUpdate()
    {
        if (sim == null || !sim.IsReady) return;

        sim.SetBoolean("switchOn", switchOn);
        sim.Step(Time.fixedDeltaTime);
        outputVoltage = sim.GetReal("outputVoltage");
    }
}
