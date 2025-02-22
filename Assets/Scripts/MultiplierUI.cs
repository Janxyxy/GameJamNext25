using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierUI : MonoBehaviour
{
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private List<int> mult = new List<int>();

    private Button currentButton;

    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }

        buttons[0].onClick.Invoke();
    }

    private void OnButtonClick(int index)
    {
        GameManager.Instance.SetEditMultiplier(mult[index]);

        if (currentButton != null)
        {
            Image oldImage = currentButton.GetComponent<Image>();
            oldImage.color = Color.white;
        }

        currentButton = buttons[index];

        Image newImage = currentButton.GetComponent<Image>();
        newImage.color = Color.green;
    }
}
