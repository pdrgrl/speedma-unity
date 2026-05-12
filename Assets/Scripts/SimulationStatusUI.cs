using UnityEngine;
using TMPro;
using Speedma;

/// <summary>
/// Optional HUD element that shows the simulation connection state.
/// Wire <see cref="sim"/> (SpeedmaSimManager) in the Inspector.
/// </summary>
public class SimulationStatusUI : MonoBehaviour
{
    public SpeedmaSimManager sim;
    public TextMeshProUGUI   statusText;

    [Header("Labels")]
    public string connectingLabel   = "Connecting to simulation…";
    public string readyLabel        = "Simulation ready";
    public string disconnectedLabel = "Simulation unavailable";

    [Header("Colours")]
    public Color connectingColour   = new Color(1f, 0.8f, 0f);
    public Color readyColour        = new Color(0.2f, 0.9f, 0.2f);
    public Color disconnectedColour = new Color(0.9f, 0.2f, 0.2f);

    private bool  _wasReady = false;
    private float _elapsed  = 0f;
    private float _timeout  = 10f;

    void Update()
    {
        if (statusText == null || sim == null) return;

        if (sim.IsSessionActive)
        {
            if (!_wasReady)
            {
                statusText.text  = readyLabel;
                statusText.color = readyColour;
                _wasReady = true;
            }
            return;
        }

        _elapsed += Time.deltaTime;
        statusText.text  = _elapsed < _timeout ? connectingLabel   : disconnectedLabel;
        statusText.color = _elapsed < _timeout ? connectingColour  : disconnectedColour;
    }
}
