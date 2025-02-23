using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Debug log
            Debug.Log("Bullet hit enemy");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
