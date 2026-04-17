using UnityEngine;

public class InspectionCamera : MonoBehaviour
{
    [Header("Target & Movement")]
    public Vector3 targetPosition;
    public float targetDistance = 3.0f;
    public float rotationSpeed = 5.0f;
    public float transitionSpeed = 5.0f;
    public float zoomSpeed = 2.0f;

    [Header("Limits & Collision")]
    public float minDistance = 0.5f;
    public float maxDistance = 10.0f;
    public LayerMask wallCollisionLayer; // Set this to your 'Environment' layer
    public float cameraCollisionRadius = 0.2f; // Prevents clipping through walls

    private float currentX = 0.0f;
    private float currentY = 20.0f; // Default slight downward angle
    
    private Vector3 currentTargetPos;
    private float currentDistance;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
        currentTargetPos = targetPosition;
        currentDistance = targetDistance;
    }

    void Update()
    {
        HandleInput();
    }

    void LateUpdate()
    {
        // Smoothly move the focus point and distance
        currentTargetPos = Vector3.Lerp(currentTargetPos, targetPosition, Time.deltaTime * transitionSpeed);
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * transitionSpeed);

        // Calculate the desired rotation and un-collided position
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = currentTargetPos - (rotation * Vector3.forward * currentDistance);

        // --- CAMERA COLLISION LOGIC ---
        // Cast a thick ray (SphereCast) from the target to the desired camera position.
        // If it hits a wall, move the camera in closer so it doesn't clip.
        Vector3 directionToCamera = desiredPosition - currentTargetPos;
        float maxRayDistance = directionToCamera.magnitude;
        
        if (Physics.SphereCast(currentTargetPos, cameraCollisionRadius, directionToCamera.normalized, out RaycastHit hit, maxRayDistance, wallCollisionLayer))
        {
            // If we hit a wall, place the camera right in front of the wall
            desiredPosition = currentTargetPos + directionToCamera.normalized * (hit.distance - cameraCollisionRadius);
        }

        // Apply position and rotation
        transform.position = desiredPosition;
        transform.rotation = rotation;
    }

    private void HandleInput()
    {
        // 1. Drag to Rotate (Right Click or Left Click Drag)
        if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            
            // Clamp vertical rotation so you don't flip upside down
            currentY = Mathf.Clamp(currentY, -80f, 80f);
        }

        // 2. Mouse Scroll to Zoom (Optional manual override)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetDistance -= scroll * zoomSpeed;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }

        // 3. Click to Focus on Hotspots
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                InteractableHotspot hotspot = hit.collider.GetComponent<InteractableHotspot>();
                if (hotspot != null)
                {
                    // Move the camera's target to the hotspot
                    targetPosition = hotspot.GetFocusPosition();
                    
                    // Set the precise distance defined by the editor for this specific switch
                    targetDistance = hotspot.focusDistance;
                    
                    // Trigger the interaction (animations, sounds, etc.)
                    hotspot.Interact();
                }
            }
        }
    }
}