using UnityEngine;

[System.Serializable]
public class GridTileData
{
    public GridTile.TileType tileType;
    public int antsCount;

    public GridTileData(GridTile.TileType type)
    {
        tileType = type;
        antsCount = 0; 
    }

    public void AddAnt(int count)
    {
        antsCount += count;
    }

    public bool RemoveAnt(int count)
    {
        if (antsCount >= count)
        {
            antsCount -= count;
            return true;
        }
        return false;
    }
}
