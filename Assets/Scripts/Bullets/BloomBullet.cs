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
        AudioManager.Instance.PlayBloomShootSound();
        send(Instantiate(bloom), Vector2.up);
        send(Instantiate(bloom), (Vector2.up + new Vector2(-0.2f, 0f)));
        send(Instantiate(bloom), (Vector2.up + new Vector2(0.2f, 0f)));

        // Destroy BloomBullet after lifetime
        Destroy(gameObject, lifetime);
    }


}