using UnityEngine;
using UnityEngine.Events;

public class ChamuscaInteractable : MonoBehaviour
{
    [Header("RAG & UI Identity")]
    [Tooltip("The internal ID used by the backend RAG (e.g., crossley_engine)")]
    public string componentId;

    [Tooltip("The human-readable name displayed in the UI")]
    public string displayName;

    [Tooltip("Optional: Custom display name for hover. Falls back to displayName if empty.")]
    public string customHoverName = "";

    [Header("Hover Visuals")]
    [Tooltip("If set, this GameObject's first Renderer (including children) will be used for hover visuals.")]
    public GameObject hoverTargetObject;

    [Tooltip("If set, this specific Renderer will be used for hover visuals. Overrides Hover Target Object.")]
    public Renderer hoverTargetRenderer;

    [Tooltip("Color used when hovered.")]
    public Color hoverColor = Color.yellow;

    [Tooltip("If unchecked, the color of the object and hotspot indicator won't change on hover.")]
    public bool changeColorOnHover = true;

    [Header("Camera Focus Settings")]
    [Tooltip("How far the camera should be when looking at this object.")]
    public float focusDistance = 1.5f;

    [Tooltip("Optional: Where the camera should exactly look. If empty, uses this object's center.")]
    public Transform customLookTarget;

    [Header("On Interact Action")]
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

    private Color originalColor;
    private Renderer rend;
    private bool isHovered = false;

    private GameObject indicatorObj;
    private SpriteRenderer indicatorRenderer;

    void Start()
    {
        // Priority: explicit renderer -> object (find child renderer) -> this object's child renderer
        if (hoverTargetRenderer != null)
        {
            rend = hoverTargetRenderer;
        }
        else if (hoverTargetObject != null)
        {
            rend = hoverTargetObject.GetComponentInChildren<Renderer>();
        }
        else
        {
            rend = GetComponentInChildren<Renderer>();
        }

        if (rend != null)
            originalColor = rend.material.color;

        // Auto-generate the white dot indicator if enabled
        if (showIndicator)
        {
            indicatorObj = new GameObject("HotspotIndicatorDot");
            indicatorObj.transform.SetParent(transform);
            indicatorObj.transform.position = GetFocusPosition() + indicatorOffset;

            indicatorRenderer = indicatorObj.AddComponent<SpriteRenderer>();
            indicatorRenderer.color = Color.white;
            indicatorRenderer.sprite = CreateCircularSprite();
            
            Shader additiveShader = Shader.Find("Legacy Shaders/Particles/Additive");
            if (additiveShader == null)
            {
                additiveShader = Shader.Find("Mobile/Particles/Additive");
            }
            if (additiveShader == null)
            {
                additiveShader = Shader.Find("Sprites/Default");
            }

            Material overlayMaterial = new Material(additiveShader);
            overlayMaterial.SetInt("_ZWrite", 0);
            overlayMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
            overlayMaterial.renderQueue = 3000;
            
            indicatorRenderer.material = overlayMaterial;
        }
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

            if (indicatorObj != null && shouldBeVisible)
            {
                indicatorObj.transform.rotation = camTransform.rotation;

                Vector3 parentScale = transform.lossyScale;
                indicatorObj.transform.localScale = new Vector3(
                    indicatorScaleMultiplier / (parentScale.x > 0.0001f ? parentScale.x : 1f),
                    indicatorScaleMultiplier / (parentScale.y > 0.0001f ? parentScale.y : 1f),
                    indicatorScaleMultiplier / (parentScale.z > 0.0001f ? parentScale.z : 1f)
                );

                Vector3 dirToCam = (camTransform.position - (GetFocusPosition() + indicatorOffset)).normalized;
                indicatorObj.transform.position = GetFocusPosition() + indicatorOffset + dirToCam * forwardOffsetAmount;
            }
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

    public void SetHover(bool hover)
    {
        if (isHovered == hover)
            return;
        isHovered = hover;

        if (changeColorOnHover)
        {
            if (rend != null)
            {
                if (isHovered)
                {
                    rend.material.color = hoverColor;
                }
                else
                {
                    rend.material.color = originalColor;
                }
            }

            if (indicatorRenderer != null)
            {
                indicatorRenderer.color = isHovered ? hoverColor : Color.white;
            }
        }
    }

    public string GetHoverText()
    {
        return !string.IsNullOrEmpty(customHoverName) ? customHoverName : displayName;
    }

    public Vector3 GetFocusPosition()
    {
        return customLookTarget != null ? customLookTarget.position : transform.position;
    }

    public void Interact()
    {
        onInteract.Invoke();
    }
}
