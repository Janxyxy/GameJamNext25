using UnityEngine;

public class GridTile : MonoBehaviour
{
    [SerializeField] private TileType tileType;
    private enum TileType
    {
        HQ,
        Forest,
        Mountain,
    }
}
