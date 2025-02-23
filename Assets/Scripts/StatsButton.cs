using UnityEngine;

public class StatsButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        string highlight = "";
        if (PlayerPrefs.HasKey("username"))
        {
            highlight = "?highlight=" + PlayerPrefs.GetString("username");
        }
        Application.OpenURL("https://colony.nasypal.cz/" + highlight);
    }
}
