//using System;
using UnityEngine;

public class BeachState : LevelState
{
    //enemy spawning variables
    private float nextSpawnTime = 0f;
    private float spawnRate = 0.4f;
    //collectible spawning variables
    private float nextCoinTime = 0f;
    private float coinSpawnRate = 1.7f;

    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered BeachState");
        GameManager.Instance.level = 4; //we are on level 4
        GameManager.Instance.EnemyGoal = 300; //kill 300 enemies to progress

        GameManager.Instance.currentBullet = "Bloom"; //set weapon to Bloom

        //TextAsset jsonFile = Resources.Load<TextAsset>("Convos/cave");
        //DialogueManager.Instance.StartConversation(jsonFile);

        AudioManager.Instance.PlayMusic(AudioManager.Instance.BeachMusic); //play beach level music
    }


    public override void UpdateState(LevelManager level)
    {
        if (DialogueManager.Instance.isDialogueActive) //if dialogue is happening, don't spawn enemies or collectibles
        {
            return;
        }
        if (GameManager.Instance.EnemyGoal <= 0) //if enemy goal is reached, change to next level
        {
            level.ChangeState(level.MountainState);
        }
        else //otherwise
        {
            SpawnBehavior(); //spawn enemies
            CollectibleBehavior(); //spawn collectibles
        }
    }

    private void SpawnBehavior()
    {
        if (Time.time >= nextSpawnTime) //spawn rate
        {
            switch(Random.Range(0, 4)) //spawn random set of enemies
            {
                case 0:
                    EnemySpawner.Instance.SpawnCharger();
                    EnemySpawner.Instance.SpawnSpitter();
                    //EnemySpawner.Instance.SpawnSpider();
                    break;
                case 1:
                    EnemySpawner.Instance.SpawnSpider();
                    EnemySpawner.Instance.SpawnSpitter();
                    break;
                case >=2:
                    EnemySpawner.Instance.SpawnCharger();
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
            switch(Random.Range(0, 3)) //spawn random collectible
            {
                case 0:
                    CollectibleSpawner.Instance.SpawnCoin();
                    //CollectibleSpawner.Instance.SpawnLife();
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

