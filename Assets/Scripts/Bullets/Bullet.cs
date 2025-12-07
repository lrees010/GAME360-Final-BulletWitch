using UnityEngine;

public class Bullet : MonoBehaviour
{
    //default bullet weapon, sends one bullet in one direction

    [Header("Bullet Settings")]
    private float speed = 10f;
    private float lifetime = 3f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * speed; //move bullet upwards
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ShootSound); //play default shooting sound

        // Destroy bullet after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) //destroy the bullet if it hits an enemy
        {
            Destroy(gameObject);
        }
    }
}