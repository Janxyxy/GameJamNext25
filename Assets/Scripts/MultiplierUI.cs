using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierUI : MonoBehaviour
{
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private List<int> mult = new List<int>();

    private void Awake()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    private void OnButtonClick(int index)
    {
        GameManager.Instance.SetEditMultiplier(mult[index]);
    }
}
