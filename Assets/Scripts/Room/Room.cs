using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Room : MonoBehaviour
{
    [SerializeField] private Button addAnts;
    [SerializeField] private Button removeAnts;

    protected RoomType roomType;

    public enum RoomType
    {
        HatchingRoom,
        GymRoom
    }

    protected virtual void Awake()
    {
        addAnts.onClick.AddListener(OnAddAntsClick);
        removeAnts.onClick.AddListener(OnRemoveAntsClick);

        RegisterRoom();
    }

    private void RegisterRoom()
    {
        GameManager.Instance.RegisterRoom(this);
    }

    protected virtual void OnRemoveAntsClick()
    {
       
    }

    protected virtual void OnAddAntsClick()
    {
      
    }

    public RoomType GetRoomType()
    {
        return roomType;
    }
}
