using UnityEngine;

public class HQ_QueenRoom : HQ_Rooms
{
    protected override void CalculateEfficiency()
    {
        // Queen Room doesn't use efficiency
        Efficiency = 1.0;
    }

    private void Start()
    {
        Type = RoomType.Queen;
        AntCount = 0; // Queen Room doesn't need ants
        UpgradeLvl = 1;
    }

    public override void OnClick()
    {
        base.OnClick(); // Optional: Call base behavior if needed
        GiveQuest();
    }

    private void GiveQuest()
    {
        // Generate a random quest
        string quest = GenerateRandomQuest();
        Debug.Log($"Queen gives you a quest: {quest}");
        // Show quest in UI (implement UI logic here)
    }

    private string GenerateRandomQuest()
    {
        // Example quests
        string[] quests = {
            "Collect 100 Wood",
            "Breed 10 Ants",
            "Upgrade the Gym Room",
            "Mine 5 Gems"
        };
        return quests[Random.Range(0, quests.Length)];
    }
}