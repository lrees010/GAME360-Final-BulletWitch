using System;
using UnityEngine;

public class ClearingState : LevelState
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
        Debug.Log("Entered ClearingState");
        GameManager.Instance.level = 1;
        GameManager.Instance.EnemyGoal = -1;
    }


    public override void UpdateState(LevelManager level)
    {
        if (GameManager.Instance.timePassed>3)
        {
            level.ChangeState(level.ForestState);
        }
        else
        {
            SpawnBehavior();
            CollectibleBehavior();
        }


    }

    private void SpawnBehavior()
    {

    }

    private void CollectibleBehavior()
    {

    }

    public override void ExitState(LevelManager level) 
    {

    }

    public override string GetLevelName() => "Clearing";
}

