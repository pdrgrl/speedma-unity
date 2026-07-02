using UnityEngine;
using UnityEngine.Events;

public class InteractableHotspot : MonoBehaviour
{
    [Header("Camera Focus Settings")]
    [Tooltip("How far the camera should be when looking at this object.")]
    public float focusDistance = 1.5f;

    [Tooltip(
        "Optional: Where the camera should exactly look. If empty, uses this object's center."
    )]
    public Transform customLookTarget;

    [Header("Interaction")]
    [Tooltip("What happens when this is clicked? (e.g., trigger an animation, play a sound)")]
    public UnityEvent onInteract;

    [Header("Visual Indicator (Auto-generated)")]
    [Tooltip("Create a billboard white dot indicator at startup?")]
    public bool showIndicator = true;
    public float indicatorScaleMultiplier = 0.01f; // Constant physical scale multiplier
    public Vector3 indicatorOffset = Vector3.zero;
    [Tooltip("Distance to offset the dot towards the camera to prevent clipping behind the mesh.")]
    public float forwardOffsetAmount = 0.05f;

    // Static toggle to show/hide all hotspot indicators globally
    public static bool GlobalIndicatorsEnabled = true;

    private GameObject indicatorObj;

    void Start()
    {
        if (showIndicator)
        {
            // Create a GameObject for the sprite indicator
            indicatorObj = new GameObject("HotspotIndicatorDot");
            indicatorObj.transform.SetParent(transform);
            indicatorObj.transform.position = GetFocusPosition() + indicatorOffset;

            SpriteRenderer sr = indicatorObj.AddComponent<SpriteRenderer>();
            sr.color = Color.white;
            sr.sprite = CreateCircularSprite();
            
            // Use the standard Sprite shader but configure it to ignore depth testing
            Material overlayMaterial = new Material(Shader.Find("Sprites/Default"));
            
            // Configure the material to draw on top of everything
            overlayMaterial.SetInt("_ZWrite", 0); // Disable depth writing
            overlayMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always); // Always pass depth test
            overlayMaterial.renderQueue = 3000; // Transparent Queue (above geometry)
            
            sr.material = overlayMaterial;
        }
    }

    private Sprite CreateCircularSprite()
    {
        int size = 32;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color[] colors = new Color[size * size];
        float center = size / 2.0f;
        float radius = size / 2.0f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dx = x - center + 0.5f;
                float dy = y - center + 0.5f;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);

                if (distance <= radius)
                {
                    // Smooth edges slightly (anti-aliasing)
                    float alpha = Mathf.Clamp01(radius - distance);
                    colors[y * size + x] = new Color(1, 1, 1, alpha);
                }
                else
                {
                    colors[y * size + x] = Color.clear;
                }
            }
        }

        texture.SetPixels(colors);
        texture.filterMode = FilterMode.Bilinear;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 32);
    }

    void LateUpdate()
    {
        // Toggle indicators globally with 'H' key (Hide/Show)
        if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.hKey.wasPressedThisFrame)
        {
            GlobalIndicatorsEnabled = !GlobalIndicatorsEnabled;
        }

        bool shouldBeVisible = showIndicator && GlobalIndicatorsEnabled;

        if (indicatorObj != null)
        {
            if (indicatorObj.activeSelf != shouldBeVisible)
            {
                indicatorObj.SetActive(shouldBeVisible);
            }
        }

        if (Camera.main != null)
        {
            Transform camTransform = Camera.main.transform;

            // 1. Handle Indicator Billboard & Offset
            if (indicatorObj != null && shouldBeVisible)
            {
                // Make the sprite look directly at the main camera
                indicatorObj.transform.rotation = camTransform.rotation;

                // Calculate local scale that compensates for parent's global scale
                Vector3 parentScale = transform.lossyScale;
                indicatorObj.transform.localScale = new Vector3(
                    indicatorScaleMultiplier / (parentScale.x > 0.0001f ? parentScale.x : 1f),
                    indicatorScaleMultiplier / (parentScale.y > 0.0001f ? parentScale.y : 1f),
                    indicatorScaleMultiplier / (parentScale.z > 0.0001f ? parentScale.z : 1f)
                );

                // Dynamically shift the indicator slightly closer to the camera along the view vector
                Vector3 dirToCam = (camTransform.position - (GetFocusPosition() + indicatorOffset)).normalized;
                indicatorObj.transform.position = GetFocusPosition() + indicatorOffset + dirToCam * forwardOffsetAmount;
            }
        }
    }

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
