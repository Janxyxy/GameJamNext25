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
    [SerializeField] private Dictionary<GridTile, GridTileData> tileDataDictionary = new Dictionary<GridTile, GridTileData>();
    public Dictionary<GridTile, GridTileData> TileDataDictionary => tileDataDictionary;

    private Dictionary<Room, RoomData> roomDataDictionary = new Dictionary<Room, RoomData>();
    public Dictionary<Room, RoomData> RoomDataDictionary => roomDataDictionary;

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
    }

    private void Loadresources()
    {
        tileInfos.AddRange(Resources.LoadAll<TileSO>("Tiles"));
    }

    private void Start()
    {
        ResourcesManager.Instance.AddResource(GameResourceType.Ant, 10);
        ResourcesManager.Instance.AddResource(GameResourceType.SpecialAnt, 1);
        ResourcesManager.Instance.AddResource(GameResourceType.Food, 25);
    }

    internal void OnTileClick(TileType tileType, GridTile gridTile)
    {
        currentTile = gridTile;

        GridTileData tileData = tileDataDictionary[gridTile];

        for (int i = 0; i < tileInfos.Count; i++)
        {
            if (tileInfos[i].name == tileType.ToString())
            {
                tileInfoUI.SetUI(tileInfos[i].tileName, tileInfos[i].description);

                if (tileData.tileType == TileType.None)
                {
                    tileInfoUI.SetAntCount(tileData.specialAntsCount);
                }
                else
                {
                    tileInfoUI.SetAntCount(tileData.antsCount);
                }
            }
        }

        tileInfoUI.SetNormalTileUI(tileType != TileType.Anthill);
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

    internal void RemoveAntsFromRoom(Room room)
    {
        foreach (KeyValuePair<Room, RoomData> entry in roomDataDictionary)
        {
            if (entry.Key == room)
            {
                bool removed = entry.Value.RemoveAnt(editMultiplier);
                if (removed)
                    ResourcesManager.Instance.AddResource(GameResourceType.Ant, editMultiplier);
            }
        }
    }

    internal void AddAntsToRoom(Room room)
    {
        foreach (KeyValuePair<Room, RoomData> entry in roomDataDictionary)
        {
            if (entry.Key == room)
            {
                bool added = ResourcesManager.Instance.RemoveResource(GameResourceType.Ant, editMultiplier);
                if (added)
                    entry.Value.AddAnt(editMultiplier);

            }
        }
    }

    public void AddAntToCurrentTile(int count, bool v)
    {
        if (currentTile != null && tileDataDictionary.ContainsKey(currentTile))
        {
            if (v)
            {
                tileDataDictionary[currentTile].AddSpecialAnt(count);
                tileInfoUI.SetAntCount(tileDataDictionary[currentTile].specialAntsCount);

                currentTile.ActivateBoost(true);
            }
            else
            {
                tileDataDictionary[currentTile].AddAnt(count);
                tileInfoUI.SetAntCount(tileDataDictionary[currentTile].antsCount);
            }
        }
    }

    public bool RemoveAntFromCurrentTile(int count, bool v)
    {
        if (currentTile != null && tileDataDictionary.ContainsKey(currentTile))
        {
            if (v)
            {
                bool removed = tileDataDictionary[currentTile].RemoveSpecialAnt(count);
                if (!removed)
                {
                    Debug.Log("No Special to remove");
                }
                tileInfoUI.SetAntCount(tileDataDictionary[currentTile].specialAntsCount);
                currentTile.ActivateBoost(false);
                return removed;
            }
            else
            {          
                bool removed = tileDataDictionary[currentTile].RemoveAnt(count);
                if (!removed)
                {
                    Debug.Log("No ants to remove");
                }
                tileInfoUI.SetAntCount(tileDataDictionary[currentTile].antsCount);
                return removed;
            }
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

    // Registering rooms and tiles
    internal void RegisterRoom(Room room)
    {
        if (!roomDataDictionary.ContainsKey(room))
        {
            roomDataDictionary[room] = new RoomData();
        }
    }
    internal void RegisterTile(TileType tileType, GridTile gridTile)
    {
        if (!tileDataDictionary.ContainsKey(gridTile))
        {
            tileDataDictionary[gridTile] = new GridTileData(tileType);
        }
    }

    public TileType GetTileTypeOfCurrentTile()
    {
        foreach (KeyValuePair<GridTile, GridTileData> entry in tileDataDictionary)
        {
            if (entry.Key == currentTile)
            {
                return entry.Value.tileType;
            }
        }
        return TileType.None;
    }
}
