using System.Resources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GymRoom : Room
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI UpgradeCost;
    [SerializeField] private TextMeshProUGUI GymRoomName;
    [SerializeField] private GameObject hideUpgrade;

    private int maxlvl = 7;

    private int cost = 10;
    private int lvl = 1;

    [SerializeField] private Button UpgradeButton;


    private void Start()
    {
        UpgradeCost.text = $"{cost}";
        GymRoomName.text = $"Gym Room lvl {lvl}";
    }

    protected override void Awake()
    {
        if (UpgradeButton != null)
        {
            UpgradeButton.onClick.AddListener(UpgradeMultiplier);
        }
        else
        {
            Debug.LogError("UpgradeButton is not assigned!");
        }
    }

    private void UpgradeMultiplier()
    {

        if (ResourcesManager.Instance.GetResourceAmount(ResourcesManager.GameResourceType.Gem) >= cost)
        {
            ResourcesManager.Instance.UpgradeMultiplier(cost);
            cost = cost * 2;

            lvl++;
            if (lvl < maxlvl) {
            GymRoomName.text = $"Gym Room lvl {lvl}";

            }
            else if (lvl >= maxlvl)
            {
                
                GymRoomName.text = $"Gym Room Max";
                hideUpgrade.SetActive(false);
            }

            UpgradeCost.text = $"{cost}";

        }
        else
        {
            Debug.Log("Not enough gems");
        }


    }
}

