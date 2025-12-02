using System;
using UnityEngine;

public class ForestState : LevelState
{
    //
    //
    private float nextSpawnTime = 0f;
    private float spawnRate = 1.5f;

    //col
    private float nextCoinTime = 0f;
    private float coinSpawnRate = 2f;


    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered ForestState");
        GameManager.Instance.level = 1;
        GameManager.Instance.EnemyGoal = 5;

        AudioManager.Instance.ChangeMusic("Forest");
    }


    public override void UpdateState(LevelManager level)
    {
        if (GameManager.Instance.EnemyGoal <= 0)
        {
            level.ChangeState(level.CaveState);
        }
        else
        {
            SpawnBehavior();
            CollectibleBehavior();
        }


    }

    private void SpawnBehavior()
    {
        if (Time.time >= nextSpawnTime)
        {
            EnemySpawner.Instance.SpawnCharger();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private void CollectibleBehavior()
    {
        if (Time.time >= nextCoinTime)
        {
            CollectibleSpawner.Instance.SpawnCoin();
            CollectibleSpawner.Instance.SpawnLife();
            CollectibleSpawner.Instance.SpawnBomb();
            nextCoinTime = Time.time + coinSpawnRate;
        }
    }

    public override void ExitState(LevelManager level) 
    {

    }

    public override string GetLevelName() => "Forest";
}

