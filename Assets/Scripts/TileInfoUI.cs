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

    [SerializeField] private Image proggressBar;

    private void Awake()
    {
        addAntButton.onClick.AddListener(OnAddAntClick);
        removeAntButton.onClick.AddListener(OnRemoveAntClick);
    }

    private void OnRemoveAntClick()
    {
        int mult = GameManager.Instance.EditMultiplier;

        bool removed = GameManager.Instance.RemoveAntFromCurrentTile(mult);
        if (removed)
        {
            ResourcesManager.Instance.AddResource(ResourcesManager.GameResourceType.Ant, GameManager.Instance.EditMultiplier);
        }
    }

    private void OnAddAntClick()
    {
        int mult = GameManager.Instance.EditMultiplier;

        bool added = ResourcesManager.Instance.RemoveResource(ResourcesManager.GameResourceType.Ant, mult);
        if(added)
        {
            GameManager.Instance.AddAntToCurrentTile(mult);
        }
    }

    internal void SetUI(string name, string description)
    {
        this.tileName.text = name;
        this.description.text = description;
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

    internal void SerProggresBarFill(float fillAmount)
    {
        proggressBar.fillAmount = fillAmount;
    }
}
