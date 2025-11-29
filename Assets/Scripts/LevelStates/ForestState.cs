using System;
using UnityEngine;

public class ForestState : LevelState
{
    private float nextSpawnTime = 0f;
    private float spawnRate = 2f;

    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered ForestState");
        GameManager.Instance.level = 1;
    }


    public override void UpdateState(LevelManager level)
    {
        if (GameManager.Instance.playingTimePassed>10)
        {
            level.ChangeState(level.CaveState);
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

    public override string GetLevelName() => "Forest";
}

