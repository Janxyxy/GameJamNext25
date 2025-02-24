
using System;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GymRoom : Room
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI upgradeCostTmp;
    [SerializeField] private TextMeshProUGUI gymRoomNameTMP;
    [SerializeField] private TextMeshProUGUI generationDurationTMP;

    private int upgradeCost = 10;
    private int maxlvl = 10;
    private int lvl = 1;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Image upgradeButtonImage;
    [SerializeField] private Color canUpgradeColor;
    private Color defaultColor;

    private void Start()
    {
        upgradeCostTmp.text = $"Cost -<color=#319BD0>{upgradeCost}</color>\r\n";
        gymRoomNameTMP.text = $"Gym Room - {lvl}";

        defaultColor = upgradeButtonImage.color;
    }

    protected override void Awake()
    {
        roomType = RoomType.GymRoom;
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(UpgradeMultiplier);
        }
        else
        {
            Debug.LogError("UpgradeButton is not assigned!");
        }
        UpdateGenerationDurationTMP();
    }

    private void UpgradeMultiplier()
    {
        if (ResourcesManager.Instance.HasEnoughResource(ResourcesManager.GameResourceType.Gem, upgradeCost))
        {
            if (lvl == maxlvl)
            {
                return;
            }

            ResourcesManager.Instance.RemoveResource(ResourcesManager.GameResourceType.Gem, upgradeCost);
            ResourcesManager.Instance.ShortGenerationDuration(0.2f);


            upgradeCost = upgradeCost * 2;

            lvl++;

            if (lvl < maxlvl)
            {
                gymRoomNameTMP.text = $"Gym Room lvl {lvl}";
                upgradeCostTmp.text = $"Cost - <color=#319BD0>{upgradeCost}</color>\r";
            }
            else if (lvl == maxlvl)
            {
                gymRoomNameTMP.text = $"Gym Room lvl {lvl} (MAX)";
                upgradeCostTmp.text = $"Cost - MAX";
            }

            Debug.Log("Updating generation duration TMP...");
            UpdateGenerationDurationTMP();
        }
        else
        {
            Debug.Log("Not enough gems");
        }

        Debug.Log("UpgradeMultiplier finished");
        
    }


    private void HandleResourceChanged(ResourcesManager.GameResourceType type)
    {
        if (type == ResourcesManager.GameResourceType.Gem)
        {
            UpgradeButtonCheck();
        }
    }

    internal void UpgradeButtonCheck()
    {

        if (upgradeButtonImage == null)
        {
            Debug.LogWarning(upgradeButtonImage);
            return;
        }

        Debug.Log("Checking upgrade button...");

        if (ResourcesManager.Instance.GetResourceAmount(ResourcesManager.GameResourceType.Gem) >= upgradeCost)
        {
            upgradeButtonImage.color = canUpgradeColor;
        }
        else
        {
            upgradeButtonImage.color = defaultColor;
        }
    }

    private void UpdateGenerationDurationTMP()
    {
        generationDurationTMP.text = $"Work cycle duration - {ResourcesManager.Instance.GenerationDuration}s";
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

