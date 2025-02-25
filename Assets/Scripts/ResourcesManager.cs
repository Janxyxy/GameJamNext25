using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GridTile;
using static Room;

public class ResourcesManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform resourcesParent;
    [SerializeField] private ResourceUI resourcePrefab;

    [Header("Settings")]
    [SerializeField] private float generationDuration = 3f; //in seconds
    public float GenerationDuration => generationDuration;

    public static ResourcesManager Instance { get; private set; }

    private List<Sprite> resourcesSprites = new List<Sprite>();

    private Dictionary<GameResourceType, ResourceUI> resourceUIs = new Dictionary<GameResourceType, ResourceUI>();
    private Dictionary<GameResourceType, int> resources = new Dictionary<GameResourceType, int>();

    public Dictionary<GameResourceType, int> GameResources => resources;

    private int allAnts;
    public int AllAnts => allAnts;

    public event Action<GameResourceType> OnResourceChanged;

    public enum GameResourceType
    {
        Ant,
        Food,
        Wood,
        Stone,
        Gem
    }
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

        LoadResources();
        InitializeResources();
    }

    private void Start()
    {
        StartCoroutine(GenerateResourcesCorutine());
    }

    private void LoadResources()
    {
        resourcesSprites.AddRange(Resources.LoadAll<Sprite>("Resources"));
        Debug.Log($"Loaded {resourcesSprites.Count} resources");
    }

    private void InitializeResources()
    {
        foreach (GameResourceType type in System.Enum.GetValues(typeof(GameResourceType)))
        {
            resources[type] = 0;
            ResourceUI resurceUI = Instantiate(resourcePrefab, resourcesParent);
            resurceUI.SetCount(0);

            foreach (Sprite sprite in resourcesSprites)
            {
                if (sprite.name == type.ToString())
                {
                    resurceUI.SetImage(sprite);
                    break;
                }
            }

            resourceUIs.Add(type, resurceUI);
        }
    }

    internal void AddResource(GameResourceType type, int amount)
    {
        if (resources.ContainsKey(type))
        {
            resources[type] += amount;
        }

        Debug.Log($"Added {amount} {type}. Total: {resources[type]}");

        OnResourceChanged?.Invoke(type);

        // Update count in UI
        resourceUIs[type].SetCount(resources[type]);
    }

    internal bool RemoveResource(GameResourceType type, int amount)
    {
        if (resources.ContainsKey(type) && resources[type] >= amount)
        {
            resources[type] -= amount;
            Debug.Log($"Removed {amount} {type}. Remaining: {resources[type]}");
            // Update count in UI
            resourceUIs[type].SetCount(resources[type]);

            OnResourceChanged?.Invoke(type);

            return true;
        }

        Debug.LogWarning($"Not enough {type} to remove!");
        return false;
    }

    internal int GetResourceAmount(GameResourceType type)
    {
        return resources.ContainsKey(type) ? resources[type] : 0;
    }

    internal bool HasEnoughResource(GameResourceType type, int amount)
    {
        return resources.ContainsKey(type) && resources[type] >= amount;
    }

    private IEnumerator GenerateResourcesCorutine()
    {
        while (true)
        {
            float timer = 0f;


            while (timer < generationDuration)
            {
                float fillRatio = timer / generationDuration;
                GameManager.Instance.SerProggresBarFill(fillRatio);

                timer += Time.deltaTime;
                yield return null;
            }

            // Meadow are generated first
            foreach (KeyValuePair<GridTile, GridTileData> entry in GameManager.Instance.TileDataDictionary
           .OrderBy(e => e.Value.tileType != TileType.Meadow))
            {
                GridTile gridTile = entry.Key;
                GridTileData tileData = entry.Value;

                int antsOnTile = tileData.antsCount;           

                int availableFood = GetResourceAmount(GameResourceType.Food);
                bool dead = false;

                gridTile.StartShowGainCount();

                //Debug.Log($"Ants on tile {gridTile.name} - {antsOnTile}");


                Debug.Log($"Available food: {availableFood}");

                if (tileData.tileType == TileType.Meadow)
                {
                    AddResource(GameResourceType.Food, antsOnTile);
                    gridTile.SetGainCount(antsOnTile);

                    dead = tileData.ChangeLifeScore(antsOnTile, gridTile.name);
                }
                else if (tileData.tileType == TileType.Forest)
                {
                    int woodToGenerate = 0;
                    if (availableFood > 0)
                    {
                        // Generate as many wood units as food permits.
                        woodToGenerate = Mathf.Min(antsOnTile, availableFood);
                        RemoveResource(GameResourceType.Food, woodToGenerate);
                        AddResource(GameResourceType.Wood, woodToGenerate);

                    }
                    gridTile.SetGainCount(woodToGenerate);
                    dead = tileData.ChangeLifeScore(woodToGenerate, gridTile.name);

                }
                else if (tileData.tileType == TileType.Mountain)
                {
                    int stoneToGenerate = 0;
                    if (availableFood > 0)
                    {
                        // Generate as many stone units as food permits.
                        stoneToGenerate = Mathf.Min(antsOnTile, availableFood);
                        RemoveResource(GameResourceType.Food, stoneToGenerate);
                        AddResource(GameResourceType.Stone, stoneToGenerate);


                    }
                    gridTile.SetGainCount(stoneToGenerate);
                    dead = tileData.ChangeLifeScore(stoneToGenerate, gridTile.name);

                }
                else if (tileData.tileType == TileType.Cave)
                {
                    int gemsGenerated = 0;
                    int foodUsed = 0;

                    if (availableFood > 0)
                    {

                        int foodCounter = availableFood;

                        for (int i = 0; i < antsOnTile; i++)
                        {
                            RemoveResource(GameResourceType.Food, 1);
                            foodUsed++;
                            if (foodCounter <= 0)
                                break;

                            if (UnityEngine.Random.value < 0.10f)
                            {
                                AddResource(GameResourceType.Gem, 1);
                                gemsGenerated++;
                                foodCounter--;
                            }
                        }
                    }
                    gridTile.SetGainCount(gemsGenerated);
                    dead = tileData.ChangeLifeScore(foodUsed, gridTile.name);
                }

                if (dead)
                {
                    gridTile.Die();
                    continue;
                }
            }

            GenerateResourcesFromRooms();

            GameManager.Instance.SerProggresBarFill(0f);
        }
    }

    private void GenerateResourcesFromRooms()
    {

        foreach (KeyValuePair<Room, RoomData> entry in GameManager.Instance.RoomDataDictionary)
        {
            Room room = entry.Key;
            RoomData roomData = entry.Value;
            int antsInRoom = roomData.antsCount;

            int availableFood = GetResourceAmount(GameResourceType.Food);

            if (antsInRoom == 0)
            {
                continue;
            }

            if (room.GetRoomType() == RoomType.HatchingRoom)
            {
                int antsToGenerate = 0;
                int costForAnt = 1;

                bool canGenerateAnts = GameManager.Instance.CanAntsBeGenerated();
                bool haveFood = availableFood >= costForAnt;

                if (canGenerateAnts && haveFood)
                {
                    int maxPossibleAnts = availableFood / costForAnt;
                    antsToGenerate = Mathf.Min(antsInRoom, maxPossibleAnts);
                    RemoveResource(GameResourceType.Food, antsToGenerate * costForAnt);
                    AddResource(GameResourceType.Ant, antsToGenerate); 
                }

                if (room is HatchingRoom hatchingRoom)
                {
                    hatchingRoom.SetAntsGain(antsToGenerate, haveFood, canGenerateAnts);
                    hatchingRoom.ShowGain();
                }
            }
        }
    }

    internal int CountAllAnts()
    {
        int unusedAnts = GetResourceAmount(GameResourceType.Ant);
        int allAnts = 0;

        foreach (KeyValuePair<GridTile, GridTileData> entry in GameManager.Instance.TileDataDictionary)
        {
            GridTile gridTile = entry.Key;
            GridTileData tileData = entry.Value;

            allAnts += tileData.antsCount;
        }

        foreach (KeyValuePair<Room, RoomData> entry in GameManager.Instance.RoomDataDictionary)
        {
            Room room = entry.Key;
            RoomData roomData = entry.Value;
            if (room.GetRoomType() == RoomType.HatchingRoom)
            {
                allAnts += roomData.antsCount;
            }
        }

        this.allAnts = unusedAnts + allAnts;
        return this.allAnts;
    }

    internal void ShortGenerationDuration(float ammount)
    {
        generationDuration -= ammount;
        generationDuration = Mathf.Max(0.1f, generationDuration);

        Debug.Log($"tick ratio {generationDuration}");

    }
}



