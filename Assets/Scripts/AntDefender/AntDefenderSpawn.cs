using UnityEngine;

public class AntDefenderSpawn : MonoBehaviour
{
    [SerializeField] private GameObject antAttackerPrefab;

    [SerializeField] private float spawnRate = 5f;
    private float nextSpawnTime = 0f;

    [SerializeField] private float minSpawnRadius = 5f;
    [SerializeField] private float maxSpawnRadius = 10f;

    [SerializeField] private int numberOfEnemier = 35;

    private Transform antDefender;

    private void Awake()
    {
        antDefender = GetComponent<Transform>();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnAntDefender();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnAntDefender()
    {
        numberOfEnemier--;
        Debug.Log("Number of enemies: " + numberOfEnemier);
        if (numberOfEnemier <= 0)
        {
            // Victory
            return;
        }
        float x = Random.Range(minSpawnRadius, maxSpawnRadius);
        float y = Random.Range(minSpawnRadius, maxSpawnRadius);
        x *= Random.value > 0.5f ? 1 : -1;
        y *= Random.value > 0.5f ? 1 : -1;

        Vector3 spawnPosition = antDefender.position + new Vector3(x, y,0);

        Instantiate(antAttackerPrefab, spawnPosition, Quaternion.identity);
    }
}
