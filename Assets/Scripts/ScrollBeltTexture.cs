using UnityEngine;

public class ScrollBeltTexture : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * -scrollSpeed;
        // Scrolls the texture along the Y or X axis depending on your UVs
        rend.material.SetTextureOffset("_BaseMap", new Vector2(0, offset));
    }
}
