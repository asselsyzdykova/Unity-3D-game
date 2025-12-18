using UnityEngine;

public class MuzzleFlashEffect : MonoBehaviour
{
    [Header("Flash Settings")]
    public float flashDuration = 0.08f;
    public float flashIntensity = 15f;
    public Color flashColor = new Color(1f, 0.6f, 0.1f);
    public float lightRange = 15f;

    [Header("Camera Shake")]
    public bool enableCameraShake = true;
    public float shakeIntensity = 0.15f;
    public float shakeDuration = 0.1f;

    private Light flashLight;
    private Camera mainCamera;
    private Vector3 originalCamPos;

    void Start()
    {
        // Создаём яркий свет
        flashLight = gameObject.AddComponent<Light>();
        flashLight.type = LightType.Point;
        flashLight.color = flashColor;
        flashLight.intensity = 0f;
        flashLight.range = lightRange;
        flashLight.shadows = LightShadows.None;

        mainCamera = Camera.main;
    }

    public void PlayFlash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
        
        if (enableCameraShake && mainCamera != null)
        {
            StartCoroutine(CameraShakeRoutine());
        }
    }

    System.Collections.IEnumerator FlashRoutine()
    {
        // Мгновенная яркая вспышка
        flashLight.intensity = flashIntensity;
        flashLight.range = lightRange;
        
        yield return new WaitForSeconds(flashDuration);
        
        // Быстрое затухание
        float elapsed = 0f;
        float fadeDuration = 0.08f;
        
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            flashLight.intensity = Mathf.Lerp(flashIntensity, 0f, t * t); // Квадратичное затухание
            yield return null;
        }
        
        flashLight.intensity = 0f;
    }

    System.Collections.IEnumerator CameraShakeRoutine()
    {
        if (mainCamera == null) yield break;
        
        Transform camTransform = mainCamera.transform;
        Vector3 originalPos = camTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            camTransform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        camTransform.localPosition = originalPos;
    }
}
