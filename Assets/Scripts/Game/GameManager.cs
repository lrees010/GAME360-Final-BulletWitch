
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    public string currentBullet = "Bullet"; //current player weapon as string

    [Header("Game Stats")]
    public int score = 0;
    public int lives = 3;
    public int bombs = 3;
    public int enemiesKilled = 0;

    public int maxLives = 6;

    public float timePassed = 0f;
    public float timeLimit = 500f;

    public float playingTimePassed = 0f;
    public float speedOfTime = 1f;
    public float speedOfSlowedTime = 0.5f;
    public bool slowingTime = false;

    public bool powerupActive = false;

    public int level = 1; //aka wave

    private int _enemyGoal = -1;
    public int EnemyGoal //make property so event is automatically triggered when the value is changed
    {
        get {
            return _enemyGoal;
        }
        set
        {
            _enemyGoal = value;
            EventManager.TriggerEvent("OnGoalChange");
        }
    }
    public InputAction exitAction;
    /* 
     Levels:
    0. Debug
    1. Forest
    2. Cave
    3. Lake
    4. Beach
    5. Mountain (final)
     */


    //public TMP_Text scoreText;

  

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManagers
        }


    }


    GameState currentState;
    public PlayingState PlayingState = new PlayingState();
    public MainMenuState MainMenuState = new MainMenuState();
    public PausedState PausedState = new PausedState();
    public GameOverState GameOverState = new GameOverState();
    public VictoryState VictoryState = new VictoryState();

    private void Start()
    {
        Application.targetFrameRate = 60;
        exitAction = InputSystem.actions.FindAction("Exit");

        switch(SceneManager.GetActiveScene().name) //for debugging, when starting the game from the MainGame scene, automatically switch game state to PlayingState
        {
            case "MainGame":
                ChangeState(PlayingState);
                break;
            case "MainMenu":
                ChangeState(MainMenuState);
                break;

        }
    }
    public void ChangeState(GameState newState) //switch game state and trigger event
    {
        
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);

        EventManager.TriggerEvent("OnGameStateChanged", currentState.GetStateName());
    }

    public string GetStateName() => currentState.GetStateName();

    public void StartGame() //When game is played from Main Menu, in case extra code needed in future. Just runs RestartGame method
    {
        
        RestartGame();
    }

    private void Update()
    {

        Time.timeScale = speedOfTime; //update TimeScale to our variable
        
        currentState.UpdateState(this);


    }


    public void AddScore(int points) //public method for giving score to the player
    {
        score += points;
        Debug.Log($"Score increased by {points}. Total: {score}");

        EventManager.TriggerEvent("OnScoreChanged");
    }


    public void LoseLife() //method for removing a player life, and switching to game over state when no lives left
    {
        lives--;
        Debug.Log($"Life lost! Lives remaining: {lives}");


        if (lives <= 0)
        {
            ChangeState(GameOverState);
        }

        EventManager.TriggerEvent("OnLivesChanged");
    }

    public void LifePickedUp() //public method for adding a player life
    {
        lives++;
        EventManager.TriggerEvent("OnLivesChanged");

        //play life pickup sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.LifePickupSound);
    }

    public void BombPickedUp() //public method for giving player a bomb powerup
    {
        bombs++;
        EventManager.TriggerEvent("OnBombsChanged");

        //play bomb pickup sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.BombPickupSound);
    }

    public void EnemyKilled() //public method to perform default actions when an enemy is killed
    {
        enemiesKilled++;
        EnemyGoal--; //Get closer to reaching the goal
        AddScore(100); // 100 points per enemy
        EventManager.TriggerEvent("OnEnemyKilled");
        Debug.Log($"Enemy killed! Total enemies defeated: {enemiesKilled}");
    }


    public void CollectiblePickedUp(int value) //public method for when a coin is picked up
    {
        //play coin sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.CoinSound);

        AddScore(value);
        Debug.Log($"Collectible picked up worth {value} points!");
    }

    public void quitGame() => Application.Quit(); //public method to close app


    public void RestartGame() //public method to reset variables and reload the game scene
    {
        EventManager.TriggerEvent("OnReload");
        ChangeState(PlayingState);
        
        speedOfTime = 1f;
        Time.timeScale = 1f;

        // Reset all game state
        score = 0;
        lives = 3;
        enemiesKilled = 0;
        bombs = 3;
        timePassed = 0f;
        playingTimePassed = 0f;
        currentBullet = "Bullet";
        
        //_enemyGoal = 1; //don't wanna trigger events


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        

    }


}
