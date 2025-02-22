using UnityEngine;

public class HQ_GymMineRoom : HQ_Rooms
{
    [SerializeField] private float gemMineChance = 0.1f; // 10% chance to mine a gem

    protected override void CalculateEfficiency()
    {
        Efficiency = 1.0 + (UpgradeLvl * 0.2); // Example efficiency calculation
    }

    private void Start()
    {
        Type = RoomType.GymMine;
        AntCount = 0;
        UpgradeLvl = 1;
        CalculateEfficiency();
    }

    public void MineResources()
    {
        if (AntCount > 0)
        {
            int minedResources = (int)(AntCount * Efficiency);
            ResourcesManager.Instance.AddResource(ResourcesManager.GameResourceType.Stone, minedResources);

            // Check for gem mining
            if (Random.value < gemMineChance)
            {
                ResourcesManager.Instance.AddResource(ResourcesManager.GameResourceType.Gem, 1);
                Debug.Log("You mined a gem!");
            }
        }
    }
}