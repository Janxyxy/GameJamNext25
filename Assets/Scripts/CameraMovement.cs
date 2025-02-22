using System;
using UnityEngine;

public class SmoothCameraScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float smoothTime;
    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;

    private float targetHeight;
    private float velocity = 0.0f;

    internal void ResetCamera()
    {
        velocity = 0.0f;
        targetHeight = 0.0f;

        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
    }

    void Start()
    {
        targetHeight = transform.position.y;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            targetHeight += scroll * scrollSpeed;
            targetHeight = Mathf.Clamp(targetHeight, minHeight, maxHeight);
        }

        float newY = Mathf.SmoothDamp(transform.position.y, targetHeight, ref velocity, smoothTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
