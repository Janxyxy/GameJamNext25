using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QueenRooom : Room
{
    [Header("UI")]
    [SerializeField] private Button SubmitButton;
    [SerializeField] private Image submitButtonImage;

    [SerializeField] private TextMeshProUGUI questTime;
    [SerializeField] private TextMeshProUGUI woodRequested;
    [SerializeField] private TextMeshProUGUI stoneRequested;
    [SerializeField] private Transform requestUI;
    [SerializeField] private Transform hatchingInfo;

    private bool isQuestActive = false;
    private int questsCompleted = 0;

    private int requestedWood;
    private int requestedStone;

    private float timeToNextQuest;

    protected override void Awake()
    {
        roomType = RoomType.QueenRoom;
    }
    void Start()
    {
        SubmitButton.onClick.AddListener(SubmitQuest);
        StartCoroutine(GenerateQuests());

    }

    private void UpdateRequestUI()
    {
        requestUI.gameObject.SetActive(isQuestActive);
        hatchingInfo.gameObject.SetActive(isQuestActive);
    }

    private void SubmitQuest()
    {
        if (isQuestActive)
        {
            if (ResourcesManager.Instance.HasEnoughResource(ResourcesManager.GameResourceType.Wood, requestedWood)
                && ResourcesManager.Instance.HasEnoughResource(ResourcesManager.GameResourceType.Stone, requestedStone))
            {
                ResourcesManager.Instance.RemoveResource(ResourcesManager.GameResourceType.Wood, requestedWood);
                ResourcesManager.Instance.RemoveResource(ResourcesManager.GameResourceType.Stone, requestedStone);

                questsCompleted++;

                isQuestActive = false;
                UpdateRequestUI();
                ChangeSubmitButton();
                GameManager.Instance.SetCanBeAntsGenerated(true);
                SubmitButton.gameObject.SetActive(false);
                StartCoroutine(GenerateQuests());
            }
            else
            {
                Debug.Log("QQ - Not enough resources");
            }
        }
    }

    private void HandleResourceChanged(ResourcesManager.GameResourceType type)
    {
        ChangeSubmitButton();
    }

    private void ChangeSubmitButton()
    {
        if (!submitButtonImage.gameObject.activeInHierarchy)
        {
            return;
        }

        if (ResourcesManager.Instance.HasEnoughResource(ResourcesManager.GameResourceType.Wood, requestedWood)
               && ResourcesManager.Instance.HasEnoughResource(ResourcesManager.GameResourceType.Stone, requestedStone))
        {
            submitButtonImage.color = Color.green;
        }
        else
        {
            submitButtonImage.color = Color.red;
        }
    }

    private IEnumerator GenerateQuests()
    {
        timeToNextQuest = UnityEngine.Random.Range(GameManager.Instance.QueenQuestMin, GameManager.Instance.QueenQuestMax);
        Debug.Log(timeToNextQuest);
        UpdateRequestUI();

        while (timeToNextQuest > 0)
        {
            yield return null;
            timeToNextQuest -= Time.deltaTime;

            if (questTime.gameObject.activeInHierarchy)
            {
                questTime.text = $"Next quest in - {Mathf.CeilToInt(timeToNextQuest)}s";
            }
        }

        SubmitButton.gameObject.SetActive(true);
        GenerateQuest();
        UpdateRequestUI();
    }



    private void GenerateQuest()
    {
        int difficultyMultiplier = Mathf.Max(1, Mathf.RoundToInt(Mathf.Pow(1.2f, questsCompleted + 1)));
        requestedStone = UnityEngine.Random.Range(40, 80) * difficultyMultiplier;
        requestedWood = UnityEngine.Random.Range(40, 80) * difficultyMultiplier;

        if (questTime.gameObject.activeInHierarchy)
        {
            questTime.text = "Quest active";
        }

        woodRequested.text = $"Wood - {requestedWood}";
        stoneRequested.text = $"Stone - {requestedStone}";

        isQuestActive = true;
        GameManager.Instance.SetCanBeAntsGenerated(false);
    }

    private void OnEnable()
    {
        ResourcesManager.Instance.OnResourceChanged += HandleResourceChanged;
    }

    private void OnDestroy()
    {
        ResourcesManager.Instance.OnResourceChanged -= HandleResourceChanged;
    }
}
