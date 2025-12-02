
using UnityEngine;
using UnityEngine.UI;
using TMPro; //Namesapce for textmeshpro
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    [Header("Game Stats")]
    public int score = 0;//score is calculated
    public int lives = 3;
    public int bombs = 3;
    public int enemiesKilled = 0;

    public float timePassed = 0f;
    public float timeLimit = 500f;

    public float playingTimePassed = 0f;
    public float speedOfTime = 1f;
    public float speedOfSlowedTime = 0.5f;
    public bool slowingTime = false;

    public int level = 1; //aka wave

    private int _enemyGoal = 10;
    public int EnemyGoal
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

    private void Start()
    {
        exitAction = InputSystem.actions.FindAction("Exit");
        if (SceneManager.GetActiveScene().name == "MainGame")
        {
            ChangeState(PlayingState);
        }
        else
        {
            ChangeState(MainMenuState);
        }
    }
    public void ChangeState(GameState newState)
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

    public void StartGame() //change later
    {
        
        RestartGame();
    }

    private void Update()
    {

            
        
        currentState.UpdateState(this);
    }


    public void AddScore(int points)
    {
        score += points;
        Debug.Log($"Score increased by {points}. Total: {score}");

        EventManager.TriggerEvent("OnScoreChanged");

        if (score>1999)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }


    public void LoseLife()
    {
        lives--;
        Debug.Log($"Life lost! Lives remaining: {lives}");


        if (lives <= 0)
        {
            ChangeState(GameOverState);
        }

        EventManager.TriggerEvent("OnLivesChanged");
    }

    public void LifePickedUp()
    {
        lives++;
        EventManager.TriggerEvent("OnLivesChanged");
    }

    public void BombPickedUp()
    {
        bombs++;
        EventManager.TriggerEvent("OnBombsChanged");
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        EnemyGoal--;
        AddScore(100); // 100 points per enemy
        EventManager.TriggerEvent("OnEnemyKilled");
        Debug.Log($"Enemy killed! Total enemies defeated: {enemiesKilled}");
    }


    public void CollectiblePickedUp(int value)
    {
        AddScore(value);
        Debug.Log($"Collectible picked up worth {value} points!");
    }



 

    public void reloadGame()
    {
        
        EventManager.TriggerEvent("OnReload");
        
        EventManager.ClearAllEvents();
        //SceneManager.LoadScene("Delete");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quitGame() => Application.Quit();


    public void RestartGame()
    {
        ChangeState(PlayingState);
        
        speedOfTime = 1f;
        Time.timeScale = 1f;

        // Reset all game state
        score = 0;
        lives = 3;
        enemiesKilled = 0;
        timePassed = 0f;
        playingTimePassed = 0f;


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
    }


}
