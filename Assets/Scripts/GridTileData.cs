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

    public void AddAnt()
    {
        antsCount++;
    }

    public bool RemoveAnt()
    {
        if (antsCount > 0)
        {
            antsCount--;
            return true;
        }
        return false;
    }
}
