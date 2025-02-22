using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GridTile;
using static ResourcesManager;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TileInfoUI tileInfoUI;
    [SerializeField] private List<TileSO> tileInfos = new List<TileSO>();

    public static GameManager Instance { get; private set; }

    private GridTile currentTile = null;
    private Dictionary<GridTile, GridTileData> tileDataDictionary = new Dictionary<GridTile, GridTileData>();
    public Dictionary<GridTile, GridTileData> TileDataDictionary => tileDataDictionary;

    private int editMultiplier = 1;
    public int EditMultiplier => editMultiplier;

    private bool isMapOpen = true;
    public bool IsMapOpen => isMapOpen;

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


        ResourcesManager.Instance.AddResource(GameResourceType.Ant, 10);
        ResourcesManager.Instance.AddResource(GameResourceType.Food, 25);
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

        tileInfoUI.SetNormalTileUI(tileType != TileType.Anthill);
    }

    internal void RegisterTile(TileType tileType, GridTile gridTile)
    {     
        if (!tileDataDictionary.ContainsKey(gridTile))
        {
            tileDataDictionary[gridTile] = new GridTileData(tileType);
        }
    }

    internal Sprite GetTileIcon(TileType tileType)
    {
        for (int i = 0; i < tileInfos.Count; i++)
        {
            if (tileInfos[i].name == tileType.ToString())
            {

                return tileInfos[i].icon;
            }
        }

        Debug.LogError("No icon found for tile type: " + tileType.ToString());
        return null;
    }

    internal void SerProggresBarFill(float fill)
    {
        UIManager.Instance.SerProggresBarFill(fill);
    }

    public void AddAntToCurrentTile(int count)
    {
        if (currentTile != null && tileDataDictionary.ContainsKey(currentTile))
        {
            tileDataDictionary[currentTile].AddAnt(count);
            tileInfoUI.SetAntCount(tileDataDictionary[currentTile].antsCount);
        }
    }

    public bool RemoveAntFromCurrentTile(int count)
    {
        if (currentTile != null && tileDataDictionary.ContainsKey(currentTile))
        {
            bool removed = tileDataDictionary[currentTile].RemoveAnt(count);
            if (!removed)
            {
                Debug.Log("No ants to remove");
            }
            tileInfoUI.SetAntCount(tileDataDictionary[currentTile].antsCount);

            return removed;
        }
        return false;
    }

    public bool SetEditMultiplier(int value)
    {
        if (value > 0)
        {
            Debug.Log("Edit multiplier set to: " + value);
            editMultiplier = value;
            return true;
        }
        return false;
    }

    internal void ToggleMap()
    {
        isMapOpen = !isMapOpen;
    }

}
