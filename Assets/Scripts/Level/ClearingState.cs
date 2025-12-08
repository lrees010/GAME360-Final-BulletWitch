using System;
using UnityEngine;

public class ClearingState : LevelState
{

    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered ClearingState");
        GameManager.Instance.level = 0; //we are on level 0
        GameManager.Instance.EnemyGoal = -1; //no enemy goal, level automatically changes after dialogue ends
        GameManager.Instance.currentBullet = "Bullet"; //change current weapon to default

        TextAsset jsonFile = Resources.Load<TextAsset>("Convos/initial");//load dialogue json
        DialogueManager.Instance.StartConversation(jsonFile);//start dialogue using dialoguemanager, with the json as a parameter

        AudioManager.Instance.PlayMusic(AudioManager.Instance.ClearingMusic); //play clearing music
    }


    public override void UpdateState(LevelManager level)
    {
        
        if (DialogueManager.Instance.isDialogueActive == false) //wait for dialogue to end
        {
            level.ChangeState(level.ForestState); //switch to next level
        }
        else
        {
            SpawnBehavior();
            CollectibleBehavior();
        }


    }

    private void SpawnBehavior()
    {
        //No enemies spawn here
    }

    private void CollectibleBehavior()
    {
        //No collectibles spawn here
    }

    public override void ExitState(LevelManager level) 
    {

    }

    public override string GetLevelName() => "Clearing";
}

