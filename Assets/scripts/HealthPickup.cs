using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Health Settings")]
    public float healAmount = 50f;

    [Header("Visual Effects")]
    public float rotationSpeed = 90f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.3f;

    [Header("Audio")]
    public AudioClip pickupSound;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Вращение
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Покачивание вверх-вниз
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.isAlive)
            {
                // Пытаемся вылечить и запоминаем результат (успех или нет)
                bool success = playerHealth.Heal(healAmount);

                if (success)
                {
                    Debug.Log("The heart has been selected! +" + healAmount + " HP");

                    if (pickupSound != null)
                    {
                        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                    }

                    Destroy(gameObject); // Удаляем ТОЛЬКО если полечились
                }
                else 
                {
                    Debug.Log("Health is full! Heart remains on the ground.");
                }
            }
        }
    }
}
