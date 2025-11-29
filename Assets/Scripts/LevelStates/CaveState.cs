using System;
using UnityEngine;

public class CaveState : LevelState
{
    private float nextSpawnTime = 0f;
    private float spawnRate = 3f;

    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered CaveState");
        GameManager.Instance.level = 2;
    }


    public override void UpdateState(LevelManager level)
    {
        if (GameManager.Instance.playingTimePassed > 150)
        {
            //level.ChangeState(level.ForestState); //add lake later
        }
        else
        {
            SpawnBehavior();
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

    public override void ExitState(LevelManager level) { }

    public override string GetLevelName() => "Cave";
}

