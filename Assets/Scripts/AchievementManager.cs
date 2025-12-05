using System.Collections.Generic;
using UnityEngine;


public class AchievementManager : MonoBehaviour
{
    Dictionary<string, bool> achievementDictionary = new Dictionary<string, bool>();




    void Start()
    {
        // Subscribe to events (become an observer)
        EventManager.Subscribe("OnScoreChanged", ScoreAchievement);
        EventManager.Subscribe("OnEnemyKilled", EnemiesKilledAchievement);

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
        if (achievementDictionary[key] == false)
        {
            achievementDictionary[key] = true;
            EventManager.TriggerEvent("OnUnlockAchievement", message);
            Debug.Log(message);
            AudioManager.Instance.PlayAchievementSound();
        }
    }



    void EnemiesKilledAchievement()
    {
        if (GameManager.Instance.enemiesKilled >= 5)
        {
            UnlockAchievement("5Enemies", "Achievement:\n 5 enemies KILLED");
        }
    }

    void ScoreAchievement()
    {
        if (GameManager.Instance.score >= 600)
        {
            UnlockAchievement("600Score", "Achievement:\n 600 score GAINED");
        }
    }
}
