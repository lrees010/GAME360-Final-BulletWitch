using UnityEngine;

public class BloomBullet : MonoBehaviour
{
    //Bloom Bullet weapon, sends three bullets in three directions
    [Header("BloomBullet Settings")]
    public GameObject bloom;
    public float speed = 10f;
    public float lifetime = 3f;



    private Rigidbody2D rb;

    private void send(GameObject bullet,Vector2 direction) //method for bullet behavior
    {
        bullet.SetActive(true); //make active
        bullet.transform.position = gameObject.transform.position; //move to proper location
        rb = bullet.GetComponent<Rigidbody2D>(); //get the rigidbody
        rb.linearVelocity = direction * speed; //move the bullet in provided direction
        Destroy(bullet, lifetime); //destroy bullet after some time
    }
    private void Start()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.BloomShootSound); //play bloom bullet sound
        send(Instantiate(bloom), Vector2.up); //send bullet upwards
        send(Instantiate(bloom), (Vector2.up + new Vector2(-0.2f, 0f))); //send another bullet diagonally left
        send(Instantiate(bloom), (Vector2.up + new Vector2(0.2f, 0f))); //send another bullet diagonally right

        // Destroy this gameobject after some time
        Destroy(gameObject, lifetime);
    }


}