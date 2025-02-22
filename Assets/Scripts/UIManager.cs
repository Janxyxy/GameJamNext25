using UnityEngine;
using UnityEngine.UI;
using static ResourcesManager;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image proggressBarFill;

    public static UIManager Instance { get; private set; }
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
    }

    internal void SerProggresBarFill(float fillAmount)
    {
        proggressBarFill.fillAmount = fillAmount;
    }

}
