using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HatchingRoom : Room
{
    [Header("UI")]
    [SerializeField] private Button addAnts;
    [SerializeField] private Button removeAnts;

    [SerializeField] private TextMeshProUGUI hatchingRoomWorkers;
    [SerializeField] private TextMeshProUGUI gainTMP;

    private Color gainTMPdefaultColor;
    private Coroutine currentShowGainCoroutine;

    protected override void Awake()
    {
        addAnts.onClick.AddListener(OnAddAntsClick);
        removeAnts.onClick.AddListener(OnRemoveAntsClick);
        roomType = RoomType.HatchingRoom;
        RegisterRoom();
    }
    protected void OnAddAntsClick()
    {
        Debug.Log("Added Ants");
        GameManager.Instance.RemoveAntsFromRoom(this);
        WorkersChanged();
    }

    protected void OnRemoveAntsClick()
    {
        Debug.Log("Remove Ants");
        GameManager.Instance.AddAntsToRoom(this);
        WorkersChanged();
    }
    private void WorkersChanged()
    {
        foreach (var roomData in GameManager.Instance.RoomDataDictionary)
        {
            if (roomData.Key.GetRoomType() == RoomType.HatchingRoom)
            {
                hatchingRoomWorkers.text = $"Workers - {roomData.Value.antsCount}";
                Debug.Log($"Workers - {roomData.Value.antsCount}");
            }
        }
    }

    internal void SetAntsGain(float gain, bool haveFood, bool canGenerate)
    {
        if (!canGenerate)
        {
            gainTMP.color = Color.red;
            gainTMP.text = "Can't generate";
            return;
        }

        if (haveFood)
        {
            gainTMP.color = Color.white;
            gainTMP.text = gain == 1 ? "+ 1 ant" : $"+ {gain} ants";
        }
        else
        {
            gainTMP.color = Color.red;
            gainTMP.text = "No food";
        }
    }


    internal void ShowGain()
    {
        if (currentShowGainCoroutine != null)
        {
            StopCoroutine(currentShowGainCoroutine);
        }

        if (!isActiveAndEnabled)
            return;

        currentShowGainCoroutine = StartCoroutine(ShowGeneratedCount());
    }

    private IEnumerator ShowGeneratedCount()
    {
        // Set text and ensure it's active.
        gainTMP.gameObject.SetActive(true);
        gainTMPdefaultColor = gainTMP.color;

        // Use faster fade durations.
        float fadeInTime = 0.18f;
        float fadeOutTime = 0.18f;
        float holdTime = GameManager.Instance.GeneratedCountDuration; // Duration to hold full opacity.

        // Use the stored default color as the target color.
        Color originalColor = gainTMPdefaultColor;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Start fully transparent.
        gainTMP.color = transparentColor;

        // Fade in with ease-in (quadratic).
        float elapsed = 0f;
        while (elapsed < fadeInTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeInTime);
            float easeIn = t * t; // Quadratic ease-in.
            gainTMP.color = Color.Lerp(transparentColor, originalColor, easeIn);
            yield return null;
        }
        gainTMP.color = originalColor;

        // Hold at full opacity.
        yield return new WaitForSeconds(holdTime);

        // Fade out with ease-out (quadratic).
        elapsed = 0f;
        while (elapsed < fadeOutTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeOutTime);
            float easeOut = 1 - (1 - t) * (1 - t); // Quadratic ease-out.
            gainTMP.color = Color.Lerp(originalColor, transparentColor, easeOut);
            yield return null;
        }
        gainTMP.color = transparentColor;
        gainTMP.gameObject.SetActive(false);
    }
}
