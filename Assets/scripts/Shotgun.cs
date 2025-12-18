using UnityEngine;
using System.Collections;
using TMPro;

public class Shotgun : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float fireRate = 0.8f; 
    public float damage = 25f;    
    public float range = 100f;  
    public int pellets = 5;      
    public float spread = 0.2f;

    [Header("Ammo Settings")]
    public int currentAmmo = 5;     
    public int magSize = 5;        
    public int totalAmmo = 50;   

    [Header("Audio Settings")]
    public AudioClip shootSound; 
    private AudioSource audioSource; 
    public AudioClip reloadSound;
    public AudioClip emptySound;

    [Header("UI Settings")]
    public TextMeshProUGUI ammoText;

    [Header("References")]
    public Transform firePoint;
    [Tooltip("Layers the beam can hit (must exclude player and weapons)")]
    public LayerMask hittableMask;

    public Animator playerAnim;  
    public float reloadDuration = 2.0f; 
    public GameObject impactEffectPrefab;
    public GameObject muzzleFlashPrefab;
    public Crosshair crosshair;
    public MuzzleFlashEffect muzzleFlashEffect;

    private float nextTimeToFire = 0f; 
    private bool isReloading = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        UpdateAmmoUI();
    }

    void Update()
    {
        if (isReloading) return;

        if (GameUIManager.isPaused) return;
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot(); 
            }
            else
            {
                PlayEmptySound();
                nextTimeToFire = Time.time + fireRate; 
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < magSize && totalAmmo > 0)
        {
            TryReload();
        }
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text ="Ammo: " + currentAmmo + " [" + totalAmmo + "]";
        }
    }

    void TryReload()
    {
        if (playerAnim == null) return;

        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("stand"))
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;

        if (playerAnim != null)
        {
            playerAnim.SetBool("isReloading", true);
        }

        if (reloadSound != null && audioSource != null)
        {
        audioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadDuration);

        int ammoNeeded = magSize - currentAmmo; 
        int ammoToSet = Mathf.Min(ammoNeeded, totalAmmo);

        currentAmmo += ammoToSet;
        totalAmmo -= ammoToSet;

        UpdateAmmoUI();

        if (playerAnim != null)
        {
            playerAnim.SetBool("isReloading", false);
        }

        isReloading = false;
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI();

        // Эффект отдачи прицела
        if (crosshair != null)
        {
            crosshair.OnShoot();
        }

        // Эффект вспышки выстрела
        if (muzzleFlashEffect != null)
        {
            muzzleFlashEffect.PlayFlash();
        }
        else if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            flash.transform.parent = firePoint; 
            Destroy(flash, 0.1f);
        }

        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        for (int i = 0; i < pellets; i++)
        {
            RaycastHit hit;
            
            Vector3 spreadVector = new Vector3(
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                Random.Range(-spread, spread)
            );
            Vector3 shotDirection = (firePoint.forward + spreadVector).normalized;
    
            if (Physics.Raycast(firePoint.position, shotDirection, out hit, range, hittableMask)) 
            {
                Debug.DrawRay(firePoint.position, shotDirection * range, Color.yellow, 0.5f);

                if (impactEffectPrefab != null)
                {
                    GameObject impact = Instantiate(impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 1f);
                }
                
                ZombieHealth zombie = hit.transform.GetComponentInParent<ZombieHealth>();
                
                if (zombie != null)
                {
                    zombie.TakeDamage(damage);
                }

                Debug.Log("Trafil do: " + hit.transform.name);
            }
        }
    }
    public void AddAmmo(int amount)
    {
        totalAmmo += amount;
        UpdateAmmoUI();

        PlayerMovement movement = GetComponentInParent<PlayerMovement>();
        if (movement != null)
        {
            movement.LogEvent($"PICKUP: Ammo Pack (+{amount} bullets). Total Ammo: {totalAmmo}");
        }
        Debug.Log("Pobrane naboje: " + amount + ". Razem: " + totalAmmo);
    }
    void PlayEmptySound()
    {
        if (emptySound != null)
        {
            audioSource.PlayOneShot(emptySound);
        }
        Debug.Log("Ammo empty!");
    }
}
