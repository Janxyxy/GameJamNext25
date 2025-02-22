using System.Diagnostics;

[System.Serializable]
public class GridTileData
{
    public GridTile.TileType tileType;
    public int antsCount;
    public int specialAntsCount;

    public GridTileData(GridTile.TileType type)
    {
        tileType = type;
        antsCount = 0; 
    }

    public void AddAnt(int count)
    {
        antsCount += count;
    }

    public void AddSpecialAnt(int count)
    {
        specialAntsCount += count;
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

    public bool RemoveSpecialAnt(int count)
    {
        if (specialAntsCount >= count)
        {
            specialAntsCount -= count;
            return true;
        }
        return false;
    }
}
