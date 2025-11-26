using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float nextFireTime = 0f;


    PlayerState currentState;
    public IdleState IdleState = new IdleState();
    public MovingState MovingState = new MovingState();
    public DamagedState DamagedState = new DamagedState();

    public bool damageCooldown = false;


    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        ChangeState(IdleState);
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);

        EventManager.TriggerEvent("OnPlayerStateChanged", currentState.GetStateName());
    }


    private void Update()
    {
        currentState.UpdateState(this);
    }



    public void FireBullet()
    {
        fireRate = 100f / ((GameManager.Instance.score/10f)+100f);


        if (bulletPrefab && firePoint)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            AudioManager.Instance.PlayShootSound();
            // Play shoot sound effect
        }



    }

    public void HandleShooting(PlayerController player)
    {
        if (Input.GetButton("Fire1") && Time.time >= player.nextFireTime)
        {
            player.FireBullet();
            player.nextFireTime = Time.time + player.fireRate;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy") && damageCooldown==false)
        {
            // Player hit by enemy - lose a life
            GameManager.Instance.LoseLife();
            if (GameManager.Instance.lives>0)
            {
                ChangeState(DamagedState);
            }
        }

        if (other.CompareTag("Collectible"))
        {
            // Player collected an item
            Collectible collectible = other.GetComponent<Collectible>();
            if (collectible)
            {
                GameManager.Instance.CollectiblePickedUp(100);
                
                Destroy(other.gameObject);


            }
        }
    }
}
