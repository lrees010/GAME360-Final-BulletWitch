using UnityEngine;

public class BloomBullet : MonoBehaviour
{
    [Header("BloomBullet Settings")]
    public GameObject bloom;
    public float speed = 10f;
    public float lifetime = 3f;



    private Rigidbody2D rb;

    private void send(GameObject bullet,Vector2 direction)
    {
        bullet.SetActive(true);
        bullet.transform.position = gameObject.transform.position;
        rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;
        Destroy(bullet, lifetime);
    }
    private void Start()
    {
        send(Instantiate(bloom), Vector2.up);
        send(Instantiate(bloom), (Vector2.up+Vector2.left));
        send(Instantiate(bloom), (Vector2.up + Vector2.right));

        // Destroy BloomBullet after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }

        // Destroy BloomBullet if it hits walls or boundaries
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}