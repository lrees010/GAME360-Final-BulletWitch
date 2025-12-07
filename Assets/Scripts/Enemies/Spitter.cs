using System.Collections;
using UnityEngine;

public class Spitter : MonoBehaviour
{

    public GameObject spitterBulletPrefab;
    [Header("Spitter Stats")]
    private int health = 3;
    private float moveSpeed = 30f;

    [Header("AI")]
    //public float detectionRange = 0.5f;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spr;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();


        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) player = playerObj.transform;

        //events
        EventManager.Subscribe("OnPlayerDeath", Vanish);
        EventManager.Subscribe("OnBomb", Explode);
        EventManager.Subscribe("OnLevelChanged", LevelChanged);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnPlayerDeath", Vanish);
        EventManager.Unsubscribe("OnBomb", Explode);

        EventManager.Unsubscribe("OnLevelChanged", LevelChanged);
    }

   

    private void FixedUpdate()
    {
        if (InBounds==false)//destroy if out of bounds
        {
            Vanish();
        }
        else
        {
            if (GameManager.Instance.powerupActive|| Exploding==true)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
            ChasePlayer();
            Shoot();
        }
    }
    private void send(GameObject bullet, Vector2 direction, float lifetime, float speed)
    {
        //fires bullets in a given direction at a given speed

        bullet.transform.position = gameObject.transform.position;
        Rigidbody2D bulletrb;
        bulletrb = bullet.GetComponent<Rigidbody2D>();
        bulletrb.linearVelocity = (direction * speed);
        Destroy(bullet, lifetime);
    }

    private float lastShootTime = 0f;
    void Shoot()
    {
        if (spitterBulletPrefab && (Time.time- lastShootTime) >0.6f) //limit firerate
        {
            rb.linearVelocity = new Vector2(0f,rb.linearVelocity.y); //stop moving horizontally when firing
            send(Instantiate(spitterBulletPrefab), Vector2.down, 3f, 6f); //send bullet downwards
            lastShootTime = Time.time;

            //play shoot sound
            AudioManager.Instance.PlaySFX(AudioManager.Instance.SpitterShootSound);
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



                Vector2 direction = (player.position - transform.position).normalized;//set direction to player



            rb.AddForce((direction * moveSpeed) * 9); //move towards player
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -2f); //change y velocity to -2f
                rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity,6f); //limit overall speed
                
               

        }
    }

    private System.Collections.IEnumerator DamageVisual(float fadeDuration, float holdDuration)
    {
        //displays enemy being damaged by flashing red

        float time = 0f;


        time = 0f;
        //text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        while (time < fadeDuration)
        {

            time += Time.deltaTime;
            float colorCalc = ((time / fadeDuration)) + holdDuration;
            spr.color = new Color(spr.color.r, colorCalc, colorCalc);
            yield return null;
        }
    }


    private Coroutine damageCoroutine;
    public void TakeDamage(int damage)
    {
        health -= damage;
        rb.linearVelocity = rb.linearVelocity * new Vector2(0.8f, 0.5f); //reduce velocity when hit

        //play hit sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.HitSound);

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        //StopAllCoroutines();
        damageCoroutine = StartCoroutine(DamageVisual(0.3f, 0.3f)); //visualize damage without halting

        if (health <= 0) //die if health too low
        {
            Debug.Log("die");
            Die();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Bullet hit enemy
            TakeDamage(1);
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
        Exploding = true;
        StartCoroutine(
            DelayedDeath(Mathf.Clamp( ((Vector2.Distance(transform.position, player.position)) / 15f),0f,0.7f))
            );//delay explosion death based on distance to player (where bomb explosion originated), for visual effect and to prevent overly loud enemy death noise
    }
    private bool Exploding = false;
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
