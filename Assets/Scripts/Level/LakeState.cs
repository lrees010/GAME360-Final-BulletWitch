//using System;
using UnityEngine;

public class LakeState : LevelState
{
    //enemy spawning variables
    private float nextSpawnTime = 0f;
    private float spawnRate = 0.4f;
    //collectible spawning variables
    private float nextCoinTime = 0f;
    private float coinSpawnRate = 2f;

    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered LakeState");
        GameManager.Instance.level = 3; //we are on level 3
        GameManager.Instance.EnemyGoal = 250; //kill 250 enemies to progress

        GameManager.Instance.currentBullet = "Bloom"; //switch weapon to Bloom

        AudioManager.Instance.PlayMusic(AudioManager.Instance.LakeMusic); //play lake music
    }


    public override void UpdateState(LevelManager level)
    {
        if (DialogueManager.Instance.isDialogueActive) //if dialogue is happening, don't spawn enemies or collectibles
        {
            return;
        }
        if (GameManager.Instance.EnemyGoal <= 0)//if enemy goal is reached, change to next level
        {
            level.ChangeState(level.BeachState); //add lake later
        }
        else
        {
            SpawnBehavior(); //spawn enemies
            CollectibleBehavior(); //spawn collectibles
        }
    }

    private void SpawnBehavior()
    {
        if (Time.time >= nextSpawnTime) //spawnrate
        {
            switch (Random.Range(0, 3))//spawn random set of enemies
            {
                case 0:

                    
                    EnemySpawner.Instance.SpawnSpider();
                    EnemySpawner.Instance.SpawnCharger();
                    EnemySpawner.Instance.SpawnCharger();
                    break;
                case 1:

                    EnemySpawner.Instance.SpawnShark();
                    EnemySpawner.Instance.SpawnSpider();
                    EnemySpawner.Instance.SpawnCharger();
                    //EnemySpawner.Instance.SpawnSpider();
                    break;
                case >=2:
                    EnemySpawner.Instance.SpawnCharger();
                    EnemySpawner.Instance.SpawnSpider();
                    
                    break;
            }
            

            nextSpawnTime = Time.time + spawnRate;
        }
    }
    private void CollectibleBehavior()
    {
        if (Time.time >= nextCoinTime)//spawn rate
        {
            switch (Random.Range(0,3))//spawn random collectible
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
            
            
            nextCoinTime = Time.time + coinSpawnRate;
        }
    }
    public override void ExitState(LevelManager level) { }

    public override string GetLevelName() => "Lake";
}

