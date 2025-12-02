using System;
using UnityEngine;

public class CaveState : LevelState
{
    private float nextSpawnTime = 0f;
    private float spawnRate = 0.7f;
    //col
    private float nextCoinTime = 0f;
    private float coinSpawnRate = 2f;

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
            CollectibleBehavior();
        }
    }

    private void SpawnBehavior()
    {
        if (Time.time >= nextSpawnTime)
        {
            switch((int)Time.time%2)
            {
                case 0:
                    EnemySpawner.Instance.SpawnCharger();
                    break;
                case 1:
                    EnemySpawner.Instance.SpawnSpider();
                    break;
            }
            

            nextSpawnTime = Time.time + spawnRate;
        }
    }
    private void CollectibleBehavior()
    {
        if (Time.time >= nextCoinTime)
        {
            switch((int)Time.time%3) //never hits 3. only 0,1,2
            {
                case 0:
                    CollectibleSpawner.Instance.SpawnCoin();
                    break;
                case 1:
                    CollectibleSpawner.Instance.SpawnLife();
                    break;
                case 2:
                    CollectibleSpawner.Instance.SpawnBomb();
                    break;
            }
            Debug.Log((int)Time.time % 3);
            
            
            nextCoinTime = Time.time + coinSpawnRate;
        }
    }
    public override void ExitState(LevelManager level) { }

    public override string GetLevelName() => "Cave";
}

