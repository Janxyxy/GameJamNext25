using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ResourcesManager;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image proggressBarFill;
    [SerializeField] private Transform managerUI;
    [SerializeField] private Transform antHillCanvas;

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


    }

    internal void SerProggresBarFill(float fillAmount)
    {
        proggressBarFill.fillAmount = fillAmount;
    }

    public void ShowManagerUI(bool show)
    {
        if (!show)
        {
            //SmoothCameraScroll SCS = Camera.main.GetComponent<SmoothCameraScroll>();
            //SCS.ResetCamera();
        }

        antHillCanvas.gameObject.SetActive(!show);
        managerUI.gameObject.SetActive(show);

        if (show)
        {
            buttonImage.sprite = antHillIcon;
        }
        else
        {
            buttonImage.sprite = mapIcon;
        }
    }

}
