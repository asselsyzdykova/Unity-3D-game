using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject healthPrefab;
    public int maxHealthPickups = 3;
    public float spawnRadius = 30f;
    public float checkDelay = 5f;
    public float minDistanceFromPlayer = 5f;

    [Header("Spawn Height")]
    public float spawnHeight = 1f;

    private Transform player;
    private List<GameObject> healthPickups = new List<GameObject>();

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        InvokeRepeating(nameof(CheckHealthPickups), 0f, checkDelay);
    }

    void CheckHealthPickups()
    {   
        if (player != null)
        {
            for (int i = healthPickups.Count - 1; i >= 0; i--)
            {
                if (healthPickups[i] != null)
                {
                    float distance = Vector3.Distance(player.position, healthPickups[i].transform.position);
                    
                    if (distance > spawnRadius + 10f)
                    {
                        Destroy(healthPickups[i]); 
                    }
                }
            }
        }

        healthPickups.RemoveAll(h => h == null);

        int need = maxHealthPickups - healthPickups.Count;
        for (int i = 0; i < need; i++)
        {
            SpawnHealthPickup();
        }
    }

    void SpawnHealthPickup()
    {
        if (healthPrefab == null || player == null) return;

        Vector3 spawnPos = GetRandomSpawnPosition();

        if (spawnPos != Vector3.zero)
        {
            GameObject health = Instantiate(healthPrefab, spawnPos, Quaternion.identity);
            healthPickups.Add(health);
            Debug.Log("The heart has been spawned at the position: " + spawnPos);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        for (int i = 0; i < 10; i++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(minDistanceFromPlayer, spawnRadius);
            
            Vector3 randomPos = new Vector3(
                player.position.x + Mathf.Cos(angle) * distance,
                player.position.y,
                player.position.z + Mathf.Sin(angle) * distance
            );

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 10f, NavMesh.AllAreas))
            {
                return hit.position + Vector3.up * spawnHeight;
            }
        }

        float fallbackAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float fallbackDist = Random.Range(minDistanceFromPlayer, spawnRadius);
        return new Vector3(
            player.position.x + Mathf.Cos(fallbackAngle) * fallbackDist,
            player.position.y + spawnHeight,
            player.position.z + Mathf.Sin(fallbackAngle) * fallbackDist
        );
    }
}
