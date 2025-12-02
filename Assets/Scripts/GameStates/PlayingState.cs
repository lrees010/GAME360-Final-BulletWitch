using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayingState : GameState
{
    public override void EnterState(GameManager game)
    {
        Debug.Log("Entered PlayingState");
        
    }

    private float lastSlowTime = 0f;
    private bool slowedTime = false;
    private float slowingSpeed = 0.6f;
    public override void UpdateState(GameManager game)
    {
        if (game.speedOfTime > 0f && !DialogueManager.Instance.isDialogueActive)
        {
            game.timePassed = game.timePassed + (Time.deltaTime / game.speedOfTime); //time passed always counts real seconds no matter what the speed of time is
            Debug.Log(game.timePassed);
            game.playingTimePassed = game.playingTimePassed + Time.deltaTime;

            if (game.timePassed > game.timeLimit) //if ran out of time..
            {
                game.ChangeState(game.GameOverState);
            }
            else
            {
                EventManager.TriggerEvent("OnTimeLeftChanged");
            }
        }
        

        if (game.slowingTime == true)
        {
            if (slowedTime == false)
            {
                lastSlowTime = Time.time;
            }
            slowedTime = true;

            game.speedOfTime = Mathf.Clamp(
                ((slowingSpeed-(Time.time - lastSlowTime)) / slowingSpeed),
                game.speedOfSlowedTime,
                1f);
        }
        else
        {
            if (slowedTime == true)
            {
                lastSlowTime = Time.time;
            }
            slowedTime = false;
            game.speedOfTime = Mathf.Clamp(
                (((Time.time - lastSlowTime)) / slowingSpeed)+game.speedOfSlowedTime,
                game.speedOfSlowedTime,
                1f);
        }

        if (game.exitAction.WasPressedThisFrame())
        {
            game.ChangeState(game.PausedState);
        }
        //game.level = (int)(game.playingTimePassed/10); //change level every 200 seconds
    }

    public override void ExitState(GameManager game) {
        //game.speedOfTime = 1f;
    }

    public override string GetStateName() => "PlayingState";
}

