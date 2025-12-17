using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 500f;
    private float currentHealth;
    public bool isAlive = true;

    [Header("UI Reference")]
    public Slider healthSlider;
    public GameUIManager uiManager;
    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Player health: " + currentHealth);
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;

        currentHealth -= amount;
        Debug.Log("Take damage: " + currentHealth);
        
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public bool Heal(float amount)
    {
        if (!isAlive || currentHealth >= maxHealth) 
        {
            return false; 
        }

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); 
        
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.LogEvent($"PICKUP: Health Pack (+{amount} HP). Current Health: {currentHealth}/{maxHealth}");
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        Debug.Log("Healed! Health: " + currentHealth);
        return true; 
    }

    void Die()
    {
        isAlive = false;
        Debug.Log("PLAYER DIE!");
        uiManager.ShowDeathScreen();
    }
}
