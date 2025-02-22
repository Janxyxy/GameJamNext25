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

       bool removed = GameManager.Instance.RemoveAntFromCurrentTile();
        if (removed)
        {
            ResourcesManager.Instance.AddResource(ResourcesManager.GameResourceType.Ant, 1);
        }
    }

    private void OnAddAntClick()
    {
        bool added = ResourcesManager.Instance.RemoveResource(ResourcesManager.GameResourceType.Ant, 1);
        if(added)
        {
            GameManager.Instance.AddAntToCurrentTile();
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
