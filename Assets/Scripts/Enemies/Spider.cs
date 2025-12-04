using System.Collections;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [Header("Spider Stats")]
    private int health = 1;
    private float moveSpeed = 20f;

    [Header("AI")]
    //public float detectionRange = 0.5f;

    private Transform player;
    private Rigidbody2D rb;
    private Collider2D col;

    private float birthTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) player = playerObj.transform;

        //events
        EventManager.Subscribe("OnPlayerDeath", Vanish);
        EventManager.Subscribe("OnBomb", Explode);
        EventManager.Subscribe("OnLevelChanged", LevelChanged);

        birthTime = Time.time;
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnPlayerDeath", Vanish);
        EventManager.Unsubscribe("OnBomb", Explode);

        EventManager.Unsubscribe("OnLevelChanged", LevelChanged);
    }

    private void Update()
    {
        if (InBounds==false)
        {
            Vanish();
        }
        if (isEscaping == true)
        {
            Escape();
        }
        else
        {
            ChasePlayer();
        }
    }

    private bool isEscaping
    {
        get
        {
            return Time.time - birthTime > 5f;
        }
    }
    private void Vanish() //player dies = all enemies vanish, no points
    {
        Destroy(gameObject);
    }

    private void LevelChanged(object data) //overload
    {
        Vanish();
    }

    private bool InBounds
    {
        get
        {
            return Mathf.Abs(transform.position.y) < 9f && Mathf.Abs(transform.position.x) < 15; //in the play area (not lower than -9, not more than 15 units left or right)
        }
    }
    private void Escape()
    {
        rb.AddForce(new Vector2(0f, -5f));

    }
    private void ChasePlayer()
    {
        if (player)
        {
            /*
            if (GameManager.Instance.score > 99)
                moveSpeed = 3f;
            if (GameManager.Instance.score > 1999)
                moveSpeed = 2f; */
            //float distance = Vector2.Distance(transform.position, player.position);
            if (GameManager.Instance.powerupActive)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 direction = (player.position - transform.position).normalized;

                //direction = new Vector2(direction.x, -1f); //Mathf.Clamp(direction.y,-1f,-0.1f)

                rb.AddForce((direction * moveSpeed) / Time.deltaTime);
                //rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y,-4f,-3f));
                rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity,7f);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
           // Bullet hit enemy
           TakeDamage(1);
           Destroy(gameObject); // Destroy self
        }

        if (other.CompareTag("Wall") && !isEscaping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x*-0.2f, rb.linearVelocity.y * -0.2f);
        }
    }

    private void Die()
    {
        // This is where Singleton shines!
        // Any enemy can easily notify the GameManager
        GameManager.Instance.EnemyKilled(); //update the score of the player
        AudioManager.Instance.PlayEnemyKilledSound();
        Destroy(gameObject); // the enemy gets destroyed
    }

    private void Explode()
    {
        StartCoroutine(
            DelayedDeath(Mathf.Clamp( ((Vector2.Distance(transform.position, player.position)) / 15f),0f,0.7f))
            );
    }

    private IEnumerator DelayedDeath(float delay)
    {
        yield return new WaitForSeconds(delay);
        Die();
    }

    /*
    private void OnDrawGizmosSelected()
    {
        // Custom 2D circle for older Unity versions
        Gizmos.color = Color.red;

        int segments = 32;
        float angle = 0f;
        Vector3 lastPos = transform.position + new Vector3(detectionRange, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            angle = (i * 360f / segments) * Mathf.Deg2Rad;
            Vector3 newPos = transform.position + new Vector3(
                Mathf.Cos(angle) * detectionRange,
                Mathf.Sin(angle) * detectionRange,
                0
            );
            Gizmos.DrawLine(lastPos, newPos);
            lastPos = newPos;
        }
    }
    */
}
