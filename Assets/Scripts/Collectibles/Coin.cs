using UnityEngine;

public class Coin : MonoBehaviour
{

    public string collectibleType = "Coin";
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //rb.AddForce(new Vector2(0f,-0.5f));

        rb.linearVelocity = new Vector2(0f, -4f);

        if (transform.position.y<-7f) //despawn
        {
            Destroy(gameObject);
        }
    }
}
