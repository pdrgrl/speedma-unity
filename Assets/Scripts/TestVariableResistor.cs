using UnityEngine;
using Speedma;

/// <summary>
/// Tests a variable resistor circuit via SpeedmaSimManager.
/// SpeedmaSimManager drives its own step loop — this script only
/// writes inputs and reads outputs each frame.
/// </summary>
public class TestVariableResistor : MonoBehaviour
{
    [Header("Simulation backend")]
    public SpeedmaSimManager sim;

    [Header("Input")]
    public float inputU = 1.0f;

    [Header("Outputs (read from simulation)")]
    public float voltageVariableResistor;
    public float voltageR2;

    void Update()
    {
        if (sim == null || !sim.IsSessionActive) return;

        sim.SetInput("u", inputU);

        voltageVariableResistor = sim.GetOutput("VariableResistor.v");
        voltageR2               = sim.GetOutput("R2.v");
    }
}
