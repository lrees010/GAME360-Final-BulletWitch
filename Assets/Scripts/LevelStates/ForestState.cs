using System;
using UnityEngine;

public class ForestState : LevelState
{
    //enemy spawning variables
    private float nextSpawnTime = 0f;
    private float spawnRate = 0.3f;

    //collectible spawning variables
    private float nextCoinTime = 0f;
    private float coinSpawnRate = 2f;


    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered ForestState");
        GameManager.Instance.level = 1; //we are on level 1
        GameManager.Instance.EnemyGoal = 25; //kill 25 enemies to progress

        AudioManager.Instance.PlayMusic(AudioManager.Instance.ForestMusic); //play forest music
    }


    public override void UpdateState(LevelManager level)
    {
        if (GameManager.Instance.EnemyGoal <= 0)//if enemy goal is reached, change to next level
        {
            level.ChangeState(level.CaveState);
        }
        else
        {
            SpawnBehavior(); //spawn enemies
            CollectibleBehavior(); //spawn collectibles
        }


    }

    private void SpawnBehavior()
    {
        if (Time.time >= nextSpawnTime) //spawn rate
        {
            EnemySpawner.Instance.SpawnCharger();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private void CollectibleBehavior()
    {
        if (Time.time >= nextCoinTime) //spawn rate
        {
            CollectibleSpawner.Instance.SpawnCoin();
            nextCoinTime = Time.time + coinSpawnRate;
        }
    }

    public override void ExitState(LevelManager level) 
    {

    }

    public override string GetLevelName() => "Forest";
}

