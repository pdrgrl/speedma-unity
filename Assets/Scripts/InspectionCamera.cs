using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InspectionCamera : MonoBehaviour
{
    [Header("Target & Movement")]
    public Vector3 targetPosition;
    public float targetDistance = 3.0f;
    public float rotationSpeed = 0.05f; // Decreased sensitivity for smoother orbiting
    public float transitionSpeed = 5.0f;
    public float zoomSpeed = 2.0f; // Restored to a higher zoom speed
    public float zoomStepAmount = 1.5f; // Larger zoom step amount for click/tap

    [Header("Limits & Collision")]
    public float minDistance = 0.5f;
    public float maxDistance = 10.0f;
    public LayerMask wallCollisionLayer;
    public float cameraCollisionRadius = 0.2f;

    [Header("Rotation Constraints")]
    [Tooltip("Maximum degrees the camera can rotate horizontally to the left or right from its starting angle.")]
    public float maxHorizontalAngle = 60f;
    public float minVerticalAngle = -60f; // Adjusted vertical constraints to be more balanced
    public float maxVerticalAngle = 60f;

    /// <summary>
    /// When true, all orbit/zoom input is suppressed.
    /// Set by RheostatWheel (or any other interactable) while grabbed.
    /// </summary>
    public bool IsLocked { get; set; } = false;

    private float startX = 0.0f;
    private float startY = 0.0f;
    private float currentX = 0.0f;
    private float currentY = 20.0f;
    private Vector3 startPosition;
    private float startDistance;
    private Vector3 currentTargetPos;
    private float currentDistance;

    private Vector2 mouseDownPos;
    private bool isDragging = false;
    private const float dragThreshold = 5f;
    private int _maxTouchCountThisGesture = 0;
    private float _prevTouchDistance = 0f;
    private Vector2 _lastTouchPosition;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        // Normalize starting y angle (yaw) and x angle (pitch)
        startX = angles.y;
        startY = angles.x;
        currentX = startX;
        currentY = startY;

        startDistance = targetDistance;
        targetPosition = transform.position + transform.forward * targetDistance;
        startPosition = targetPosition;
        
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
        if (IsLocked)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        // --- Touch Support (Mobile/Web Friendly) ---
        if (Touchscreen.current != null)
        {
            var activeTouches = Touchscreen.current.touches;
            int currentTouchCount = 0;
            UnityEngine.InputSystem.Controls.TouchControl firstTouch = null;
            UnityEngine.InputSystem.Controls.TouchControl secondTouch = null;

            foreach (var touch in activeTouches)
            {
                if (touch.isInProgress)
                {
                    currentTouchCount++;
                    if (firstTouch == null) firstTouch = touch;
                    else if (secondTouch == null) secondTouch = touch;

                    _lastTouchPosition = touch.position.ReadValue();
                }
            }

            // Track the maximum number of touches detected during this gesture
            if (currentTouchCount > _maxTouchCountThisGesture)
            {
                _maxTouchCountThisGesture = currentTouchCount;
            }

            if (currentTouchCount == 0)
            {
                // All fingers lifted: Evaluate tap interactions
                if (_maxTouchCountThisGesture == 1 && !isDragging)
                {
                    if (!CheckHotspotInteraction(_lastTouchPosition))
                    {
                        Zoom(zoomStepAmount); // One finger tap = Zoom In
                    }
                }
                else if (_maxTouchCountThisGesture >= 2)
                {
                    Zoom(-zoomStepAmount); // Two finger tap = Zoom Out
                }

                // Reset gesture state
                _maxTouchCountThisGesture = 0;
                isDragging = false;
            }
            else if (currentTouchCount == 1 && firstTouch != null)
            {
                if (firstTouch.press.wasPressedThisFrame)
                {
                    mouseDownPos = firstTouch.position.ReadValue();
                    isDragging = false;
                }
                else if (firstTouch.press.isPressed)
                {
                    Vector2 delta = firstTouch.delta.ReadValue();
                    if (!isDragging && Vector2.Distance(mouseDownPos, firstTouch.position.ReadValue()) > dragThreshold)
                    {
                        isDragging = true;
                    }

                    // Only allow rotation if it isn't part of a multi-finger pinch gesture
                    if (isDragging && _maxTouchCountThisGesture < 2)
                    {
                        currentX += delta.x * rotationSpeed;
                        currentY -= delta.y * rotationSpeed;
                        ClampAngles();
                    }
                }
            }
            else if (currentTouchCount == 2 && firstTouch != null && secondTouch != null)
            {
                // Smooth pinch-to-zoom calculation
                Vector2 pos1 = firstTouch.position.ReadValue();
                Vector2 pos2 = secondTouch.position.ReadValue();
                float currentTouchDistance = Vector2.Distance(pos1, pos2);

                if (firstTouch.press.wasPressedThisFrame || secondTouch.press.wasPressedThisFrame)
                {
                    _prevTouchDistance = currentTouchDistance;
                }
                else
                {
                    float deltaDist = currentTouchDistance - _prevTouchDistance;
                    if (Mathf.Abs(deltaDist) > 0.1f)
                    {
                        targetDistance -= deltaDist * 0.005f * zoomSpeed;
                        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
                        _prevTouchDistance = currentTouchDistance;
                    }
                }
            }

            // If a touch gesture is in progress or just finishing, bypass mouse input checks to avoid duplication
            if (currentTouchCount > 0 || _maxTouchCountThisGesture > 0)
            {
                return;
            }
        }

        // --- Mouse & Keyboard ---
        if (Mouse.current != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 delta = Mouse.current.delta.ReadValue();

            if (Mouse.current.rightButton.isPressed)
            {
                currentX += delta.x * rotationSpeed;
                currentY -= delta.y * rotationSpeed;
                ClampAngles();
            }
            else if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                mouseDownPos = mousePosition;
                isDragging = false;
            }
            else if (Mouse.current.leftButton.isPressed)
            {
                if (!isDragging && Vector2.Distance(mouseDownPos, mousePosition) > dragThreshold)
                {
                    isDragging = true;
                }

                if (isDragging)
                {
                    currentX += delta.x * rotationSpeed;
                    currentY -= delta.y * rotationSpeed;
                    ClampAngles();
                }
            }

            // Scroll Wheel Zoom
            Vector2 scrollDelta = Mouse.current.scroll.ReadValue();
            if (Mathf.Abs(scrollDelta.y) > 0.01f)
            {
                targetDistance -= scrollDelta.y * 0.002f * zoomSpeed;
                targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
            }

            // Tap Zoom
            bool bothReleasedOrPressed = (Mouse.current.leftButton.wasReleasedThisFrame && Mouse.current.rightButton.isPressed) ||
                                         (Mouse.current.rightButton.wasReleasedThisFrame && Mouse.current.leftButton.isPressed);

            if (bothReleasedOrPressed)
            {
                Zoom(-zoomStepAmount); // Simulated 2-finger zoom out
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame && !isDragging)
            {
                if (!CheckHotspotInteraction(mousePosition))
                {
                    Zoom(zoomStepAmount);
                }
            }
            else if (Mouse.current.rightButton.wasReleasedThisFrame && delta.magnitude < 1.0f)
            {
                Zoom(-zoomStepAmount);
            }
        }
    }

    private bool CheckHotspotInteraction(Vector2 screenPos)
    {
        if (Camera.main == null) return false;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ChamuscaInteractable interactable = hit.collider.GetComponentInParent<ChamuscaInteractable>();
            if (interactable != null)
            {
                targetPosition = interactable.GetFocusPosition();
                targetDistance = interactable.focusDistance;
                interactable.Interact();
                return true;
            }
        }
        return false;
    }

    private void Zoom(float amount)
    {
        targetDistance -= amount;
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        
        // If we zoomed all the way out, reset focus target and rotation back to the starting "spawn" point
        if (targetDistance >= maxDistance - 0.01f)
        {
            targetPosition = startPosition;
            currentX = startX;
            currentY = startY;
        }
    }

    private void ClampAngles()
    {
        // Clamp Y (Vertical pitch)
        currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);

        // Clamp X (Horizontal yaw) around starting X position to limit rotation to front and sides
        float diff = Mathf.DeltaAngle(startX, currentX);
        diff = Mathf.Clamp(diff, -maxHorizontalAngle, maxHorizontalAngle);
        currentX = startX + diff;
    }
}


