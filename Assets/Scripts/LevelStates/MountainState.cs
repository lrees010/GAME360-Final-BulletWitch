//using System;
using UnityEngine;

public class MountainState : LevelState
{
    private float nextSpawnTime = 0f;
    private float spawnRate = 2f;
    //col
    private float nextCoinTime = 0f;
    private float coinSpawnRate = 3f;

    private bool spawnedBoss = false;

    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered MountainState");
        GameManager.Instance.level = 5;
        GameManager.Instance.EnemyGoal = 1;
        spawnedBoss = false;

        GameManager.Instance.currentBullet = "Wave";

        TextAsset jsonFile = Resources.Load<TextAsset>("Convos/boss");
        DialogueManager.Instance.StartConversation(jsonFile);

        AudioManager.Instance.PlayMusic(AudioManager.Instance.MountainMusic);

    }


    public override void UpdateState(LevelManager level)
    {
        SpawnObsidian();
        if (DialogueManager.Instance.isDialogueActive)
        {
            return;
        }
        if (GameManager.Instance.EnemyGoal <= 0)
        {
            GameManager.Instance.ChangeState(GameManager.Instance.VictoryState);
        }
        else
        {
            CollectibleBehavior();
            //SpawnBehavior();
        }
    }


    private void SpawnObsidian()
    {
        //spawn boss
        if (!spawnedBoss)
        {
            spawnedBoss = true;
            EnemySpawner.Instance.SpawnObsidian();

        }
    }
    
    private void SpawnBehavior()
    {


        if (Time.time >= nextSpawnTime)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    EnemySpawner.Instance.SpawnCharger();
                    //EnemySpawner.Instance.SpawnSpider();
                    break;
                case 1:
                    EnemySpawner.Instance.SpawnSpider();
                    break;
                case 2:
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
            switch (Random.Range(0, 4))
            {
                case 0:
                    CollectibleSpawner.Instance.SpawnBomb();
                    
                    //CollectibleSpawner.Instance.SpawnLife();
                    break;
                case 1:
                    CollectibleSpawner.Instance.SpawnLife();
                    break;
                case >=2:
                    CollectibleSpawner.Instance.SpawnCoin();
                    break;
            }
            //Debug.Log((int)Time.time % 3);
            
            
            nextCoinTime = Time.time + coinSpawnRate;
        }
    }
    public override void ExitState(LevelManager level) { }

    public override string GetLevelName() => "Mountain";
}

