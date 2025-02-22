using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    public Color outlineColor = Color.red;
    public float outlineSize = 0.1f;

    private GameObject outlineObject;

    void Start()
    {
        outlineObject = new GameObject("Outline");
        outlineObject.transform.parent = transform;
        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localScale = Vector3.one + Vector3.one * outlineSize;

        SpriteRenderer sr = outlineObject.AddComponent<SpriteRenderer>();
        SpriteRenderer originalSr = GetComponent<SpriteRenderer>();

        sr.sprite = originalSr.sprite;
        sr.color = outlineColor;
        sr.sortingLayerID = originalSr.sortingLayerID;
        sr.sortingOrder = originalSr.sortingOrder - 1;
    }
}
