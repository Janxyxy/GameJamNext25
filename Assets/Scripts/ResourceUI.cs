using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resourceCount;
    [SerializeField] private Image resourceImage;

    internal void SetName(string name)
    {
        resourceCount.text = name;
    }

    internal void SetCount(int count)
    {
        resourceCount.text = count.ToString();
    }
}
