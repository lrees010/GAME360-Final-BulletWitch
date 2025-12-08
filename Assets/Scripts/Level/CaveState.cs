//using System;
using UnityEngine;

public class CaveState : LevelState
{
    //enemy spawning variables
    private float nextSpawnTime = 0f;
    private float spawnRate = 0.4f;
    //collectible spawning variables
    private float nextCoinTime = 0f;
    private float coinSpawnRate = 2f;

    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered CaveState");
        GameManager.Instance.level = 2; //we are on level 2
        GameManager.Instance.EnemyGoal = 80; //kill 80 enemies to progress

        TextAsset jsonFile = Resources.Load<TextAsset>("Convos/cave"); //load dialogue json
        DialogueManager.Instance.StartConversation(jsonFile); //start dialogue using dialoguemanager, with the json as a parameter

        AudioManager.Instance.PlayMusic(AudioManager.Instance.CaveMusic); //play cave music
    }


    public override void UpdateState(LevelManager level)
    {
        if (DialogueManager.Instance.isDialogueActive)//if dialogue is happening, don't spawn enemies or collectibles
        {
            return;
        }
        if (GameManager.Instance.EnemyGoal <= 0) //if enemy goal is reached, change to next level
        {
            level.ChangeState(level.LakeState); 
        }
        else//otherwise
        {
            SpawnBehavior(); //spawn enemies
            CollectibleBehavior(); //spawn collectibiles
        }
    }

    private void SpawnBehavior()
    {
        if (Time.time >= nextSpawnTime)//spawn rate
        {
            switch (Random.Range(0, 2))//spawn random set of enemies
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
        if (Time.time >= nextCoinTime)//spawn rate
        {
            switch (Random.Range(0, 4))//spawn random collectible
            {
                case 0:
                    CollectibleSpawner.Instance.SpawnLife();
                    break;
                case 1:
                    CollectibleSpawner.Instance.SpawnBomb();
                    break;
                case >=2:
                    CollectibleSpawner.Instance.SpawnCoin();
                    break;
            }

            
            
            nextCoinTime = Time.time + coinSpawnRate;
        }
    }
    public override void ExitState(LevelManager level) { }

    public override string GetLevelName() => "Cave";
}

