using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGScroller : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 0.3f;
    [SerializeField]
    private float offset;
    private Material mat;
    private Image uiImage;
    void Start()
    {
        uiImage = GetComponent<Image>();
        mat = uiImage.material;
    }

    // Update is called once per frame
    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) / 10;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0.01f*Mathf.Sin(100*offset)));
    }
}
