using UnityEngine;

public class AntDefender : MonoBehaviour
{
    [SerializeField] private GameObject antGameObject;
    [SerializeField] private float rotationSpeed = 250f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;

    private float nextFireTime = 0f;
    public float fireRate = 0.2f;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Rotate(Vector3.forward, scroll * rotationSpeed);

        if ((Input.GetButtonDown("Fire1") || Input.GetKey(KeyCode.Space)) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.up * bulletSpeed;
            }

            Destroy(bullet, 3f);
        }
    }

}
