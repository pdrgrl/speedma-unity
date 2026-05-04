using UnityEngine;

/// <summary>
/// Tests a variable resistor circuit wired to the remote FMU simulation.
/// Mirrors the original TestVariableResistor but talks to the backend
/// instead of loading a native FMU – fully WebGL-safe.
/// </summary>
public class TestVariableResistor : MonoBehaviour
{
    [Header("Simulation backend")]
    public RemoteFmuSimulation sim;

    [Header("FMU name (informational only)")]
    public string fmuName = "ShowVariableResistor";

    [Header("Input")]
    public float inputU = 1.0f;

    [Header("Outputs (read from simulation)")]
    public float voltageVariableResistor;
    public float voltageR2;

    void FixedUpdate()
    {
        if (sim == null || !sim.IsReady) return;

        sim.SetReal("u", inputU);
        sim.Step(Time.fixedDeltaTime);

        voltageVariableResistor = sim.GetReal("VariableResistor.v");
        voltageR2               = sim.GetReal("R2.v");
    }
}
