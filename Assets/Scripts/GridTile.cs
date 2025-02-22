using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridTile : MonoBehaviour
{
    [SerializeField] private TileType tileType;
    [SerializeField] private bool isAntHill = false; // HQ is only one

    [Header("UI")]
    [SerializeField] private Image tileIconImage;
    [SerializeField] private TextMeshProUGUI generatedCount;
    [SerializeField] private float generatedCountDuration;

    private Coroutine currentShowCountCoroutine;
    private Color defaultColor;

    private Button button;
    public enum TileType
    {
        None,
        Anthill,
        Forest,
        Mountain,
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnTileClick);

        defaultColor = generatedCount.color;
    }

    private void Start()
    {
        SetRandomTile();
        SetTileIcon();
    }

    private void SetTileIcon()
    {
        Sprite tileIcon = GameManager.Instance.GetTileIcon(tileType);

        if (tileIcon != null && tileIconImage != null)
            tileIconImage.sprite = tileIcon;
    }

    private void SetRandomTile()
    {
        if (isAntHill)
        {
            tileType = TileType.Anthill;
            GameManager.Instance.OnTileClick(tileType, this);
            return;
        }
        else
        {
            // skip none ant anthill
            tileType = (TileType)UnityEngine.Random.Range(2, Enum.GetValues(typeof(TileType)).Length);
        }

        GameManager.Instance.RegisterTile(tileType, this);
    }

    private void OnTileClick()
    {
        GameManager.Instance.OnTileClick(tileType, this);
    }

    internal void ShowGeneratedCountWrapper(int antsCount)
    {
        if (currentShowCountCoroutine != null)
        {
            StopCoroutine(currentShowCountCoroutine);
        }
        currentShowCountCoroutine = StartCoroutine(ShowGeneratedCount(antsCount));
    }

    private IEnumerator ShowGeneratedCount(int antsCount)
    {
        if (antsCount <= 0)
        {
            yield break;
        }

        // Set text and ensure it's active.
        generatedCount.text = $"+{antsCount}";
        generatedCount.gameObject.SetActive(true);

        // Use faster fade durations.
        float fadeInTime = 0.18f;
        float fadeOutTime = 0.18f;
        float holdTime = generatedCountDuration; // Duration to hold full opacity.

        // Use the stored default color as the target color.
        Color originalColor = defaultColor;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Start fully transparent.
        generatedCount.color = transparentColor;

        // Fade in with ease-in (quadratic).
        float elapsed = 0f;
        while (elapsed < fadeInTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeInTime);
            float easeIn = t * t; // Quadratic ease-in.
            generatedCount.color = Color.Lerp(transparentColor, originalColor, easeIn);
            yield return null;
        }
        generatedCount.color = originalColor;

        // Hold at full opacity.
        yield return new WaitForSeconds(holdTime);

        // Fade out with ease-out (quadratic).
        elapsed = 0f;
        while (elapsed < fadeOutTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeOutTime);
            float easeOut = 1 - (1 - t) * (1 - t); // Quadratic ease-out.
            generatedCount.color = Color.Lerp(originalColor, transparentColor, easeOut);
            yield return null;
        }
        generatedCount.color = transparentColor;
        generatedCount.gameObject.SetActive(false);
    }
}
