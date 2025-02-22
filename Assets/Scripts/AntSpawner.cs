using UnityEngine;
using System.Collections;

public class AntSpawner : MonoBehaviour
{
    public GameObject antPrefab; // Assign in Inspector
    public float spawnRadius = 10f; // Distance from screen edge to spawn
    public float speed = 2f; // Movement speed

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main Camera not found! Make sure it's tagged 'MainCamera'.");
            return;
        }
        StartCoroutine(SpawnAntsContinuously());
    }

    IEnumerator SpawnAntsContinuously()
    {
        while (true)
        {
            SpawnAnt();
            yield return new WaitForSeconds(1f); // Adjust spawn interval as needed
        }
    }

    void SpawnAnt()
    {
        Vector3 spawnPos = GetRandomOffscreenPosition();
        Vector3 targetPos = GetRandomOnscreenPosition();

        GameObject ant = Instantiate(antPrefab, spawnPos + new Vector3(0,0, 3), Quaternion.identity);
        
        ant.GetComponent<AntMover>().Initialize(targetPos, speed);

        Debug.Log($"Spawned Ant at {spawnPos} moving to {targetPos}");
    }

    Vector2 GetRandomOffscreenPosition()
    {
        Vector3 camPos = cam.transform.position;
        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        int side = Random.Range(0, 4);
        Vector2 pos = Vector2.zero;

        switch (side)
        {
            case 0: // Left
                pos = new Vector2(camPos.x - camWidth - spawnRadius, Random.Range(camPos.y - camHeight, camPos.y + camHeight));
                break;
            case 1: // Right
                pos = new Vector2(camPos.x + camWidth + spawnRadius, Random.Range(camPos.y - camHeight, camPos.y + camHeight));
                break;
            case 2: // Top
                pos = new Vector2(Random.Range(camPos.x - camWidth, camPos.x + camWidth), camPos.y + camHeight + spawnRadius);
                break;
            case 3: // Bottom
                pos = new Vector2(Random.Range(camPos.x - camWidth, camPos.x + camWidth), camPos.y - camHeight - spawnRadius);
                break;
        }
        return pos;
    }

    Vector2 GetRandomOnscreenPosition()
    {
        Vector3 viewportPoint = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 50);
        return cam.ViewportToWorldPoint(viewportPoint);
    }
}
