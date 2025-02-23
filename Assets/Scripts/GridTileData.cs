using System;
using UnityEngine;

[System.Serializable]
public class GridTileData
{
    public GridTile.TileType tileType;
    public int antsCount;
    public int specialAntsCount;

    public int maxLifeScore;
    public int currentLifeScore;

    public bool dead;

    public GridTileData(GridTile.TileType type, int maxLifeScore)
    {
        tileType = type;
        antsCount = 0;
        specialAntsCount = 0;
        this.maxLifeScore = maxLifeScore;
        currentLifeScore = maxLifeScore;

        dead = false;
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

    public bool ChangeLifeScore(int antsOnTile, string nameName)
    {
        if(dead)
            return false;

        currentLifeScore -= antsOnTile;

        //Recovery
        float recovery = maxLifeScore / 100 * UnityEngine.Random.Range(1, 5);

        currentLifeScore += (int)recovery;


        currentLifeScore = Math.Min(Math.Max(0, currentLifeScore), maxLifeScore);

        //Debug.Log($"Tile {nameName} - {tileType} has {currentLifeScore} / {maxLifeScore} life score with ant {antsOnTile} recovering {recovery}");

        if (currentLifeScore <= 0)
        {
            dead = true;
            return true;
        }
        return false;
    }

}
