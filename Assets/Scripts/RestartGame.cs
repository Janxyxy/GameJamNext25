using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.LoadScene("Name");
    }
}
