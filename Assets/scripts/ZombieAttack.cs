using UnityEngine;
public class ZombieAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float damageAmount = 10f;
    public float attackRate = 1f;

    [Header("Debug")]
    public bool playerInZone = false; 

    private PlayerHealth targetHealth;     
    private float nextAttackTime = 0f;    
    
    private const string PlayerTag = "Player"; 

    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (anim != null)
        {
            anim.SetBool("isAttacking", playerInZone);
        }   
        if (playerInZone && targetHealth != null && Time.time >= nextAttackTime)
        {
            AttackPlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            targetHealth = other.GetComponent<PlayerHealth>();
            
            if (targetHealth != null)
            {
                playerInZone = true;
                Debug.Log("The player entered the attack zone. The zombie waits 5 seconds....");
                
                nextAttackTime = Time.time + attackRate; 
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            playerInZone = false;
            targetHealth = null;
            Debug.Log("The player has left the attack zone.");
        }
    }
    void AttackPlayer()
    {
        targetHealth.TakeDamage(damageAmount);
        
        nextAttackTime = Time.time + attackRate;
        
        Debug.Log("A zombie has attacked!");
    }
}
