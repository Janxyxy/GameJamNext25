using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridTile;

public class ResourcesManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform resourcesParent;
    [SerializeField] private ResourceUI resourcePrefab;

    [Header("Settings")]
    [SerializeField] private float generationDuration = 1.5f; //in seconds

    public static ResourcesManager Instance { get; private set; }

    private List<Sprite> resourcesSprites = new List<Sprite>();

    private Dictionary<GameResourceType, ResourceUI> resourceUIs = new Dictionary<GameResourceType, ResourceUI>();
    private Dictionary<GameResourceType, int> resources = new Dictionary<GameResourceType, int>();

    public Dictionary<GameResourceType, int> GameResources => resources;

    public enum GameResourceType
    {
        Wood,
        Stone,
        Food,
        Ant,
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

    public void AddResource(GameResourceType type, int amount)
    {
        if (resources.ContainsKey(type))
        {
            resources[type] += amount;
        }

        Debug.Log($"Added {amount} {type}. Total: {resources[type]}");

        // Update count in UI
        resourceUIs[type].SetCount(resources[type]);
    }

    public bool RemoveResource(GameResourceType type, int amount)
    {
        if (resources.ContainsKey(type) && resources[type] >= amount)
        {
            resources[type] -= amount;
            Debug.Log($"Removed {amount} {type}. Remaining: {resources[type]}");
            // Update count in UI
            resourceUIs[type].SetCount(resources[type]);

            return true;
        }

        Debug.LogWarning($"Not enough {type} to remove!");
        return false;
    }

    public int GetResourceAmount(GameResourceType type)
    {
        return resources.ContainsKey(type) ? resources[type] : 0;
    }

    public bool HasEnoughResource(GameResourceType type, int amount)
    {
        return resources.ContainsKey(type) && resources[type] >= amount;
    }

    // Add ants
    public void AddAnt()
    {
        AddResource(GameResourceType.Ant, 1);
    }

    // Add multiple ants
    public void AddAnts(int count)
    {
        AddResource(GameResourceType.Ant, count);
    }

    // Remove ants
    public bool RemoveAnt()
    {
        return RemoveResource(GameResourceType.Ant, 1);
    }

    // Check if an ant can be removed
    public bool CanRemoveAnt()
    {
        return HasEnoughResource(GameResourceType.Ant, 1);
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

            foreach (KeyValuePair<GridTile, GridTileData> entry in GameManager.Instance.TileDataDictionary)
            {
                GridTile gridTile = entry.Key;
                GridTileData tileData = entry.Value;

                if (tileData.antsCount == 0)
                {
                    continue;
                }

                int availableFood = GetResourceAmount(GameResourceType.Food);
                Debug.Log($"Available food: {availableFood}");

                int ants = tileData.antsCount;


                if (tileData.tileType == TileType.Meadow)
                {
                    AddResource(GameResourceType.Food, ants);
                    gridTile.ShowGeneratedCountWrapper(ants);
                }
                else if (tileData.tileType == TileType.Forest)
                {
                    if (availableFood > 0)
                    {
                        // Generate as many wood units as food permits.
                        int woodToGenerate = Mathf.Min(ants, availableFood);
                        RemoveResource(GameResourceType.Food, woodToGenerate);
                        AddResource(GameResourceType.Wood, woodToGenerate);

                        gridTile.ShowGeneratedCountWrapper(woodToGenerate);
                    }

                }
                else if (tileData.tileType == TileType.Mountain)
                {
                    if (availableFood > 0)
                    {
                        // Generate as many stone units as food permits.
                        int stoneToGenerate = Mathf.Min(ants, availableFood);
                        RemoveResource(GameResourceType.Food, stoneToGenerate);
                        AddResource(GameResourceType.Stone, stoneToGenerate);

                        gridTile.ShowGeneratedCountWrapper(stoneToGenerate);
                    }

                }

            }
            GameManager.Instance.SerProggresBarFill(0f);
        }
    }

}



