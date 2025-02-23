using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using static GridTile;

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

    [SerializeField] private Transform buttons;

    [SerializeField] private AudioClip addAnt;
    private AudioSource audioSource;


    private void Awake()
    {
        addAntButton.onClick.AddListener(OnAddAntClick);
        removeAntButton.onClick.AddListener(OnRemoveAntClick);
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnRemoveAntClick()
    {
        int mult = GameManager.Instance.EditMultiplier;

        if (GameManager.Instance.CurrentTile.tileType == TileType.None)
        {
            return;
        }

        bool removed = GameManager.Instance.RemoveAntFromCurrentTile(mult);
        if (removed)
        {
            ResourcesManager.Instance.AddResource(ResourcesManager.GameResourceType.Ant, GameManager.Instance.EditMultiplier);
            audioSource.PlayOneShot(addAnt);
        }
    }

    private void OnAddAntClick()
    {
        int mult = GameManager.Instance.EditMultiplier;

        if(GameManager.Instance.CurrentTile.tileType == TileType.None)
        {
            return;
        }

        bool added = ResourcesManager.Instance.RemoveResource(ResourcesManager.GameResourceType.Ant, mult);
        if (added)
        {
            GameManager.Instance.AddAntToCurrentTile(mult);
            audioSource.PlayOneShot(addAnt);
        }
    }

    internal void SetUI(string name, string description)
    {
        this.tileName.text = name;
        this.description.text = $"- {description}";
    }

    internal void SetAntCount(int count)
    {
        Debug.Log($"Count {count}");
        antsCount.text = count.ToString();
    }

    internal void SetNormalTileUI(TileType tileType)
    {
        if (tileType == TileType.Anthill)
        {
            normalTileUI.gameObject.SetActive(false);
            antHillTileUI.gameObject.SetActive(true);
        }
        else
        {
            normalTileUI.gameObject.SetActive(true);
            antHillTileUI.gameObject.SetActive(false);

            if(tileType == TileType.None)
            {
                buttons.gameObject.SetActive(false);
            }
            else
            {
                buttons.gameObject.SetActive(true);
            }
        }

      
    }
}
