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
    public LayerMask wallCollisionLayer;
    public float cameraCollisionRadius = 0.2f;

    /// <summary>
    /// When true, all orbit/zoom input is suppressed.
    /// Set by RheostatWheel (or any other interactable) while grabbed.
    /// </summary>
    public bool IsLocked { get; set; } = false;

    private float currentX = 0.0f;
    private float currentY = 20.0f;
    private Vector3 currentTargetPos;
    private float currentDistance;

    private Vector3 mouseDownPos;
    private bool isDragging = false;
    private const float dragThreshold = 5f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;

        targetPosition = transform.position + transform.forward * targetDistance;
        currentTargetPos = targetPosition;
        currentDistance = targetDistance;
    }

    void Update() => HandleInput();

    void LateUpdate()
    {
        currentTargetPos = Vector3.Lerp(
            currentTargetPos,
            targetPosition,
            Time.deltaTime * transitionSpeed
        );
        currentDistance = Mathf.Lerp(
            currentDistance,
            targetDistance,
            Time.deltaTime * transitionSpeed
        );

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = currentTargetPos - (rotation * Vector3.forward * currentDistance);

        Vector3 directionToCamera = desiredPosition - currentTargetPos;
        float maxRayDistance = directionToCamera.magnitude;

        if (
            Physics.SphereCast(
                currentTargetPos,
                cameraCollisionRadius,
                directionToCamera.normalized,
                out RaycastHit hit,
                maxRayDistance,
                wallCollisionLayer
            )
        )
        {
            desiredPosition =
                currentTargetPos
                + directionToCamera.normalized * (hit.distance - cameraCollisionRadius);
        }

        transform.position = desiredPosition;
        transform.rotation = rotation;
    }

    private void HandleInput()
    {
        // ── All orbit/zoom input blocked while something is grabbed ────
        if (IsLocked)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
            isDragging = false;
        }

        if (Input.GetMouseButton(1))
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentY = Mathf.Clamp(currentY, -80f, 80f);
        }
        else if (Input.GetMouseButton(0))
        {
            if (!isDragging && Vector3.Distance(mouseDownPos, Input.mousePosition) > dragThreshold)
                isDragging = true;

            if (isDragging)
            {
                currentX += Input.GetAxis("Mouse X") * rotationSpeed;
                currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
                currentY = Mathf.Clamp(currentY, -80f, 80f);
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetDistance -= scroll * zoomSpeed;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }

        if (Input.GetMouseButtonUp(0) && !isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                InteractableHotspot hotspot =
                    hit.collider.GetComponentInParent<InteractableHotspot>();
                if (hotspot != null)
                {
                    targetPosition = hotspot.GetFocusPosition();
                    targetDistance = hotspot.focusDistance;
                    hotspot.Interact();
                }
            }
        }
    }
}
