using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GridTile;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TileInfoUI tileInfoUI;

    [SerializeField] private List<TileSO> tileInfos = new List<TileSO>();

    public static GameManager Instance { get; private set; }

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

    internal void OnTileClick(TileType tileType)
    {
        for(int i = 0; i < tileInfos.Count; i++)
        {
            if (tileInfos[i].name == tileType.ToString())
            {
                tileInfoUI.SetUI(tileInfos[i].name, tileInfos[i].description);
                break;
            }
        }
    }
}
