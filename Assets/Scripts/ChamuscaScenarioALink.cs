using UnityEngine;
using Chamusca.Simulation;
using Speedma;

/// <summary>
/// Main coordinator for Cenario A (Night Discharge).
/// Gathers interactive visual inputs, sends them to the FMU client, 
/// and maps the simulation outputs back to the dials, meters, lights, and circuit breaker.
/// </summary>
public class ChamuscaScenarioALink : MonoBehaviour
{
    [Header("Simulation Manager")]
    public SpeedmaSimManager simManager;

    [Header("Meters")]
    public VoltmeterPointer voltmeter;
    public AmpController batteryAmmeter;
    public AmpController dynamoAmmeter;

    [Header("Selector Plug")]
    public VoltmeterSelectorFuse voltmeterSelector;

    [Header("Switches & Levers")]
    public FmuToggleSwitch swCasaLuz;
    public FmuToggleSwitch swDepLuz;
    public FmuToggleSwitch swQuadroLuz;
    public FmuToggleSwitch swBatLuz;
    public ReductorDuploController reductorController;

    [Header("Light Controllers")]
    public HouseLightController houseLights;
    public HouseLightController depLights;
    public HouseLightController panelLights;
    public HouseLightController batteryRoomLights;

    private bool _resetActive = false;
    private float _resetTimer = 0f;

    private void Update()
    {
        if (simManager == null || !simManager.IsSessionActive)
            return;

        // 1. Gather inputs and write them to the FMU
        SendInputsToSim();

        // 2. Fetch outputs from the FMU and update visual indicators
        UpdateVisualsFromSim();

        // 3. Handle protection reset timer (momentary pulse)
        if (_resetActive)
        {
            _resetTimer -= Time.deltaTime;
            if (_resetTimer <= 0f)
            {
                _resetActive = false;
                simManager.SetInput("resetProtection", false);
                Debug.Log("[ScenarioALink] Reset protection input pulse finished (resetProtection=false)");
            }
        }
    }

    private void SendInputsToSim()
    {
        // Knife switches
        if (swCasaLuz != null) simManager.SetInput("sw_casa_luz", swCasaLuz.switchOn);
        if (swDepLuz != null) simManager.SetInput("sw_dep_luz", swDepLuz.switchOn);
        
        // Nipple board/battery switches
        if (swQuadroLuz != null) simManager.SetInput("sw_quadro_luz", swQuadroLuz.switchOn);
        if (swBatLuz != null) simManager.SetInput("sw_bat_luz", swBatLuz.switchOn);

        // Double-reductor cell index (discharge arm)
        // Reductor pos is 41-60. FMU expects 1-20 (reductor_pos = cell - 40)
        if (reductorController != null)
        {
            int fmuPos = Mathf.Clamp(reductorController.dischargeCell - 40, 1, 20);
            simManager.SetInput("reductor_pos", fmuPos);
        }
    }

    private void UpdateVisualsFromSim()
    {
        // Update Ammeters
        float iBat = simManager.GetOutput("i_bat");
        if (batteryAmmeter != null)
        {
            // Ammeter dial is positive 0-12A. Map absolute value of battery flow.
            batteryAmmeter.SetValue(Mathf.Abs(iBat));
        }

        float iDin = simManager.GetOutput("i_din");
        if (dynamoAmmeter != null)
        {
            dynamoAmmeter.SetValue(iDin); // Should sit at 0A in Scenario A
        }

        // Update Voltmeter depending on selector plug position
        if (voltmeter != null && voltmeterSelector != null)
        {
            float targetVoltage = 0f;
            switch (voltmeterSelector.selectedSlot)
            {
                case 0: // Descarga: displays battery voltage (discharge branch)
                    targetVoltage = simManager.GetOutput("batteryVoltage");
                    break;
                case 1: // Carga: displays battery voltage (charge branch)
                    targetVoltage = simManager.GetOutput("batteryVoltageCarga");
                    break;
                case 2: // Dínamo: displays dynamo voltage (0V in Scenario A)
                    targetVoltage = 0f; 
                    break;
            }
            voltmeter.SetValue(targetVoltage);
        }

        // Update lighting intensites (Fmu returns 0.0 - 1.0)
        if (houseLights != null)
            houseLights.UpdateFromIntensity(simManager.GetOutput("houseLightIntensity"));
        if (depLights != null)
            depLights.UpdateFromIntensity(simManager.GetOutput("depLightIntensity"));
        if (panelLights != null)
            panelLights.UpdateFromIntensity(simManager.GetOutput("panelLightIntensity"));
        if (batteryRoomLights != null)
            batteryRoomLights.UpdateFromIntensity(simManager.GetOutput("batteryRoomLightIntensity"));
    }

    /// <summary>
    /// Rearm the circuit breaker and blown fuses by pulsing resetProtection to true in the FMU.
    /// Called by clicking the tripped B. Dijunctor breaker handle.
    /// </summary>
    public void RequestProtectionReset()
    {
        if (simManager == null || !simManager.IsSessionActive) return;

        _resetActive = true;
        _resetTimer = 0.5f; // Maintain high pulse for 0.5 seconds
        simManager.SetInput("resetProtection", true);
        Debug.Log("[ScenarioALink] Protection reset requested (resetProtection=true)");
    }
}
