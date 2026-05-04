using UnityEngine;
using TMPro;

/// <summary>
/// Optional HUD element that shows the simulation connection state.
/// Attach to any GameObject with a TextMeshProUGUI in the scene.
/// Wire <see cref="sim"/> in the Inspector.
/// </summary>
public class SimulationStatusUI : MonoBehaviour
{
    public RemoteFmuSimulation sim;
    public TextMeshProUGUI     statusText;

    [Header("Labels")]
    public string connectingLabel  = "Connecting to simulation…";
    public string readyLabel       = "Simulation ready";
    public string disconnectedLabel = "Simulation unavailable";

    [Header("Colours")]
    public Color connectingColour   = new Color(1f, 0.8f, 0f);   // amber
    public Color readyColour        = new Color(0.2f, 0.9f, 0.2f); // green
    public Color disconnectedColour = new Color(0.9f, 0.2f, 0.2f); // red

    private bool _wasReady = false;
    private float _timeout = 10f;
    private float _elapsed = 0f;

    void Update()
    {
        if (statusText == null || sim == null) return;

        if (sim.IsReady)
        {
            if (!_wasReady)
            {
                statusText.text  = readyLabel;
                statusText.color = readyColour;
                _wasReady        = true;
            }
            return;
        }

        _elapsed += Time.deltaTime;
        if (_elapsed < _timeout)
        {
            statusText.text  = connectingLabel;
            statusText.color = connectingColour;
        }
        else
        {
            statusText.text  = disconnectedLabel;
            statusText.color = disconnectedColour;
        }
    }
}
