using System.Collections.Generic;
using UnityEngine;


public class AchievementManager : MonoBehaviour
{
    //Store achievements and whether or not it has been completed, in a dictionary. 
    Dictionary<string, bool> achievementDictionary = new Dictionary<string, bool>();
    



    void Start()
    {
        // Subscribe to events (become an observer)
        EventManager.Subscribe("OnScoreChanged", ScoreAchievement);
        EventManager.Subscribe("OnEnemyKilled", EnemiesKilledAchievement);

        //Set up dictionary with some achievements
        achievementDictionary["600Score"] = false;
        achievementDictionary["5Enemies"] = false;

    }
    void OnDestroy()
    {
        // Unsubscribe to prevent errors on scene change
        EventManager.Unsubscribe("OnScoreChanged", ScoreAchievement);
        EventManager.Unsubscribe("OnEnemyKilled", EnemiesKilledAchievement);
    }

    private void UnlockAchievement(string key, string message)
    {
        if (achievementDictionary[key] == false) //if the achievement has not been unlocked already
        {
            achievementDictionary[key] = true; //unlock it (turn to true)
            EventManager.TriggerEvent("OnUnlockAchievement", message);
            Debug.Log(message);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.AchievementSound); //play achievement sound using AudioManager singleton
        }
    }



    void EnemiesKilledAchievement()
    {
        //when an enemy is killed, check for 5 enemies killed achievement
        if (GameManager.Instance.enemiesKilled >= 5)
        {
            UnlockAchievement("5Enemies", "Achievement:\n 5 enemies KILLED");
        }
    }

    void ScoreAchievement()
    {
        //when score is gained, check for 600 score achievement
        if (GameManager.Instance.score >= 600)
        {
            UnlockAchievement("600Score", "Achievement:\n 600 score GAINED");
        }
    }
}
