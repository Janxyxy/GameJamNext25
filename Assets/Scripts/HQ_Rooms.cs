using System.Collections.Generic;
using UnityEngine;

public abstract class HQ_Rooms : MonoBehaviour
{
    public enum RoomType { Queen, GymMine, Nursery }

    // Properties
    public int AntCount { get; protected set; }
    public int UpgradeLvl { get; protected set; }
    public double Efficiency { get; protected set; }
    public RoomType Type { get; protected set; }

    [SerializeField]
    protected List<ResourceRequirement> materialsNeeded;

    // Abstract method to calculate efficiency based on upgrade level
    protected abstract void CalculateEfficiency();

    // Method to handle adding ants
    public void AddAnt()
    {
        if (ResourcesManager.Instance.CanRemoveAnt())
        {
            AntCount++;
            ResourcesManager.Instance.RemoveAnt();
        }
    }

    // Method to handle removing ants
    public void RemoveAnt()
    {
        if (AntCount > 0)
        {
            AntCount--;
            ResourcesManager.Instance.AddAnt();
        }
    }

    // Method to handle upgrading the room
    public void UpgradeRoom()
    {
        if (CanUpgrade())
        {
            foreach (var requirement in materialsNeeded)
            {
                ResourcesManager.Instance.RemoveResource(requirement.resourceType, requirement.amount);
            }
            UpgradeLvl++;
            CalculateEfficiency();
        }
    }

    // Method to check if the room can be upgraded
    private bool CanUpgrade()
    {
        foreach (var requirement in materialsNeeded)
        {
            if (!ResourcesManager.Instance.HasEnoughResource(requirement.resourceType, requirement.amount))
            {
                return false;
            }
        }
        return true;
    }

    // Method to display resource requirements
    public void DisplayRequirements()
    {
        foreach (var requirement in materialsNeeded)
        {
            bool hasEnough = ResourcesManager.Instance.HasEnoughResource(requirement.resourceType, requirement.amount);
            string color = hasEnough ? "green" : "red";
            Debug.Log($"<color={color}>{requirement.resourceType}: {requirement.amount}</color>");
        }
    }

    // Virtual method for room-specific behavior
    public virtual void OnClick()
    {
        // Default behavior (e.g., show UI)
    }
}

// Resource requirement struct
[System.Serializable]
public struct ResourceRequirement
{
    public ResourcesManager.GameResourceType resourceType;
    public int amount;
}