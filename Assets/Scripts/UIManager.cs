using System;
using System.Collections;
using System.Data;
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

    [Header("TacticalView")]
    [SerializeField] private Button tacticalViewButton;
    [SerializeField] private Image tacticalViewImage;
    [SerializeField] private Sprite tacticalIcon;
    [SerializeField] private Sprite gainIcon;

    [Header("Queen Quest Button")]
    [SerializeField] private Button queenPopUpButton;

    [Header("Queen Quest Window")]

    [SerializeField] private GameObject questPopUp;
    [SerializeField] private Button exitWindow;
    [SerializeField] private TextMeshProUGUI questNeeds;

    [Header("Data")]
    [SerializeField] private Transform dataSend;



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

        questPopUp.SetActive(false);
        queenPopUpButton.onClick.AddListener(OpenQuestWindow);
        exitWindow.onClick.AddListener(ExitQuestWindow);
    }

    private void ExitQuestWindow()
    {
        questPopUp.SetActive(false);

    }

    private void OpenQuestWindow()
    {
        if (!questPopUp.activeSelf) // Check if the questPopUp is currently inactive
        {
            questPopUp.SetActive(true); // Activate the questPopUp
        }
        else
        {
            questPopUp.SetActive(false);

        }
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

    private void ShowDataSend(bool send)
    {
        dataSend.gameObject.SetActive(send);
    }

    internal void DataSendOK()
    {
        StartCoroutine(DataSendOKCoroutine());
    }

    private IEnumerator DataSendOKCoroutine()
    {
        ShowDataSend(true);
        Image img = dataSend.GetComponentInChildren<Image>();

        Color initialColor = img.color;
        Color targetColor = Color.green;

        float fadeDuration = 1f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            img.color = Color.Lerp(initialColor, targetColor, timer / fadeDuration);
            yield return null;
        }
        img.color = targetColor; 

        yield return new WaitForSeconds(2f);

        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            img.color = Color.Lerp(targetColor, initialColor, timer / fadeDuration);
            yield return null;
        }
        img.color = initialColor;

        ShowDataSend(false);
    }

}
