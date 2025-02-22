using System;
using UnityEngine;
using UnityEngine.UI;

public class GridTile : MonoBehaviour
{
    [SerializeField] private TileType tileType;
    [SerializeField] private bool isAntHill = false; // HQ is only one

    [Header("UI")]
    [SerializeField] private Image tileIconImage;

    private Button button;
    public enum TileType
    {
        None,
        Anthill,
        Forest,
        Mountain,
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnTileClick);
    }

    private void Start()
    {
        SetRandomTile();
        SetTileIcon();
    }

    private void SetTileIcon()
    {
        Sprite tileIcon = GameManager.Instance.GetTileIcon(tileType);

        if (tileIcon != null && tileIconImage != null)
            tileIconImage.sprite = tileIcon;
    }

    private void SetRandomTile()
    {
        if(isAntHill)
        {
            tileType = TileType.Anthill;
            GameManager.Instance.OnTileClick(tileType, this);
            return;
        }
        else
        {
            // skip none ant anthill
            tileType = (TileType)UnityEngine.Random.Range(2, Enum.GetValues(typeof(TileType)).Length);
        }
    }

    private void OnTileClick()
    {
        GameManager.Instance.OnTileClick(tileType, this);
    }
}
