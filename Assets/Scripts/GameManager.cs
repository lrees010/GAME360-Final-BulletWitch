
using UnityEngine;
using UnityEngine.UI;
using TMPro; //Namesapce for textmeshpro
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    [Header("Game Stats")]
    public int score = 0;//score is calculated
    public int lives = 3;
    public int enemiesKilled = 0;


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

    private void Start()
    {
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

    public void StartGame()
    {
        ChangeState(PlayingState);
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
            GameOver();
        }

        EventManager.TriggerEvent("OnLivesChanged");
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        AddScore(100); // 100 points per enemy
        EventManager.TriggerEvent("OnEnemyKilled");
        Debug.Log($"Enemy killed! Total enemies defeated: {enemiesKilled}");
    }


    public void CollectiblePickedUp(int value)
    {
        AddScore(value);
        Debug.Log($"Collectible picked up worth {value} points!");
    }



    private void GameOver()
    {
        Debug.Log("GAME OVER!");
        EventManager.TriggerEvent("OnGameOver");
        EventManager.ClearAllEvents();
        Time.timeScale = 0f; // Pause the game
    }

    public void reloadGame()
    {
        
        EventManager.TriggerEvent("OnReload");
        
        EventManager.ClearAllEvents();
        //SceneManager.LoadScene("Delete");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quitGame()
    {
        Application.Quit();
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;

        // Reset all game state
        score = 0;
        lives = 3;
        enemiesKilled = 0;

        

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
    }


}
