using UnityEngine;

public class HitMarker : MonoBehaviour
{
    [Header("Settings")]
    public float lifetime = 0.5f;
    public float startSize = 0.05f;
    public float endSize = 0.02f;
    public Color hitColor = Color.red;

    private float timer;
    private SpriteRenderer spriteRenderer;
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
        
        // Создаём спрайт-точку
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateCircleSprite();
        spriteRenderer.color = hitColor;
        spriteRenderer.sortingOrder = 100;
        
        transform.localScale = Vector3.one * startSize;
        timer = lifetime;
    }

    void Update()
    {
        // Всегда смотрит на камеру
        transform.LookAt(mainCamera);
        
        timer -= Time.deltaTime;
        
        // Уменьшаем размер и прозрачность
        float t = 1f - (timer / lifetime);
        float size = Mathf.Lerp(startSize, endSize, t);
        transform.localScale = Vector3.one * size;
        
        Color c = spriteRenderer.color;
        c.a = Mathf.Lerp(1f, 0f, t);
        spriteRenderer.color = c;
        
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    Sprite CreateCircleSprite()
    {
        int resolution = 32;
        Texture2D texture = new Texture2D(resolution, resolution);
        
        Vector2 center = new Vector2(resolution / 2f, resolution / 2f);
        float radius = resolution / 2f - 1;
        
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                if (dist < radius)
                {
                    texture.SetPixel(x, y, Color.white);
                }
                else
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
        }
        
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, resolution, resolution), new Vector2(0.5f, 0.5f), resolution);
    }
}
