using System;
using System.Collections.Generic;
using UnityEngine;
using static GridTile;

public class TileManager : MonoBehaviour
{
    protected List<GridTile> tiles = new List<GridTile>();
    protected List<GridTile> tilesRND = new List<GridTile>();


    // Minimum counts for each tile type
    private Dictionary<TileType, int> minTileCounts = new Dictionary<TileType, int>
    {
        { TileType.None, 4 },     // At least None tiles
        { TileType.Forest, 3 },   // At least Forest tiles
        { TileType.Mountain, 3 }, // At least Mountain tiles
        { TileType.Meadow, 10 },   // At least Meadow tiles
        { TileType.Cave, 3 }      // At least Cave tiles
    };

    void Start()
    {
        FindTiles();
        RandomizeRNDTiles();
        RandomizeTilesWithMinCounts();
    }

    private void RandomizeRNDTiles()
    {
        // Ensure there are tiles in the tilesRND list
        if (tilesRND.Count == 0)
        {
            Debug.LogWarning("No tiles in the tilesRND list to randomize!");
            return;
        }

        // Calculate how many tiles to hide (e.g., hide 30% of the tiles)
        int tilesToHide = Mathf.RoundToInt(tilesRND.Count * 0.3f); // Adjust the percentage as needed

        // Ensure at least one tile is hidden
        tilesToHide = Mathf.Max(1, tilesToHide);

        // Hide random tiles
        for (int i = 0; i < tilesToHide; i++)
        {
            // Get a random index from the tilesRND list
            int randomIndex = UnityEngine.Random.Range(0, tilesRND.Count);

            // Deactivate the tile's GameObject
            tilesRND[randomIndex].gameObject.SetActive(false);

            // Remove the tile from the tilesRND list to avoid hiding it again
            tilesRND.RemoveAt(randomIndex);

            // Log the hidden tile for debugging
            //Debug.Log($"Hidden tile at index {randomIndex}");
        }
    }

    private void FindTiles()
    {
        // Find all GridTile objects in the scene
        GridTile[] foundTiles = FindObjectsByType<GridTile>(FindObjectsSortMode.InstanceID);



        // Add the found tiles to the list
        tiles.AddRange(foundTiles);

        foreach (GridTile tile in foundTiles)
        {
            if (tile.isRandomTile == true)
            {
                tilesRND.Add(tile);
            }
        }

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

        // Set the tile icons and register the tiles
        foreach (GridTile tile in tiles)
        {
            tile.SetTileIcon();
            GameManager.Instance.RegisterTile(tile.tileType, tile);

            if(tile.tileType == TileType.Anthill)
            {
                GameManager.Instance.OnTileClick(tile.tileType, tile);
            }
        }
    }

    private TileType GetRandomTileTypeExcludingAnthill()
    {
        // List of tile types to randomize (excluding Anthill)
        TileType[] tileTypes = { TileType.None, TileType.Forest, TileType.Mountain, TileType.Meadow, TileType.Cave };

        // Pick a random tile type
        int randomIndex = UnityEngine.Random.Range(0, tileTypes.Length);
        return tileTypes[randomIndex];

    }
}