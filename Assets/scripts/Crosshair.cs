using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [Header("Crosshair Settings")]
    public Image crosshairImage;
    public float normalSize = 50f;
    public float shootSize = 70f;
    public float smoothSpeed = 10f;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color enemyColor = Color.red;

    [Header("Detection")]
    public float detectionRange = 100f;
    public LayerMask enemyMask;
    public Transform firePoint;

    private float currentSize;
    private RectTransform rectTransform;

    void Start()
    {
        if (crosshairImage != null)
        {
            rectTransform = crosshairImage.GetComponent<RectTransform>();
            currentSize = normalSize;
        }
    }

    void Update()
    {
        if (crosshairImage == null || rectTransform == null) return;

        // Плавное изменение размера
        currentSize = Mathf.Lerp(currentSize, normalSize, smoothSpeed * Time.deltaTime);
        rectTransform.sizeDelta = new Vector2(currentSize, currentSize);

        // Проверка наведения на врага
        CheckEnemyAim();
    }

    void CheckEnemyAim()
    {
        if (firePoint == null)
        {
            // Используем центр экрана если firePoint не задан
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            if (Physics.Raycast(ray, out RaycastHit hit, detectionRange, enemyMask))
            {
                crosshairImage.color = enemyColor;
            }
            else
            {
                crosshairImage.color = normalColor;
            }
        }
        else
        {
            if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, detectionRange, enemyMask))
            {
                crosshairImage.color = enemyColor;
            }
            else
            {
                crosshairImage.color = normalColor;
            }
        }
    }

    // Вызывать при выстреле для эффекта отдачи прицела
    public void OnShoot()
    {
        currentSize = shootSize;
    }
}
