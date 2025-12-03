using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    private float speed = 10f;
    private float lifetime = 3f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * speed;

        // Destroy bullet after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }

        // Destroy bullet if it hits walls or boundaries
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}