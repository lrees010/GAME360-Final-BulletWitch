using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR;

public class ObsidianManager : MonoBehaviour
{

    public GameObject spitterBulletPrefab;
    [Header("Obsidian Stats")]
    private int health = 300;
    private int originalHealth;
    private float moveSpeed = 6f;

    public Transform firePoint;

    [Header("AI")]
    //public float detectionRange = 0.5f;

    private Transform player;
    private GameObject playerObj;
    private Rigidbody2D rb;
    private SpriteRenderer spr;

    //state manager
    ObsidianState currentState;
    public ObsidianChaseState ObsidianChaseState = new ObsidianChaseState();
    public ObsidianFiringState ObsidianFiringState = new ObsidianFiringState();


    private void Start()
    {
        originalHealth = health;
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        transform.position = new Vector2(0, 7); //overwrite spawn position

        // Find player
        playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) player = playerObj.transform;

        //events
        EventManager.Subscribe("OnPlayerDeath", Respawn);
        EventManager.Subscribe("OnBomb", Explode);
        EventManager.Subscribe("OnLevelChanged", LevelChanged);

        //state manager
        ChangeState(ObsidianFiringState);
    }
    public void ChangeState(ObsidianState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);

        EventManager.TriggerEvent("OnObsidianStateChanged", currentState.GetStateName());
    }

    private void OnDestroy()
    {
        //EventManager.Unsubscribe("OnPlayerDeath", Vanish);
        EventManager.Unsubscribe("OnBomb", Explode);

        EventManager.Unsubscribe("OnLevelChanged", LevelChanged);

        EventManager.Unsubscribe("OnPlayerDeath", Respawn);
    }

   

    private void FixedUpdate()
    {
        if (InBounds==false)
        {
            Respawn();
        }
        else
        {
            currentState.UpdateState(this);
        }
    }

    private void Respawn()
    {
        if (currentState!=ObsidianFiringState)
        {
            transform.position = new Vector2(0, 7);
            rb.linearVelocity = Vector2.zero;
        }

    }

    private void send(GameObject bullet, Vector2 direction, float lifetime, float speed)
    {
        bullet.transform.position = firePoint.position;
        Rigidbody2D bulletrb;
        bulletrb = bullet.GetComponent<Rigidbody2D>();
        bulletrb.linearVelocity = (direction * speed)+rb.linearVelocity;
        Destroy(bullet, lifetime);
    }

    private float lastShootTime = 0f;
    public void Shoot(float fireRate)
    {
        if (spitterBulletPrefab && (Time.time- lastShootTime) > fireRate)
        {
            //rb.linearVelocity = new Vector2(0f,rb.linearVelocity.y);
            send(Instantiate(spitterBulletPrefab), Vector2.down, 7f, 2f);
            send(Instantiate(spitterBulletPrefab), Vector2.down+Vector2.left, 7f, 2f);
            send(Instantiate(spitterBulletPrefab), Vector2.down + Vector2.right, 7f, 2f);
            lastShootTime = Time.time;
        }
    }

    public void SideShoot(float fireRate)
    {
        if (spitterBulletPrefab && (Time.time - lastShootTime) > fireRate)
        {
            //rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            send(Instantiate(spitterBulletPrefab), Vector2.left, 7f, 6f);
            send(Instantiate(spitterBulletPrefab), Vector2.right, 7f, 6f);
            lastShootTime = Time.time;
        }
    }

    public void Vanish() //player dies = all enemies vanish, no points
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

    public void MoveTo(Vector3 pos,float speedMultiplier, float speedLimit)
    {
        Vector2 direction = (pos - transform.position).normalized;

        rb.AddForce(((direction * moveSpeed) * 9)*speedMultiplier);
        //rb.linearVelocity = new Vector2(rb.linearVelocity.x, -2f);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, speedLimit);
    }
    public void ChasePlayer()
    {
        if (player==null)
        {
            return;
        }
        if (GameManager.Instance.powerupActive == false)
        {

            MoveTo(player.position, Mathf.Abs((Mathf.Sin(Time.time) / 2f)), 8f);
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
        float healthPercentage = Mathf.Clamp((float)health / (float)originalHealth, 0.5f,1f);
        transform.localScale = new Vector3(healthPercentage, healthPercentage, healthPercentage);
        rb.linearVelocity = rb.linearVelocity * new Vector2(0.8f, 0.5f);

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        //StopAllCoroutines();
        damageCoroutine = StartCoroutine(DamageVisual(0.3f, 0.3f));

        if (health <= 0)
        {
            Debug.Log("die");
            Die();
        }
    }
    private void Explode()
    {
        StartCoroutine(
    DelayedDamage(Mathf.Clamp(((Vector2.Distance(transform.position, player.position)) / 15f), 0f, 0.7f))
    );
    }
    private IEnumerator DelayedDamage(float delay)
    {
        yield return new WaitForSeconds(delay);
        TakeDamage(15);
        Respawn();
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
