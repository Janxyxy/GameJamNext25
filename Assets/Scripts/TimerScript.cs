using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private float countdownTime = 300f;
    [SerializeField] private TMP_Text timerText;

    private void Start()
    {
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (countdownTime > 0)
        {
            UpdateTimerUI();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        SceneManager.LoadScene("End Screen");
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(countdownTime / 60);
        int seconds = Mathf.FloorToInt(countdownTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
