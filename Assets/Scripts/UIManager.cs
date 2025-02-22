using System;
using UnityEngine;
using UnityEngine.UI;
using static ResourcesManager;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image proggressBarFill;
    [SerializeField] private Transform managerUI;

    [Header("Navigation Button")]
    [SerializeField] private Button navigateButton;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite mapIcon;
    [SerializeField] private Sprite antHillIcon;


    public static UIManager Instance { get; private set; }
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

        navigateButton.onClick.AddListener(ChangeNavigation);
    }

    private void ChangeNavigation()
    {
       

        GameManager.Instance.ToggleMap();
        bool isMapOpen = GameManager.Instance.IsMapOpen;
        ShowManagerUI(isMapOpen);

        if(isMapOpen)
        {
            buttonImage.sprite = mapIcon;
        }
        else
        {
           buttonImage.sprite = antHillIcon;
        }
    }

    internal void SerProggresBarFill(float fillAmount)
    {
        proggressBarFill.fillAmount = fillAmount;
    }

    public void ShowManagerUI(bool show)
    {
        managerUI.gameObject.SetActive(show);

        if (!show)
        {
            Camera.main.gameObject.transform.position = new Vector3(0, 0, -1);
        }
    }

}
