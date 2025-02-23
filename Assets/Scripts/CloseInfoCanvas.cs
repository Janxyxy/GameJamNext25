using UnityEngine;

public class CloseInfoCanvas : MonoBehaviour
{
    [SerializeField] private GameObject infoCanvas;

    public void CloseCanvas()
    {
        infoCanvas.SetActive(false);
    }
}
