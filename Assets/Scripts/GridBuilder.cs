using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] private GameObject hexTilePrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private int layerCount = 3; // Number of hex layers
    [SerializeField] private float tileSize = 1.0f; // Tile size in Unity world units

    private HashSet<Vector2Int> existingTiles = new HashSet<Vector2Int>();
    private Vector3 gridCenter;

    private void Start()
    {
        gridCenter = gridParent.position; // Set grid center
        CreateHexGrid();
    }

    private void CreateHexGrid()
    {
        PlaceTile(0, 0); // Place the center tile

        for (int radius = 1; radius <= layerCount; radius++)
        {
            GenerateLayer(radius);
        }
    }

    private void GenerateLayer(int radius)
    {
        // Hexagonal movement directions (right, bottom-right, bottom-left, left, top-left, top-right)
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),   // Right
            new Vector2Int(1, -1),  // Bottom-right
            new Vector2Int(0, -1),  // Bottom-left
            new Vector2Int(-1, 0),  // Left
            new Vector2Int(-1, 1),  // Top-left
            new Vector2Int(0, 1)    // Top-right
        };

        // Start at the rightmost point of this layer
        Vector2Int tilePos = new Vector2Int(radius, 0);

        // Move around the ring
        for (int dir = 0; dir < 6; dir++)
        {
            for (int step = 0; step < radius; step++)
            {
                PlaceTile(tilePos.x, tilePos.y);
                tilePos += directions[dir]; // Move in the current direction
            }
        }
    }

    private void PlaceTile(int q, int r)
    {
        Vector2Int axialCoords = new Vector2Int(q, r);
        if (existingTiles.Contains(axialCoords)) return; // Avoid duplicate tiles

        Vector3 worldPosition = AxialToWorld(q, r);
        GameObject tile = Instantiate(hexTilePrefab, worldPosition, Quaternion.identity, gridParent);

        existingTiles.Add(axialCoords);
    }

    private Vector3 AxialToWorld(int q, int r)
    {
        float x = tileSize * (1.5f * q); // Proper hex spacing
        float y = tileSize * (Mathf.Sqrt(3) * (r + q * 0.5f));

        return gridCenter + new Vector3(x, y, 0); // Offset by center position
    }
}
