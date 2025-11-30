using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.1f;
    public float nextFireTime = 0f;


    PlayerState currentState;
    public IdleState IdleState = new IdleState();
    public MovingState MovingState = new MovingState();
    public DamagedState DamagedState = new DamagedState();

    public bool damageCooldown = false;


    public Rigidbody2D rb;

    //inputactions
    InputAction attackAction;
    InputAction specialAction; //shift slow time
    public InputAction moveAction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        ChangeState(IdleState);

        //inputs
        attackAction = InputSystem.actions.FindAction("Attack");
        specialAction = InputSystem.actions.FindAction("Special");
        moveAction = InputSystem.actions.FindAction("Move");
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
        //fireRate = 100f / ((GameManager.Instance.score/10f)+100f);


        if (bulletPrefab && firePoint)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            AudioManager.Instance.PlayShootSound();
            // Play shoot sound effect
        }



    }

    public void HandleSlowTime(PlayerController player)
    {
        if (
            specialAction.IsPressed() //pressing shift/slow time button
            && (GameManager.Instance.GetStateName() == "PlayingState") //we are in the playing state of the game, not main menu, paused, etc
            && currentState.GetStateName() != "Damaged" //the player isn't currently respawning
            )
        {
            //Debug.Log(currentState.GetStateName());
            GameManager.Instance.speedOfTime = 0.5f;
        }
        else
        {
            GameManager.Instance.speedOfTime = 1f;
        }
    }

    public void HandleShooting(PlayerController player)
    {
        if (attackAction.IsPressed() && Time.time >= player.nextFireTime)
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
