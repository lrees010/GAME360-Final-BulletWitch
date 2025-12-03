using System;
using UnityEngine;

public class BeachState : LevelState
{
    private float nextSpawnTime = 0f;
    private float spawnRate = 0.4f;
    //col
    private float nextCoinTime = 0f;
    private float coinSpawnRate = 1.7f;

    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered BeachState");
        GameManager.Instance.level = 4;
        GameManager.Instance.EnemyGoal = 40;

        GameManager.Instance.currentBullet = "Bloom";

        //TextAsset jsonFile = Resources.Load<TextAsset>("Convos/cave");
        //DialogueManager.Instance.StartConversation(jsonFile);

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
            switch((int)Time.time%4)
            {
                case 0:
                    EnemySpawner.Instance.SpawnCharger();
                    EnemySpawner.Instance.SpawnSpider();
                    break;
                case 1:
                    EnemySpawner.Instance.SpawnCharger();
                    break;
                case >=2:
                    EnemySpawner.Instance.SpawnSpider();
                    EnemySpawner.Instance.SpawnSpitter();
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
                    CollectibleSpawner.Instance.SpawnLife();
                    break;
                case 1:
                    CollectibleSpawner.Instance.SpawnLife();
                    break;
                case 2:
                    CollectibleSpawner.Instance.SpawnBomb();
                    break;
            }
            //Debug.Log((int)Time.time % 3);
            
            
            nextCoinTime = Time.time + coinSpawnRate;
        }
    }
    public override void ExitState(LevelManager level) { }

    public override string GetLevelName() => "Beach";
}

