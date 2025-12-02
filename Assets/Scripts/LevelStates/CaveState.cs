using System;
using UnityEngine;

public class CaveState : LevelState
{
    private float nextSpawnTime = 0f;
    private float spawnRate = 0.7f;


    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered CaveState");
        GameManager.Instance.level = 2;
        GameManager.Instance.EnemyGoal = 10;

        TextAsset jsonFile = Resources.Load<TextAsset>("Convos/cave");
        DialogueManager.Instance.StartConversation(jsonFile);

    }


    public override void UpdateState(LevelManager level)
    {
        if (DialogueManager.Instance.isDialogueActive)
        {
            return;
        }
        if (GameManager.Instance.EnemyGoal <= 0)
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

