using System;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private  AudioSource bgMusic;
    private float[] samples = new float[1024];
    private float lastRMS = 0f;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public float DetectDrop()
    {
        if (!bgMusic || !bgMusic.isPlaying) return 0f;

        // Získání aktuálních audio vzorkù
        bgMusic.GetOutputData(samples, 0);

        // Výpoèet RMS (prùmìrné hlasitosti)
        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }
        float currentRMS = Mathf.Sqrt(sum / samples.Length);

        // Vypoèítáme zmìnu hlasitosti
        float dropIntensity = Mathf.Clamp01((lastRMS - currentRMS) * 10f); // Posílení zmìny

        // Aktualizujeme poslední hodnotu RMS
        lastRMS = currentRMS;

        return dropIntensity;
    }

    internal void PlayClickSound()
    {
        //clickAudio.Play();
    }
}
