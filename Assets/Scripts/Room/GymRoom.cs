using UnityEngine;

public class GymRoom : Room
{
    protected override void OnAddAntsClick()
    {
        GameManager.Instance.RemoveAntsFromRoom(this);
    }

    protected override void OnRemoveAntsClick()
    {
        GameManager.Instance.AddAntsToRoom(this);
    }

    protected override void Awake()
    {
        base.Awake();
        roomType = RoomType.GymRoom;
    }
}
