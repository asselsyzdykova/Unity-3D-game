using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject[] plants;
    public Terrain terrain;
    public int count = 100;

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(0f, terrain.terrainData.size.x);
            float z = Random.Range(0f, terrain.terrainData.size.z);
            float y = terrain.SampleHeight(new Vector3(x, 0, z));

            Vector3 pos = new Vector3(x, y, z) + terrain.transform.position;

            GameObject plant = plants[Random.Range(0, plants.Length)];

            Instantiate(plant, pos, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }
}
