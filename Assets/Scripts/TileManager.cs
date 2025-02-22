using System;
using System.Collections.Generic;
using UnityEngine;
using static GridTile;

public class TileManager : MonoBehaviour
{
    protected List<GridTile> tiles = new List<GridTile>();

    // Minimum counts for each tile type
    private Dictionary<TileType, int> minTileCounts = new Dictionary<TileType, int>
    {
        { TileType.None, 5 },     // At least None tiles
        { TileType.Forest, 3 },   // At least Forest tiles
        { TileType.Mountain, 3 }, // At least Mountain tiles
        { TileType.Meadow, 3 },   // At least Meadow tiles
        { TileType.Cave, 3 }      // At least Cave tiles
    };

    void Start()
    {
        FindTiles();
        RandomizeTilesWithMinCounts();
    }

    private void FindTiles()
    {
        // Find all GridTile objects in the scene
        GridTile[] foundTiles = FindObjectsByType<GridTile>(FindObjectsSortMode.InstanceID);

        // Add the found tiles to the list
        tiles.AddRange(foundTiles);

        // Optional: Log the number of tiles found for debugging
        Debug.Log($"Found {tiles.Count} tiles in the scene.");
    }

    private void RandomizeTilesWithMinCounts()
    {
        // List to track tiles that haven't been assigned a type yet
        List<GridTile> unassignedTiles = new List<GridTile>(tiles);

        // Assign the Anthill tile first
        foreach (GridTile tile in unassignedTiles)
        {
            if (tile.name == "Tile")
            {
                tile.ChangeTileType(TileType.Anthill);
                unassignedTiles.Remove(tile);
                break; // Only one Anthill tile
            }
        }

        // Assign the minimum required tiles for each type
        foreach (var kvp in minTileCounts)
        {
            TileType tileType = kvp.Key;
            int minCount = kvp.Value;

            for (int i = 0; i < minCount; i++)
            {
                if (unassignedTiles.Count == 0)
                {
                    Debug.LogWarning("Not enough tiles to satisfy minimum counts!");
                    return;
                }

                // Assign the tile type to a random unassigned tile
                int randomIndex = UnityEngine.Random.Range(0, unassignedTiles.Count);
                GridTile tile = unassignedTiles[randomIndex];
                tile.ChangeTileType(tileType);
                unassignedTiles.RemoveAt(randomIndex);
            }
        }

        // Randomize the remaining tiles
        foreach (GridTile tile in unassignedTiles)
        {
            // Randomize the tile type (excluding Anthill)
            TileType randomTileType = GetRandomTileTypeExcludingAnthill();

            //Debug.Log($"{tile.name} - {randomTileType}");
            tile.ChangeTileType(randomTileType);
        }

        foreach (GridTile tile in tiles) {
            tile.SetTileIcon();
        
        }
    }

    private TileType GetRandomTileTypeExcludingAnthill()
    {
        // List of tile types to randomize (excluding Anthill)
        TileType[] tileTypes = { TileType.None, TileType.Forest, TileType.Mountain, TileType.Meadow };

        // Pick a random tile type
        int randomIndex = UnityEngine.Random.Range(0, tileTypes.Length);
        return tileTypes[randomIndex];
      
    }
}