using UnityEngine;
using UnityEngine.Events;

public class InteractableHotspot : MonoBehaviour
{
    [Header("Camera Focus Settings")]
    [Tooltip("How far the camera should be when looking at this object.")]
    public float focusDistance = 1.5f;
    
    [Tooltip("Optional: Where the camera should exactly look. If empty, uses this object's center.")]
    public Transform customLookTarget;

    [Header("Interaction")]
    [Tooltip("What happens when this is clicked? (e.g., trigger an animation, play a sound)")]
    public UnityEvent onInteract;

    // A simple visual indicator could be a child SpriteRenderer or Particle System
    // that we toggle on/off when the player is near.

    public Vector3 GetFocusPosition()
    {
        return customLookTarget != null ? customLookTarget.position : transform.position;
    }

    public void Interact()
    {
        // This triggers any logic you link in the Unity Inspector 
        // (like opening a drawer or flipping a switch)
        onInteract.Invoke();
    }
}