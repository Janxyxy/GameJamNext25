using System;
using System.Collections.Generic;
using UnityEngine;
using static GridTile;
using static ResourcesManager;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TileInfoUI tileInfoUI;

    [Header("Settings")]
    [SerializeField] private int countdownTimeInSeconds = 301;
    [SerializeField] private float generatedCountDuration = 0.75f;
    [SerializeField] private int queenQuestMin = 40;
    [SerializeField] private int queenQuestMax = 55;
    [SerializeField] private bool devMode = false;


    public int QueenQuestMin => queenQuestMin;
    public int QueenQuestMax => queenQuestMax;
    public int CountdownTimeInSeconds => countdownTimeInSeconds;
    public bool DevMode => devMode;
    public float GeneratedCountDuration => generatedCountDuration;

    private bool canAntsBeGenerated = true;


    private List<TileSO> tileInfos = new List<TileSO>();

    public static GameManager Instance { get; private set; }

    private GridTile currentTile = null;

    public GridTile CurrentTile => currentTile;
    [SerializeField] private Dictionary<GridTile, GridTileData> tileDataDictionary = new Dictionary<GridTile, GridTileData>();
    public Dictionary<GridTile, GridTileData> TileDataDictionary => tileDataDictionary;

    private Dictionary<Room, RoomData> roomDataDictionary = new Dictionary<Room, RoomData>();
    public Dictionary<Room, RoomData> RoomDataDictionary => roomDataDictionary;

    private int editMultiplier = 1;
    public int EditMultiplier => editMultiplier;

    private bool isMapOpen = true;
    public bool IsMapOpen => isMapOpen;
    private bool tacticalView = false;
    public bool TacticalView => tacticalView;

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
        ResourcesManager.Instance.AddResource(GameResourceType.Food, 25);

        if (devMode)
        {
            ResourcesManager.Instance.AddResource(GameResourceType.Wood, 1000);
             ResourcesManager.Instance.AddResource(GameResourceType.Stone, 1000);
             ResourcesManager.Instance.AddResource(GameResourceType.Ant, 1000);
             ResourcesManager.Instance.AddResource(GameResourceType.Gem, 1000);
        }
       
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
                    
                }
                else
                {
                    tileInfoUI.SetAntCount(tileData.antsCount);
                }
            }
        }

        tileInfoUI.SetNormalTileUI(tileType);
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
            int maxLifeScore = GetLifrScore(tileType);
            tileDataDictionary[gridTile] = new GridTileData(tileType, maxLifeScore);
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

    internal void ToggleTacticalView()
    {
        tacticalView = !tacticalView;
    }

    internal GridTileData GetTileData(GridTile gridTile)
    {
        if (tileDataDictionary == null || tileDataDictionary.Count == 0)
        {
            return null;
        }

        if (tileDataDictionary.TryGetValue(gridTile, out GridTileData data))
        {
            return data;
        }
        return null;
    }

    internal int GetLifrScore(TileType type)
    {
        if (type == TileType.Forest || type == TileType.Mountain)
        {
            return UnityEngine.Random.Range(450, 750);
        }
        else if (type == TileType.Meadow)
        {
            return UnityEngine.Random.Range(600, 800);
        }
        else if (type == TileType.Cave)
        {
            return UnityEngine.Random.Range(300, 500);
        }
        else
        {
            return 0;
        }
    }

    internal void SetCanBeAntsGenerated(bool value)
    {
        canAntsBeGenerated = value;
    }

    internal bool CanAntsBeGenerated()
    {
        return canAntsBeGenerated;
    }
}
