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

    void Die()
    {
        isAlive = false;
        Debug.Log("PLAYER DIE!");
        uiManager.ShowDeathScreen();
    }
}
