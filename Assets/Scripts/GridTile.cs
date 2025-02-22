using System;
using UnityEngine;
using UnityEngine.UI;

public class GridTile : MonoBehaviour
{
    [SerializeField] private TileType tileType;
    [SerializeField] private bool isAntHill = false; // HQ is only one

    private Button button;
    public enum TileType
    {
        None,
        AntHill,
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
    }

    private void SetRandomTile()
    {
        if(isAntHill)
        {
            tileType = TileType.AntHill;
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
