using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resourceCount;
    [SerializeField] private Image resourceImage;

    internal void SetCount(int count)
    {
        resourceCount.text = count.ToString();
    }

    internal void SetImage(Sprite sprite)
    {
        resourceImage.sprite = sprite;
    }
}
