using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TileInfoUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI tileName;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private TextMeshProUGUI antsCount;

    [SerializeField] private Button addAntButton;
    [SerializeField] private Button removeAntButton;

    [SerializeField] private Transform normalTileUI;
    [SerializeField] private Transform antHillTileUI;



    private void Awake()
    {
        addAntButton.onClick.AddListener(OnAddAntClick);
        removeAntButton.onClick.AddListener(OnRemoveAntClick);
    }

    private void OnRemoveAntClick()
    {
        int mult = GameManager.Instance.EditMultiplier;

        if (GameManager.Instance.GetTileTypeOfCurrentTile() == GridTile.TileType.None)
        {
            bool removed = GameManager.Instance.RemoveAntFromCurrentTile(mult, true);
            if (removed)
            {
                ResourcesManager.Instance.AddResource(ResourcesManager.GameResourceType.SpecialAnt, GameManager.Instance.EditMultiplier);
            }
        }
        else
        {
            bool removed = GameManager.Instance.RemoveAntFromCurrentTile(mult, false);
            if (removed)
            {
                ResourcesManager.Instance.AddResource(ResourcesManager.GameResourceType.Ant, GameManager.Instance.EditMultiplier);
            }
        }
    }

    private void OnAddAntClick()
    {
        int mult = GameManager.Instance.EditMultiplier;

        if (GameManager.Instance.GetTileTypeOfCurrentTile() == GridTile.TileType.None)
        {
            bool added = ResourcesManager.Instance.RemoveResource(ResourcesManager.GameResourceType.SpecialAnt, mult);
            if (added)
            {
                GameManager.Instance.AddAntToCurrentTile(mult, true);
            }
        }
        else
        {
            bool added = ResourcesManager.Instance.RemoveResource(ResourcesManager.GameResourceType.Ant, mult);
            if (added)
            {
                GameManager.Instance.AddAntToCurrentTile(mult, false);
            }
        }

    }

    internal void SetUI(string name, string description)
    {
        this.tileName.text = name;
        this.description.text = $"- {description}";
    }

    internal void SetAntCount(int count)
    {
        antsCount.text = count.ToString();
    }

    internal void SetNormalTileUI(bool active)
    {
        normalTileUI.gameObject.SetActive(active);
        antHillTileUI.gameObject.SetActive(!active);
    }
}
