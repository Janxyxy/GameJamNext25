using UnityEngine;
using System.Collections;

public class AntMover : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private bool canDespawn = false;

    public void Initialize(Vector2 target, float moveSpeed)
    {
        direction = (target - (Vector2)transform.position).normalized;
        speed = moveSpeed;

        // Rotate to face movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle); // For 2D rotation

        // Start despawn check after a short delay
        StartCoroutine(EnableDespawn());

        Debug.Log($"Ant moving to {target} at speed {speed}, angle {angle}");
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        if (canDespawn && IsOutsideExtendedViewport())
        {
            Debug.Log("Ant exited screen, destroying...");
            Destroy(gameObject);
        }
    }

    IEnumerator EnableDespawn()
    {
        yield return new WaitForSeconds(1f); // Wait before checking for despawn
        canDespawn = true;
    }

    bool IsOutsideExtendedViewport()
    {
        if (Camera.main == null) return false;

        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x < -0.3f || screenPoint.x > 1.3f || screenPoint.y < -0.3f || screenPoint.y > 1.3f;
    }
}
