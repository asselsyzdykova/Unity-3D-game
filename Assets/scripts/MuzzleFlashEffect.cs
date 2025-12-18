using UnityEngine;

public class MuzzleFlashEffect : MonoBehaviour
{
    [Header("Flash Settings")]
    public float flashDuration = 0.05f;
    public float flashIntensity = 3f;
    public Color flashColor = new Color(1f, 0.8f, 0.3f); // Оранжево-жёлтый

    [Header("Light")]
    public bool useLight = true;
    
    private Light flashLight;
    private ParticleSystem particles;

    void Awake()
    {
        CreateFlashEffect();
    }

    void CreateFlashEffect()
    {
        // Создаём свет
        if (useLight)
        {
            GameObject lightObj = new GameObject("FlashLight");
            lightObj.transform.SetParent(transform);
            lightObj.transform.localPosition = Vector3.zero;
            
            flashLight = lightObj.AddComponent<Light>();
            flashLight.type = LightType.Point;
            flashLight.color = flashColor;
            flashLight.intensity = 0f;
            flashLight.range = 5f;
        }

        // Создаём систему частиц
        GameObject particleObj = new GameObject("FlashParticles");
        particleObj.transform.SetParent(transform);
        particleObj.transform.localPosition = Vector3.zero;
        particleObj.transform.localRotation = Quaternion.identity;

        particles = particleObj.AddComponent<ParticleSystem>();
        
        // Останавливаем автовоспроизведение
        var main = particles.main;
        main.playOnAwake = false;
        main.duration = 0.1f;
        main.loop = false;
        main.startLifetime = 0.1f;
        main.startSpeed = 15f;
        main.startSize = 0.3f;
        main.startColor = flashColor;
        main.maxParticles = 20;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        // Форма эмиссии - конус вперёд
        var shape = particles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 15f;
        shape.radius = 0.1f;

        // Эмиссия - короткий всплеск
        var emission = particles.emission;
        emission.enabled = true;
        emission.rateOverTime = 0f;
        emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0f, 10)
        });

        // Уменьшение размера со временем
        var sizeOverLifetime = particles.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve();
        sizeCurve.AddKey(0f, 1f);
        sizeCurve.AddKey(1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        // Затухание цвета
        var colorOverLifetime = particles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(flashColor, 0f), 
                new GradientColorKey(flashColor, 0.5f),
                new GradientColorKey(Color.red, 1f)
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(1f, 0f), 
                new GradientAlphaKey(0.5f, 0.5f),
                new GradientAlphaKey(0f, 1f) 
            }
        );
        colorOverLifetime.color = gradient;

        // Рендерер - используем стандартный материал
        var renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        
        // Создаём простой материал
        Material mat = new Material(Shader.Find("Particles/Standard Unlit"));
        mat.SetColor("_Color", flashColor);
        renderer.material = mat;
    }

    public void PlayFlash()
    {
        // Вспышка света
        if (flashLight != null)
        {
            StartCoroutine(FlashLightRoutine());
        }

        // Частицы
        if (particles != null)
        {
            particles.Play();
        }
    }

    System.Collections.IEnumerator FlashLightRoutine()
    {
        flashLight.intensity = flashIntensity;
        yield return new WaitForSeconds(flashDuration);
        
        // Плавное затухание
        float elapsed = 0f;
        float fadeDuration = 0.05f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            flashLight.intensity = Mathf.Lerp(flashIntensity, 0f, elapsed / fadeDuration);
            yield return null;
        }
        flashLight.intensity = 0f;
    }
}
