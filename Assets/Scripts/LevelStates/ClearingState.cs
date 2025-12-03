using System;
using UnityEngine;

public class ClearingState : LevelState
{
    //
    //



    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered ClearingState");
        GameManager.Instance.level = 1;
        GameManager.Instance.EnemyGoal = -1;
        GameManager.Instance.currentBullet = "Bullet";

        TextAsset jsonFile = Resources.Load<TextAsset>("Convos/initial");
        DialogueManager.Instance.StartConversation(jsonFile);

        AudioManager.Instance.ChangeMusic("Clearing");
    }


    public override void UpdateState(LevelManager level)
    {
        
        if (DialogueManager.Instance.isDialogueActive == false) //wait for dialogue end
        {
            level.ChangeState(level.ForestState);
        }
        else
        {
            SpawnBehavior();
            CollectibleBehavior();
        }


    }

    private void SpawnBehavior()
    {

    }

    private void CollectibleBehavior()
    {

    }

    public override void ExitState(LevelManager level) 
    {

    }

    public override string GetLevelName() => "Clearing";
}

