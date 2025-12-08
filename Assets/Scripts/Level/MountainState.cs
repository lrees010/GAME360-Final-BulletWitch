//using System;
using UnityEngine;

public class MountainState : LevelState
{
    //enemy spawning variables
    private float nextSpawnTime = 0f;
    private float spawnRate = 2f;
    //collectible spawning variables
    private float nextCoinTime = 0f;
    private float coinSpawnRate = 3f;

    private bool spawnedBoss = false;

    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered MountainState");
        GameManager.Instance.level = 5; //we are on level 5
        GameManager.Instance.EnemyGoal = 1; //kill one enemy (Obsidian boss) to progress
        spawnedBoss = false; //boss hasn't spawned yet

        GameManager.Instance.currentBullet = "Wave"; //change weapon to wave

        TextAsset jsonFile = Resources.Load<TextAsset>("Convos/boss");//load dialogue json
        DialogueManager.Instance.StartConversation(jsonFile); //start dialogue using dialoguemanager, with the json as a parameter

        AudioManager.Instance.PlayMusic(AudioManager.Instance.MountainMusic); //play mountain music

    }


    public override void UpdateState(LevelManager level)
    {
        SpawnObsidian(); //spawn boss
        if (DialogueManager.Instance.isDialogueActive)//if dialogue is happening, don't spawn enemies or collectibles
        {
            return;
        }
        if (GameManager.Instance.EnemyGoal <= 0)//if enemy goal is reached, the boss is defeated, switch game to victory state
        {
            GameManager.Instance.ChangeState(GameManager.Instance.VictoryState);
        }
        else
        {
            CollectibleBehavior(); //spawn collectibles
            //SpawnBehavior();
        }
    }


    private void SpawnObsidian()
    {
        //spawn boss, if it wasn't already spawned
        if (!spawnedBoss)
        {
            spawnedBoss = true; //boss has been spawned
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
        if (Time.time >= nextCoinTime)//spawn rate
        {
            switch (Random.Range(0, 4)) //spawn random collectible
            {
                case 0:
                    CollectibleSpawner.Instance.SpawnBomb();
                    break;
                case 1:
                    CollectibleSpawner.Instance.SpawnLife();
                    break;
                case >=2:
                    CollectibleSpawner.Instance.SpawnCoin();
                    break;
            }
            
            
            nextCoinTime = Time.time + coinSpawnRate;
        }
    }
    public override void ExitState(LevelManager level) { }

    public override string GetLevelName() => "Mountain";
}

