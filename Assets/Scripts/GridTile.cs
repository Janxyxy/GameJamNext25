using UnityEngine;
using UnityEngine.UI;

public class GridTile : MonoBehaviour
{
    [SerializeField] private TileType tileType;

    private Button button;
    public enum TileType
    {
        HQ,
        Forest,
        Mountain,
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnTileClick);

        //ResourcesManager.Instance.AddGridTileData();
    }

    private void OnTileClick()
    {
        GameManager.Instance.OnTileClick(tileType);
    }
}
