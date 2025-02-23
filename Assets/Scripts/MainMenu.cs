using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button play;
    [SerializeField] private Button exit;

    private void Start()
    {
        play.onClick.AddListener(Play);
        exit.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void Play()
    {
        SceneManager.LoadScene("Name");
    }
}
