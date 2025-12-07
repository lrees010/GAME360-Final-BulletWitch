using UnityEngine;

public class Coin : MonoBehaviour
{

    //provides basic movement for collectibles

    public Vector2 movement = new Vector2(0f, -4f); //direction to move in, and speed. public so it can be changed per collectible prefab
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        rb.linearVelocity = movement; //set velocity

        if (transform.position.y<-7f) //despawn if lower than y -7 (off screen)
        {
            Destroy(gameObject);
        }
    }
}
