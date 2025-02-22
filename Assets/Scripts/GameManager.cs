using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GridTile;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TileInfoUI tileInfoUI;
    [SerializeField] private List<TileSO> tileInfos = new List<TileSO>();

    public static GameManager Instance { get; private set; }

    private GridTile currentTile = null;
    private Dictionary<GridTile, GridTileData> tileDataDictionary = new Dictionary<GridTile, GridTileData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Loadresources();
    }

    private void Loadresources()
    {
        tileInfos.AddRange(Resources.LoadAll<TileSO>("Tiles"));
    }


    internal void OnTileClick(TileType tileType, GridTile gridTile)
    {
        currentTile = gridTile;

        if (!tileDataDictionary.ContainsKey(gridTile))
        {
            tileDataDictionary[gridTile] = new GridTileData(tileType);
        }

        GridTileData tileData = tileDataDictionary[gridTile];

        for (int i = 0; i < tileInfos.Count; i++)
        {
            if (tileInfos[i].name == tileType.ToString())
            {
                tileInfoUI.SetUI(tileInfos[i].name, tileInfos[i].description);
                tileInfoUI.SetAntCount(tileData.antsCount);
                break;
            }
        }

        tileInfoUI.SetNormalTileUI(tileType != TileType.AntHill);
    }

    public void AddAntToCurrentTile()
    {
        if (currentTile != null && tileDataDictionary.ContainsKey(currentTile))
        {
            tileDataDictionary[currentTile].AddAnt();
            tileInfoUI.SetAntCount(tileDataDictionary[currentTile].antsCount);
        }
    }

    public void RemoveAntFromCurrentTile()
    {
        if (currentTile != null && tileDataDictionary.ContainsKey(currentTile))
        {
            bool removed = tileDataDictionary[currentTile].RemoveAnt();
            if (!removed)
            {
                Debug.Log("No ants to remove");
            }
            tileInfoUI.SetAntCount(tileDataDictionary[currentTile].antsCount);
        }
    }

}
