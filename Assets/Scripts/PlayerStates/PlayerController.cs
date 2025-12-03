using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public GameObject bloomBulletPrefab;
    public GameObject waveBulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.1f;
    public float nextFireTime = 0f;


    PlayerState currentState;
    public IdleState IdleState = new IdleState();
    public MovingState MovingState = new MovingState();
    public DamagedState DamagedState = new DamagedState();
    public PowerupState PowerupState = new PowerupState();

    public bool damageCooldown = false;


    public Rigidbody2D rb;

    //inputactions
    InputAction attackAction;
    InputAction specialAction; //shift slow time
    public InputAction moveAction;
    InputAction powerupAction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        ChangeState(IdleState);

        //inputs
        attackAction = InputSystem.actions.FindAction("Attack");
        specialAction = InputSystem.actions.FindAction("Special");
        moveAction = InputSystem.actions.FindAction("Move");
        powerupAction = InputSystem.actions.FindAction("Powerup");
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
    
    private GameObject currentBulletPrefab
    {
        get
        {
            switch(GameManager.Instance.currentBullet)
            {
                default: //"Bullet"
                    {
                        nextFireTime = Time.time + 0.3f;
                        return bulletPrefab;
                    }
                case "Bloom":
                    {
                        nextFireTime = Time.time + 0.3f;
                        return bloomBulletPrefab;
                    }
                case "Wave":
                    {
                        nextFireTime = Time.time + 0.1f;
                        return waveBulletPrefab;
                    }
            }
        }
    }

    public void HandleShooting(PlayerController player)
    {
        if (attackAction.IsPressed() && Time.time >= player.nextFireTime
            && GameManager.Instance.GetStateName() == "PlayingState")
        {
            player.FireBullet();
            
        }

    }

    public void FireBullet()
    {
        //fireRate = 100f / ((GameManager.Instance.score/10f)+100f);


        if (currentBulletPrefab && firePoint)
        {
            Instantiate(currentBulletPrefab, firePoint.position, firePoint.rotation);

            AudioManager.Instance.PlayShootSound();
            // Play shoot sound effect
        }



    }

    public void HandleSlowTime(PlayerController player)
    {
        if  (
            (GameManager.Instance.GetStateName() != "PlayingState") //if we are not in the playing state of the game
            || currentState.GetStateName() == "Damaged" //or if the player is currently respawning
            || GameManager.Instance.lives < 1 //if we have no lives
            )
        {
            return; //don't allow time slowing
        }

        if (
            specialAction.IsPressed() //pressing shift/slow time button
            )
        {
            //Debug.Log(currentState.GetStateName());
            GameManager.Instance.slowingTime = true;
        }
        else
        {
            GameManager.Instance.slowingTime = false;
        }
    }

    public void HandleBomb(PlayerController player)
    {
        if (
            powerupAction.WasPressedThisFrame()
            && (GameManager.Instance.GetStateName() == "PlayingState")
            && (GameManager.Instance.bombs>0)
            )
        {
            
            GameManager.Instance.bombs = GameManager.Instance.bombs - 1;
            EventManager.TriggerEvent("OnBomb");
            ChangeState(PowerupState);
            //add flair later
        }
    }



    private void TakeDamage()
    {
        if (damageCooldown == false)
        {
            // Player hit by enemy - lose a life
            GameManager.Instance.LoseLife();
            if (GameManager.Instance.lives > 0)
            {
                ChangeState(DamagedState);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.tag)
        {
            case "Coin":
                GameManager.Instance.CollectiblePickedUp(100);
                Destroy(other.gameObject);
                break;

            case "Enemy":
                TakeDamage();
                break;

            case "Life":
                GameManager.Instance.LifePickedUp();
                Destroy(other.gameObject);
                break;

            case "Bomb":
                GameManager.Instance.BombPickedUp();
                Destroy(other.gameObject);
                break;
        }
    }
}
