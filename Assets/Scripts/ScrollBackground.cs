using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;

    [SerializeField] private Renderer bgrenderer;

    void Update()
    {
        bgrenderer.material.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, 0);
    }
}
