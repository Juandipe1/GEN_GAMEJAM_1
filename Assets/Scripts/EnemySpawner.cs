using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private GameObject enemy1;
    [SerializeField]
    private GameObject enemy2;

    [SerializeField]
    private float enemy1Interval = 3.5f;
    [SerializeField]
    private float enemy2Interval = 6f;
    void Start()
    {
        StartCoroutine(spawnEnemy(enemy1Interval, enemy1));
        StartCoroutine(spawnEnemy(enemy2Interval, enemy2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);

        // Generar posici�n aleatoria sobre el suelo
        Vector3 spawnPosition = transform.position;

        Instantiate(enemy, spawnPosition, Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
