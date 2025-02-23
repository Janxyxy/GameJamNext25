using System;
using TMPro;
using UnityEngine;

public class HatchingRoom : Room
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI HatchingRoomWorkers;
    protected override void OnAddAntsClick()
    {
        GameManager.Instance.RemoveAntsFromRoom(this);
        WorkersChanged();
    }

    protected override void OnRemoveAntsClick()
    {
        GameManager.Instance.AddAntsToRoom(this);
        WorkersChanged();
    }
    private void WorkersChanged()
    {
        foreach (var roomData in GameManager.Instance.RoomDataDictionary)
        {
            if (roomData.Key.GetRoomType() == RoomType.HatchingRoom)
            {
                HatchingRoomWorkers.text = $"Workers - {roomData.Value.antsCount}";
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        roomType = RoomType.HatchingRoom;
    }
}
