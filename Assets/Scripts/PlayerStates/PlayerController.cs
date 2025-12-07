using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject explosionPrefab;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public GameObject bloomBulletPrefab;
    public GameObject waveBulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.1f;
    public float nextFireTime = 0f;


    PlayerState currentState; //player states
    public IdleState IdleState = new IdleState();
    public MovingState MovingState = new MovingState();
    public DamagedState DamagedState = new DamagedState();
    public PowerupState PowerupState = new PowerupState();

    public bool damageCooldown = false;


    public Rigidbody2D rb;

    //input actions
    InputAction attackAction; //fire bullets
    InputAction specialAction; //shift slow time
    public InputAction moveAction;
    InputAction powerupAction; //bomb powerup

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
        HandleSkipDialogue();

    }
    
    private GameObject currentBulletPrefab
    {
        get //returns different bullet prefabs and firerates based on currentBullet
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
        if (DialogueManager.Instance.isDialogueActive==true) //disable shooting when dialogue is happening
        {
            return;
        }
        if (attackAction.IsPressed() && Time.time >= player.nextFireTime //if attack button is held, fire bullet
            && GameManager.Instance.GetStateName() == "PlayingState")
        {
            player.FireBullet();
            
        }

    }

    private void HandleSkipDialogue()
    {
        if (powerupAction.WasPressedThisFrame()) //powerup button also skips dialogue
        {
            DialogueManager.Instance.EndDialogue();
        }
    }

    public void FireBullet()
    {

        if (currentBulletPrefab && firePoint)
        {
            Instantiate(currentBulletPrefab, firePoint.position, firePoint.rotation); //fire bullet from given position
        }



    }

    private void ExplosionFX()
    {
        if (explosionPrefab)
        {
            GameObject explosionInstance = Instantiate(explosionPrefab, gameObject.transform.position, gameObject.transform.rotation); //spawn explosion effect prefab
            Destroy(explosionInstance,1f);
        }
    }

    public void HandleSlowTime(PlayerController player)
    {
        if (DialogueManager.Instance.isDialogueActive == true) //if dialogue, ignore inputs and automatically unslow time
        {
            GameManager.Instance.slowingTime = false;
            return;
        }
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
            GameManager.Instance.slowingTime = true; //slow time
        }
        else
        {
            GameManager.Instance.slowingTime = false; //unslow time
        }
    }

    public void HandleBomb(PlayerController player)
    {
        if (DialogueManager.Instance.isDialogueActive == true) //dont allow bombs when dialogue occurring
        {
            return;
        }
        if (
            powerupAction.WasPressedThisFrame() //pressing bomb button
            && (GameManager.Instance.GetStateName() == "PlayingState") //game is currently playing
            && (GameManager.Instance.bombs>0) //have any bombs
            )
        {
            
            GameManager.Instance.bombs = GameManager.Instance.bombs - 1; //Reduce bomb count
            AudioManager.Instance.PlaySFX(AudioManager.Instance.BombSound); //play bomb explosion sound
            ExplosionFX(); 
            EventManager.TriggerEvent("OnBomb");
            ChangeState(PowerupState);
        }
    }



    private void TakeDamage()
    {
        if (DialogueManager.Instance.isDialogueActive == true) //don't allow taking damage during dialogue
        {
            return;
        }

        if (damageCooldown == false) //if we aren't in damage cooldown
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
        switch(other.tag) //different behavior depending on what we collided with
        {
            case "Coin":
                GameManager.Instance.CollectiblePickedUp(100);
                Destroy(other.gameObject);
                break;

            case "Enemy":
                TakeDamage();
                break;
            case "EnemyBullet":
                TakeDamage();
                break;

            case "Life":
                if (GameManager.Instance.lives< GameManager.Instance.maxLives) //don't add lives if we have the max amount
                {
                    GameManager.Instance.LifePickedUp();
                    Destroy(other.gameObject);
                }

                break;

            case "Bomb":
                GameManager.Instance.BombPickedUp();
                Destroy(other.gameObject);
                break;
        }
    }
}
