using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Room : MonoBehaviour
{
    protected RoomType roomType = RoomType.None;

    public enum RoomType
    {
        None,
        HatchingRoom,
        GymRoom,
        QueenRoom
    }

    protected virtual void Awake()
    {
        RegisterRoom();
    }

    protected void RegisterRoom()
    {
        GameManager.Instance.RegisterRoom(this);
    }

    internal RoomType GetRoomType()
    {
        return roomType;
    }
}
