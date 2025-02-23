using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridTile : MonoBehaviour
{
    [SerializeField] public TileType tileType;

    [Header("UI")]
    [SerializeField] private Image tileIconImage;
    [SerializeField] private TextMeshProUGUI gainCount;
    [SerializeField] private TextMeshProUGUI antCount;

    [SerializeField] private float generatedCountDuration;

    [SerializeField] public bool isAntHill;

    private Coroutine currentShowGainCoroutine;
    private Coroutine updateCountsCorutine;

    private Color defaultColor;

    private Button button;
    public enum TileType
    {
        None,
        Anthill,
        Forest,
        Mountain,
        Meadow,
        Cave
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnTileClick);

        defaultColor = gainCount.color;
    }

    internal void SetTileIcon()
    {
        Sprite tileIcon = GameManager.Instance.GetTileIcon(tileType);

        if (tileIcon != null)

            if (tileIcon != null && tileIconImage != null)
                tileIconImage.sprite = tileIcon;
    }

    internal void OnTileClick()
    {
        GameManager.Instance.OnTileClick(tileType, this);
    }

    private void OnEnable()
    {
        UpdateCounts();
    }

    private void OnDisable()
    {
        if (currentShowGainCoroutine != null)
        {
            StopCoroutine(currentShowGainCoroutine);
        }

        if (updateCountsCorutine != null)
        {
            StopCoroutine(updateCountsCorutine);
        }
    }

    internal void UpdateCounts()
    {
        updateCountsCorutine = StartCoroutine(UpdateCountsCoroutine());
    }

    private IEnumerator UpdateCountsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            int antcount = GameManager.Instance.GetTileData(this).antsCount;
            SetAntCount(antcount);

            if (GameManager.Instance.TacticalView)
            {
                gainCount.gameObject.SetActive(false);
                if (antcount != 0)
                {
                    antCount.gameObject.SetActive(true);
                }

            }
            else
            {
                antCount.gameObject.SetActive(false);
            }

            Debug.Log("AAA");
        }
    }

    internal void StartGeneratedCount()
    {
        if (currentShowGainCoroutine != null)
        {
            StopCoroutine(currentShowGainCoroutine);
        }

        if (GameManager.Instance.TacticalView)
            return;

        currentShowGainCoroutine = StartCoroutine(ShowGeneratedCount());
    }

    internal void SetGainCount(int coint)
    {
        gainCount.text = $"+{coint.ToString()}";
    }

    internal void SetAntCount(int count)
    {
        if (count == 0)
        {
            antCount.gameObject.SetActive(false);
        }
        else
        {
            antCount.gameObject.SetActive(true);
        }
        antCount.text = count.ToString();
    }

    private IEnumerator ShowGeneratedCount()
    {
        if (GameManager.Instance.TacticalView)
            yield break;

        // Set text and ensure it's active.
        gainCount.gameObject.SetActive(true);

        // Use faster fade durations.
        float fadeInTime = 0.18f;
        float fadeOutTime = 0.18f;
        float holdTime = generatedCountDuration; // Duration to hold full opacity.

        // Use the stored default color as the target color.
        Color originalColor = defaultColor;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Start fully transparent.
        gainCount.color = transparentColor;

        // Fade in with ease-in (quadratic).
        float elapsed = 0f;
        while (elapsed < fadeInTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeInTime);
            float easeIn = t * t; // Quadratic ease-in.
            gainCount.color = Color.Lerp(transparentColor, originalColor, easeIn);
            yield return null;
        }
        gainCount.color = originalColor;

        // Hold at full opacity.
        yield return new WaitForSeconds(holdTime);

        // Fade out with ease-out (quadratic).
        elapsed = 0f;
        while (elapsed < fadeOutTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeOutTime);
            float easeOut = 1 - (1 - t) * (1 - t); // Quadratic ease-out.
            gainCount.color = Color.Lerp(originalColor, transparentColor, easeOut);
            yield return null;
        }
        gainCount.color = transparentColor;
        gainCount.gameObject.SetActive(false);
    }

    //internal void ActivateBoost(bool v)
    //{
    //    foreach (var item in GameManager.Instance.TileDataDictionary)
    //    {
    //        if (item.Key == this)
    //        {
    //            item.Value.isBoosted = v;
    //            Debug.Log("IsBoosted: " + v);
    //        }
    //    }
    //}

    internal void ChangeTileType(TileType tileType)
    {
        this.tileType = tileType;
    }
}
