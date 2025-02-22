using UnityEngine;
using System.Collections;

public class HQ_NurseryRoom : HQ_Rooms
{
    [SerializeField] private float breedingInterval = 5.0f; // Time between breeding cycles
    [SerializeField] private int antsPerCycle = 1; // Number of ants bred per cycle

    protected override void CalculateEfficiency()
    {
        Efficiency = 1.0 + (UpgradeLvl * 0.15); // Example efficiency calculation
    }

    private void Start()
    {
        Type = RoomType.Nursery;
        AntCount = 0;
        UpgradeLvl = 1;
        CalculateEfficiency();

        StartCoroutine(BreedAnts());
    }

    private IEnumerator BreedAnts()
    {
        while (true)
        {
            yield return new WaitForSeconds(breedingInterval);
            if (AntCount > 0)
            {
                int newAnts = (int)(antsPerCycle * Efficiency);
                ResourcesManager.Instance.AddAnts(newAnts);
                Debug.Log($"Bred {newAnts} ants!");
            }
        }
    }
}