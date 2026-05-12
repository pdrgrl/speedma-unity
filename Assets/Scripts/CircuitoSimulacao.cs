using UnityEngine;
using Speedma;

/// <summary>
/// Central simulation coordinator for the Chamusca 1920 scene.
/// This is the ONLY script that calls sim.Step() — all others
/// only call SetInput/GetOutput via FmuSceneLink.
/// </summary>
public class CircuitoSimulacao : MonoBehaviour
{
    [Header("Simulation backend")]
    public RemoteFmuSimulation sim;

    [Header("Step size (s) — keep ≤ 0.0005 for this RC circuit")]
    public float simDt = 0.001f;

    // ── Inputs (set by FmuDebugController / RheostatInput / ReducerInput) ──
    // These are pushed to the FMU every step via FmuSceneLink.
    // Do NOT set them here — let the input controllers write to FmuSceneLink.

    // ── Outputs (read from FMU, available to any script) ──────────────────
    [Header("Outputs (updated each step)")]
    public float i_c_out;   // Condensador current (A)
    public float v_c_out;   // Condensador voltage (V)

    void FixedUpdate()
    {
        if (sim == null || !sim.IsReady) return;

        // ── Advance simulation ───────────────────────────────
        // Inputs are already buffered in RemoteFmuSimulation by
        // FmuSceneLink.SetSwitches / SetRCarga / SetVFonte.
        sim.Step(simDt);

        // ── Pull outputs (for debug Inspector visibility) ───
        i_c_out = sim.GetReal("i_c_out");
        v_c_out = sim.GetReal("v_c_out");
    }
}
