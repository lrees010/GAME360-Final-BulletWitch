using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shark : MonoBehaviour
{
    [Header("Shark Stats")]
    private int health = 5;
    public float moveSpeed = 8f;

    public GameObject SpriteObject;

    [Header("AI")]
    //public float detectionRange = 0.5f;

    private Transform player;
    private Rigidbody2D rb;

    private SpriteRenderer spr;

    private Vector3 chargeLocation;
    private Quaternion chargeDirection;

    private void Start()
    {
        transform.position = transform.position + new Vector3(0, 3,0);
        rb = GetComponent<Rigidbody2D>();
        spr = SpriteObject.GetComponent<SpriteRenderer>();

        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) player = playerObj.transform;

        chargeLocation = (player.position - transform.position).normalized;
        chargeDirection = Quaternion.LookRotation(player.position - transform.position);

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
        if (InBounds==false)
        {
            Vanish();
        }
        else
        {
            ChasePlayer();
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
            return Mathf.Abs(transform.position.y) < 15f && Mathf.Abs(transform.position.x) < 15; //in the play area (not lower than -9, not more than 15 units left or right)
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

            if (GameManager.Instance.powerupActive)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }




            

            transform.rotation = new Quaternion(0, 0, chargeDirection.z, chargeDirection.w);
            rb.AddForce((chargeLocation*9f)*9f);
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity,6f);
            

        }
    }

    private System.Collections.IEnumerator DamageVisual(float fadeDuration, float holdDuration)
    {
        float time = 0f;
        /*
        
        if (holdDuration > 0f)
        {
            while (time < holdDuration)
            {
                time += Time.deltaTime;
                yield return null;
            }

        }
        */

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
        rb.linearVelocity = rb.linearVelocity * new Vector2(0.8f, 0.5f);

        //play hit sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.HitSound);

        if (damageCoroutine!=null)
        {
            StopCoroutine(damageCoroutine);
        }
        
        damageCoroutine = StartCoroutine(DamageVisual(0.3f, 0.3f));

        if (health <= 0)
        {
            //Debug.Log("die");
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
