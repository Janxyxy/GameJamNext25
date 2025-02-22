using TMPro;
using UnityEngine;

public class TileInfoUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI tileName;
    [SerializeField] private TextMeshProUGUI description;
   
    internal void SetUI(string name, string description)
    {
        this.tileName.text = name;
        this.description.text = description;
    }
}
