using UnityEngine;

public class Coin : MonoBehaviour
{

    //public string collectibleType = "Coin";

    public Vector2 movement = new Vector2(0f, -4f);
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //rb.AddForce(new Vector2(0f,-0.5f));

        rb.linearVelocity = movement;

        if (transform.position.y<-7f) //despawn
        {
            Destroy(gameObject);
        }
    }
}
