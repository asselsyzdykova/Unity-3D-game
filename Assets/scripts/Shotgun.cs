using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float fireRate = 0.8f; 
    public float damage = 25f;    
    public float range = 100f;  
    public int pellets = 5;      

    [Header("Audio Settings")]
    public AudioClip shootSound; 
    private AudioSource audioSource; 

    [Header("References")]
    public Transform firePoint;
    [Tooltip("Layers the beam can hit (must exclude player and weapons)")]
    public LayerMask hittableMask;  

    private float nextTimeToFire = 0f; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot(); 
        }
    }

    void Shoot()
    {
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        for (int i = 0; i < pellets; i++)
        {
            // Raycasting
            RaycastHit hit;
            
            Vector3 shotDirection = firePoint.forward; 
    
            if (Physics.Raycast(firePoint.position, shotDirection, out hit, range, hittableMask)) 
            {
                Debug.DrawRay(firePoint.position, shotDirection * range, Color.yellow, 0.5f);
                
                ZombieHealth zombie = hit.transform.GetComponentInParent<ZombieHealth>();
                
                if (zombie != null)
                {
                    zombie.TakeDamage(damage);
                }

                Debug.Log("Trafil do: " + hit.transform.name);
            }
        }
    }
}
