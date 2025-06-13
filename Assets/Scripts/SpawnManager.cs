using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject spherePrefab; // ← asigna aquí el prefab llamado "Sphere"
    public float spawnIntervalMin = 1f;
    public float spawnIntervalMax = 3f;
    public int maxSpheres = 10;

    private int currentSpheres = 0;

    void Start()
    {
        Invoke("SpawnSphere", Random.Range(spawnIntervalMin, spawnIntervalMax));
    }

    void SpawnSphere()
    {
        if (currentSpheres >= maxSpheres) return;

        GameObject sphere = Instantiate(spherePrefab, transform.position, Quaternion.identity);
        currentSpheres++;

        // Invoca el siguiente spawn
        Invoke("SpawnSphere", Random.Range(spawnIntervalMin, spawnIntervalMax));
    }
}
