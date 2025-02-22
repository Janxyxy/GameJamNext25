[System.Serializable]
public class RoomData
{
    public int antsCount;

    public void AddAnt(int count)
    {
        antsCount += count;
    }

    public bool RemoveAnt(int count)
    {
        if (antsCount >= count)
        {
            antsCount -= count;
            return true;
        }
        return false;
    }
}
