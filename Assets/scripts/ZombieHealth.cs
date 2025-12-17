using UnityEngine;
using System.Collections;

public class ZombieHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;
    private Animator anim;
    
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>(); 
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        Debug.Log(gameObject.name + "dostal škodu. Zdravie: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(1);
        }
        Debug.Log(gameObject.name + " bol zničený!");

        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.RegisterZombieKill();
        }

        if (anim != null)
        {
            anim.SetTrigger("Die"); 
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        StartCoroutine(DestroyAfterAnimation());

        // Destroy(gameObject); 
    }
    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(3f);
        
        Destroy(gameObject); 
    }
}