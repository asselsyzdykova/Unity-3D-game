using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int maxZombies = 5;
    public float spawnRadius = 20f;
    public float checkDelay = 1f;

    private Transform player;
    private List<GameObject> zombies = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating(nameof(CheckZombies), 0f, checkDelay);
    }

    void CheckZombies()
    {
        zombies.RemoveAll(z => z == null);

        int need = maxZombies - zombies.Count;
        for (int i = 0; i < need; i++)
        {
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        Vector3 spawnPos = GetRandomPointAroundPlayer();

        GameObject zombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
        zombies.Add(zombie);
    }

    Vector3 GetRandomPointAroundPlayer()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 randomPos = new Vector3(
            player.position.x + randomCircle.x,
            player.position.y,
            player.position.z + randomCircle.y
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, 5f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return player.position + Vector3.forward * spawnRadius;
    }
}