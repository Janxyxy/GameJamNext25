using TMPro;
using UnityEngine;

public class UsernameHelper : MonoBehaviour
{
    [SerializeField] private TMP_InputField username_field;
    [SerializeField] private TMP_Text invalid_input;
    public void LoadGameScene()
    {
        string username = username_field.text;
        if (string.IsNullOrEmpty(username))
        {
            invalid_input.text = "Username can't be empty!";
            invalid_input.enabled = true;
            return;
        }
        PlayerPrefs.SetString("username", username);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Janxyxy");
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            username_field.text = PlayerPrefs.GetString("username");
        }
    }
}
