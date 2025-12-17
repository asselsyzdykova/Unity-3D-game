using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AmmoSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject ammoPrefab; 
    public int maxAmmoPacks = 3; 
    public float spawnRadius = 25f;
    public float checkDelay = 7f;
    public float minDistanceFromPlayer = 10f;

    private Transform player;
    private List<GameObject> activeAmmoPacks = new List<GameObject>();

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        InvokeRepeating(nameof(CheckAmmo), 1f, checkDelay);
    }

    void CheckAmmo()
    {
        if (player != null)
        {
            for (int i = activeAmmoPacks.Count - 1; i >= 0; i--)
            {
                if (activeAmmoPacks[i] != null)
                {
                    float distance = Vector3.Distance(player.position, activeAmmoPacks[i].transform.position);
                    
                    if (distance > spawnRadius + 10f)
                    {
                        Destroy(activeAmmoPacks[i]);
                    }
                }
            }
        }

        activeAmmoPacks.RemoveAll(pack => pack == null);

        int needToSpawn = maxAmmoPacks - activeAmmoPacks.Count;
        for (int i = 0; i < needToSpawn; i++)
        {
            SpawnPack();
        }
    }

    void SpawnPack()
    {
        if (ammoPrefab == null || player == null) return;

        Vector3 pos = GetRandomPos();
        if (pos != Vector3.zero)
        {
            GameObject pack = Instantiate(ammoPrefab, pos, Quaternion.identity);
            activeAmmoPacks.Add(pack);
        }
    }

    Vector3 GetRandomPos()
    {
        for (int i = 0; i < 10; i++)
        {
            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            float dist = Random.Range(minDistanceFromPlayer, spawnRadius);
            Vector3 randomPoint = player.position + new Vector3(Mathf.Cos(angle) * dist, 0, Mathf.Sin(angle) * dist);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
            {
                return hit.position + Vector3.up * 0.5f; 
            }
        }
        return Vector3.zero;
    }
}
