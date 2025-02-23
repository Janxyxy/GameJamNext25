using System;
using System.Diagnostics;

[System.Serializable]
public class GridTileData
{
    public GridTile.TileType tileType;
    public int antsCount;
    public int specialAntsCount;
    public bool isBoosted;
    public int maxResorces;

    public GridTileData(GridTile.TileType type)
    {
        tileType = type;
        antsCount = 0; 
        specialAntsCount = 0;
        maxResorces = GameManager.Instance.GetMaxResources(type);
        isBoosted = false;
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

    public void AddSpecialAnt(int count)
    {
        specialAntsCount += count;
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
    public void Boost(bool boost)
    {
        isBoosted = boost;
    }
}
