using System;
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

    [Header("TacticalView")]
    [SerializeField] private Button tacticalViewButton;
    [SerializeField] private Image tacticalViewImage;
    [SerializeField] private Sprite tacticalIcon;
    [SerializeField] private Sprite gainIcon;

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
        tacticalViewButton.onClick.AddListener(ChangeTacticalView);
    }

    private void ChangeNavigation()
    {
        GameManager.Instance.ToggleMap();
        bool isMapOpen = GameManager.Instance.IsMapOpen;
        ShowManagerUI(isMapOpen);
    }
    private void ChangeTacticalView()
    {
        GameManager.Instance.ToggleTacticalView();
        bool isTacticalViewOpen = GameManager.Instance.TacticalView;
        ChangeTacticalButtonIcon(isTacticalViewOpen);

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

    private void ChangeTacticalButtonIcon(bool isTacticalViewOpen)
    {
        if (isTacticalViewOpen)
        {
            tacticalViewImage.sprite = tacticalIcon;
        }
        else
        {
            tacticalViewImage.sprite = gainIcon;
        }
    }

}
