// Assets/Scripts/SimpleSwitchTest.cs
using Speedma;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Toggles sw_luz in the ChamuscaDigitalTwin FMU via FmuSceneLink.
/// Animates the switch handle between rotationOff and rotationOn.
/// No longer owns a SpeedmaSimManager — FmuSceneLink owns the session.
/// </summary>
public class SimpleSwitchTest : MonoBehaviour
{
    [Header("Simulation Backend")]
    public FmuSceneLink fmuLink;

    [Header("UI")]
    public Button activateButton;

    [Header("Switch Visuals")]
    [Tooltip("Drag the switch handle Transform here (e.g. Sphere.024_Baked)")]
    public Transform switchHandle;
    public Vector3 rotationOff = new Vector3(0f, 0f, 0f);
    public Vector3 rotationOn = new Vector3(0f, 0f, 45f);
    public float rotationSpeed = 5f;

    private bool _switchOn = false;
    private Quaternion _targetRotation;

    private void Start()
    {
        if (activateButton != null)
            activateButton.onClick.AddListener(ToggleSwitch);

        _targetRotation = Quaternion.Euler(rotationOff);
        if (switchHandle != null)
            switchHandle.localRotation = _targetRotation;
    }

    public void ToggleSwitch()
    {
        _switchOn = !_switchOn;
        _targetRotation = Quaternion.Euler(_switchOn ? rotationOn : rotationOff);
        fmuLink?.SetSwLuz(_switchOn);
    }

    private void Update()
    {
        if (switchHandle == null)
            return;
        switchHandle.localRotation = Quaternion.Lerp(
            switchHandle.localRotation,
            _targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }
}
