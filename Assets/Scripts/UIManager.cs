using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputRemoting;

public class UIManager : MonoBehaviour
{

    [Header("UI References")]
    public Text scoreText;
    public Text livesText;
    public Text enemiesKilledText;
    public Text AchievementText;
    public Text stateText;
    public Text timeLeftText;
    public Text bombText;

    public Text goalText;

    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject victoryPanel;
    public Text victoryPanelText;

    public GameObject slowingPanel;



    void Start()
    {
        // Subscribe to events (become an observer)
        EventManager.Subscribe("OnScoreChanged", UpdateScore);
        EventManager.Subscribe("OnLivesChanged", UpdateLives);
        EventManager.Subscribe("OnEnemyKilled", UpdateEnemiesKilled);
        EventManager.Subscribe("OnGameOver", GameOver);
        EventManager.Subscribe("OnVictory", Victory);
        EventManager.Subscribe("OnReload", RefreshUIReferences);
        EventManager.Subscribe("OnUnlockAchievement", UpdateAchievement);
        EventManager.Subscribe("OnPlayerStateChanged", UpdateStateDisplay);

        EventManager.Subscribe("OnBomb", UpdateBombs);
        EventManager.Subscribe("OnBombsChanged", UpdateBombs);

        EventManager.Subscribe("OnPause", Pause);

        EventManager.Subscribe("OnTimeLeftChanged", UpdateTimeLeft);

        EventManager.Subscribe("OnGoalChange", UpdateGoal);

        EventManager.Subscribe("OnLevelChanged", LevelChanged);


        //refresh ui references
        //RefreshUIReferences();
    }
    void OnDestroy()
    {
        // Unsubscribe to prevent errors on scene change
        EventManager.Unsubscribe("OnScoreChanged", UpdateScore);
        EventManager.Unsubscribe("OnLivesChanged", UpdateLives);
        EventManager.Unsubscribe("OnEnemyKilled", UpdateEnemiesKilled);
        EventManager.Unsubscribe("OnGameOver", GameOver);
        EventManager.Unsubscribe("OnVictory", Victory);
        EventManager.Unsubscribe("OnReload", RefreshUIReferences);
        EventManager.Unsubscribe("OnUnlockAchievement", UpdateAchievement);
        EventManager.Unsubscribe("OnPlayerStateChanged", UpdateStateDisplay);

        EventManager.Unsubscribe("OnPause", Pause);

        EventManager.Unsubscribe("OnBomb", UpdateBombs);
        EventManager.Unsubscribe("OnBombsChanged", UpdateBombs);

        EventManager.Unsubscribe("OnTimeLeftChanged", UpdateTimeLeft);

        EventManager.Unsubscribe("OnGoalChange", UpdateGoal);

        EventManager.Unsubscribe("OnLevelChanged", LevelChanged);
    }
    void LevelChanged(object data)
    {
        
    }

    private void Update()
    {
        if (slowingPanel != null && GameManager.Instance.GetStateName()=="PlayingState" && Time.timeScale>0f)
        {
            slowingPanel.GetComponent<CanvasGroup>().alpha = 1f-(Time.timeScale);
        }
    }
    void UpdateAchievement(object data)
    {
        AchievementText.text = data.ToString();
        StartCoroutine(FadeOutText(AchievementText, 1f, 2f));
    }

    void Pause(object data)
    {
        if (pausePanel == null)
        {
            return;
        }
        if (data.ToString() == "Pause")
        {

            MakeVisible(true, pausePanel.GetComponent<CanvasGroup>());
            //pausePanel.gameObject.SetActive(true);
        }
        else
        {
            MakeVisible(false, pausePanel.GetComponent<CanvasGroup>());
            //pausePanel.gameObject.SetActive(false);
        }
    }

