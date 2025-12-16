using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 500f;
    private float currentHealth;
    
    public bool isAlive = true;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Player health: " + currentHealth);
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;

        currentHealth -= amount;
        Debug.Log("Take damage: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isAlive = false;
        Debug.Log("PLAYER DIE!");
    }
}
