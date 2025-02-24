using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class TimerScript : MonoBehaviour
{
    private TMP_Text timerText;
    private int countdownTime;

    private void Start()
    {
        countdownTime = GameManager.Instance.CountdownTimeInSeconds;
        timerText = GetComponent<TMP_Text>();
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