    void MakeVisible(bool visible, CanvasGroup group)
    {
        if (visible)
        {
            group.alpha = 1f;
            group.interactable = true;
            group.blocksRaycasts = true;
        }
        else
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }
    private System.Collections.IEnumerator FadeOutText(Text text, float fadeDuration, float holdDuration)
    {
        float time = 0f;
        if (holdDuration > 0f)
        {
            while (time < holdDuration)
            {
                time += Time.deltaTime;
                yield return null;
            }
            
        }
        

        time = 0f;
        //text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            text.color = new Color(text.color.r,text.color.g,text.color.b, 1f - (time / fadeDuration));
            yield return null;
        }
    }


    void UpdateBombs()
    {
        if (bombText)
        {
            bombText.text = "Bombs: " + GameManager.Instance.bombs;
        }
        else
        {
            Debug.Log("attempted refresh");
            RefreshUIReferences();
        }
    }

    string returnS(int goal) //yup
    {
        if(goal>1)
        {
            return "s";
        }
        else
        {
            return "";
        }
    }
    void UpdateGoal()
    {
        if (goalText)
        {
            int goal = GameManager.Instance.EnemyGoal;

            if (goal>-1)
            {
                goalText.text = $"Goal: {goal} kill{returnS(goal)}";
            }
            else
            {
                goalText.text = "Goal: N/A";
            }
        }
        else
        {
            Debug.Log("attempted refresh");
            RefreshUIReferences();
        }
    }

    void UpdateTimeLeft()
    {
        if (timeLeftText)
        {
            decimal timeLeftParse = Math.Round((decimal)(GameManager.Instance.timeLimit - GameManager.Instance.timePassed), 2);
            
            timeLeftText.text = "Time: " + (timeLeftParse);
        }
        else
        {
            Debug.Log("attempted refresh");
            RefreshUIReferences();
        }
    }

    void UpdateStateDisplay(object stateData)
    {
        if (stateText!=null)
        {
            stateText.text = "State: "+ stateData.ToString();
        }

    }

    // This runs when score changes
    void UpdateScore()
    {
        if (scoreText)
        {
            scoreText.text = "Score: " + GameManager.Instance.score;

        }
        else
        {
            Debug.Log("attempted refresh");
            RefreshUIReferences();
        }
        
    }

    void UpdateLives()
    {
        if (livesText)
        {
            livesText.text = "Lives: " + GameManager.Instance.lives+"/"+GameManager.Instance.maxLives;
            
        }
        else
        {
            Debug.Log("attempted refresh");
            RefreshUIReferences();
        }
    }

    void UpdateEnemiesKilled()
    {
        if (enemiesKilledText)
        {
            enemiesKilledText.text = "Kills: " + GameManager.Instance.enemiesKilled;

        }
        else
        {
            Debug.Log("attempted refresh");
            RefreshUIReferences();
        }
 
    }


    private void GameOver()
    {
        Debug.Log("owwwwww GameOver triggered | panel = " + gameOverPanel);
        MakeVisible(true, gameOverPanel.GetComponent<CanvasGroup>());

    }

    private void Victory()
    {
        Debug.Log("ui victory");
        MakeVisible(true, victoryPanel.GetComponent<CanvasGroup>());

        victoryPanelText.text = $"- FINAL STATS -\nFinal Score: {GameManager.Instance.score}\nFinal Time: {GameManager.Instance.timePassed}";
    }

    public void InitializeUI()
    {
        UpdateScore();
        UpdateLives();
        UpdateEnemiesKilled();
        UpdateBombs();
        UpdateTimeLeft();
        UpdateGoal();
    }

    private void RefreshUIReferences()
    {
        Debug.Log("refresh");
        scoreText = GameObject.Find("Score")?.GetComponent<Text>(); //scores not scorestext
        livesText = GameObject.Find("Lives")?.GetComponent<Text>();
        AchievementText = GameObject.Find("AchievementText")?.GetComponent<Text>();
        enemiesKilledText = GameObject.Find("EnemiesKilled")?.GetComponent<Text>();
        bombText = GameObject.Find("Bombs")?.GetComponent<Text>();
        stateText = GameObject.Find("State")?.GetComponent<Text>();

        timeLeftText = GameObject.Find("Time")?.GetComponent<Text>();

        goalText = GameObject.Find("Goal")?.GetComponent<Text>();

        victoryPanelText = GameObject.Find("VictoryDetails")?.GetComponent<Text>();

        pausePanel = GameObject.Find("PausePanel");
        gameOverPanel = GameObject.Find("GameEndPanel");
        victoryPanel = GameObject.Find("VictoryPanel");

        slowingPanel = GameObject.Find("SlowingPanel");


    }

 



    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //unsubscribe
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; //subscribe
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("sceneloaduimanager");
        RefreshUIReferences(); //refresh ui upon reloading the scene, score and lives
        InitializeUI();
    }

}
