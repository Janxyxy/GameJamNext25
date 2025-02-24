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


    [Header("Data")]
    [SerializeField] private Transform dataSend;

    [Header("Help")]
    [SerializeField] private Button helpBbutton;
    [SerializeField] private Button closeHelpButton;
    [SerializeField] private Transform helpTransform;

    [Header("Dev")]
    [SerializeField] private Transform devCanvas;


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

        helpBbutton.onClick.AddListener(() => ShowHelp(true));
        closeHelpButton.onClick.AddListener(() => ShowHelp(false));
    }
    private void Start()
    {
        //DevCanvas();
        ShowHelp(true);
        antHillCanvas.gameObject.SetActive(true);
    }

    private void ShowHelp(bool show)
    {
        helpTransform.gameObject.SetActive(show);
        if (show)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
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

        //antHillCanvas.gameObject.SetActive(!show);
        managerUI.gameObject.SetActive(show);

        helpBbutton.gameObject.SetActive(show);

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

    internal void DevCanvas()
    {
        devCanvas.gameObject.SetActive(GameManager.Instance.DevMode);
    }
}
