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
        if (dead)
            return false;

        currentLifeScore -= antsOnTile;
        float recoveryMultiplier = 1f;

        int maxAntsForMult = 100;
        int maxMult = 3;

        if (antsOnTile != 0)
        {
            recoveryMultiplier = Mathf.Max(1f, maxMult - (antsOnTile - 1) * ((float)(maxMult - 1) / maxAntsForMult));
        }
        else
        {
            recoveryMultiplier = maxMult; // Recovery multiplier for 0 ants
        }


        float recovery = (maxLifeScore / 100f) * UnityEngine.Random.Range(1, 5) * recoveryMultiplier;

        currentLifeScore += (int)recovery;

        currentLifeScore = Math.Min(Math.Max(0, currentLifeScore), maxLifeScore);

        Debug.Log($"Tile {nameName} - {tileType} has {currentLifeScore} / {maxLifeScore} life score with ant {antsOnTile} recovering {recovery} recoveryMultiplayer {recoveryMultiplier}");

        if (currentLifeScore <= 0)
        {
            dead = true;
            return true;
        }
        return false;
    }

}
