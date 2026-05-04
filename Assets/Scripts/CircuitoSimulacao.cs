using UnityEngine;

/// <summary>
/// Central simulation coordinator for the Chamusca 1920 scene.
/// Wire <see cref="sim"/> to the RemoteFmuSimulation component in the
/// Inspector.  Add public fields below for every FMU variable your
/// scene needs to read or write.
///
/// This script is the single place that calls sim.Step() – all other
/// scripts should only call Set*/Get* and let this drive the clock.
/// </summary>
public class CircuitoSimulacao : MonoBehaviour
{
    [Header("Simulation backend")]
    public RemoteFmuSimulation sim;

    // ── Add your scene-specific inputs here ───────────────────────
    [Header("Inputs")]
    public bool engineRunning  = false;
    public bool switchMercury  = false;
    public float reostatValue  = 0f;   // 0–1 normalised

    // ── Add your scene-specific outputs here ──────────────────────
    [Header("Outputs (updated each FixedUpdate)")]
    public float batteryVoltage;
    public float dynamoVoltage;
    public float loadCurrent;

    void FixedUpdate()
    {
        if (sim == null || !sim.IsReady) return;

        // — Push inputs —
        sim.SetBoolean("engineRunning", engineRunning);
        sim.SetBoolean("switchMercury", switchMercury);
        sim.SetReal("reostatValue",  reostatValue);

        // — Advance one physics step —
        sim.Step(Time.fixedDeltaTime);

        // — Pull outputs —
        batteryVoltage = sim.GetReal("batteryVoltage");
        dynamoVoltage  = sim.GetReal("dynamoVoltage");
        loadCurrent    = sim.GetReal("loadCurrent");
    }
}
