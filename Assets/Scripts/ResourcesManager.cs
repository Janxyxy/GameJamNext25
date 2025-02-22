using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform resourcesParent;
    [SerializeField] private ResourceUI resourcePrefab;

    public static ResourcesManager Instance { get; private set; }

    [SerializeField] private List<Sprite> resourcesSprites = new List<Sprite>();

    private Dictionary<GameResourceType, ResourceUI> resourceUIs = new Dictionary<GameResourceType, ResourceUI>();
    private Dictionary<GameResourceType, int> resources = new Dictionary<GameResourceType, int>();

    public enum GameResourceType
    {
        Wood,
        Stone
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

        ResourcesManager.Instance.AddResource(GameResourceType.Wood, 10);
        ResourcesManager.Instance.AddResource(GameResourceType.Stone, 5);

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
        else
        {
            resources[type] = amount;
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
            return true;
        }

        Debug.LogWarning($"Not enough {type} to remove!");
        return false;
    }

    public int GetResourceAmount(GameResourceType type)
    {
        return resources.ContainsKey(type) ? resources[type] : 0;
    }
}


