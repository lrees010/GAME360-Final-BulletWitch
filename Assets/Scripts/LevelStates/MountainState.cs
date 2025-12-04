using System;
using UnityEngine;

public class MountainState : LevelState
{
    private float nextSpawnTime = 0f;
    private float spawnRate = 0.4f;
    //col
    private float nextCoinTime = 0f;
    private float coinSpawnRate = 1.7f;

    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered MountainState");
        GameManager.Instance.level = 5;
        GameManager.Instance.EnemyGoal = 1;
        spawnedBoss = false;

        GameManager.Instance.currentBullet = "Wave";

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
            //level.ChangeState(level.ForestState); //add victory later
        }
        else
        {
            SpawnBehavior();
            CollectibleBehavior();
        }
    }

    private bool spawnedBoss = false;
    private void SpawnBehavior()
    {
        //spawn boss
        if (!spawnedBoss)
        {
            spawnedBoss = true;
            EnemySpawner.Instance.SpawnObsidian();

        }
        /*
        if (Time.time >= nextSpawnTime)
        {
            switch((int)Time.time%4)
            {
                case 0:
                    EnemySpawner.Instance.SpawnCharger();
                    //EnemySpawner.Instance.SpawnSpider();
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
        }*/
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

    public override string GetLevelName() => "Mountain";
}

