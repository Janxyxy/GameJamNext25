using UnityEngine;

public class AntAttack : MonoBehaviour
{
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 200f;
    Vector3 targetPosition = Vector3.zero;

    // random speed between minSpeed and maxSpeed
    private float speed => Random.Range(minSpeed, maxSpeed);

    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        Vector2 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
        {
            // TODO: fight lost
        }
    }
}
