using UnityEngine;

public class UrlButton : MonoBehaviour
{
    [SerializeField] private string url;
    private void OnMouseDown()
    {
        Application.OpenURL(url);
    }
}
