using UnityEngine;
using Chamusca.Simulation;

/// <summary>
/// Scales the Animator playback speed of the Crossley engine flywheel or Dynamo rotor
/// dynamically based on the engine's current RPMs from the simulation.
/// </summary>
public class EngineRpmAnimator : MonoBehaviour
{
    [Header("Simulation Link")]
    public ChamuscaSimController controller;

    [Header("Animation Parameters")]
    [Tooltip("The nominal engine RPM corresponding to 1.0x animation playback speed.")]
    public float nominalRPM = 350f;

    [Tooltip("Multiplier for the base animation speed.")]
    public float baseAnimationSpeed = 1.0f;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }

        if (controller == null)
        {
            controller = Object.FindFirstObjectByType<ChamuscaSimController>();
        }

        if (_animator == null)
        {
            Debug.LogWarning($"[EngineRpmAnimator] No Animator found on GameObject '{name}' or its children!");
        }
    }

    private void Update()
    {
        if (_animator == null || controller == null)
            return;

        // Fetch engine RPM from the sim controller state directly
        float rpm = controller.engine_rpm;

        // Calculate and set animator speed
        float targetSpeed = (rpm / nominalRPM) * baseAnimationSpeed;
        _animator.speed = Mathf.Max(0f, targetSpeed);
    }
}
