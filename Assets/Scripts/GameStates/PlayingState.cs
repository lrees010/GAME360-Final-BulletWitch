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
    private float slowingInSpeed = 0.2f;
    public override void UpdateState(GameManager game)
    {
        if (DialogueManager.Instance == null)
        {
            Debug.Log("no dialogue manager!");
            return;
        }
        if (game.speedOfTime > 0f && !DialogueManager.Instance.isDialogueActive) //if time isn't frozen, and dialogue isn't happening
        {
            game.timePassed = game.timePassed + (Time.deltaTime / game.speedOfTime); //time passed always counts real seconds no matter what the speed of time is
            
            game.playingTimePassed = game.playingTimePassed + Time.deltaTime;

            if (game.timePassed > game.timeLimit) //if ran out of time..
            {
                game.ChangeState(game.GameOverState);
            }
            else
            {
                EventManager.TriggerEvent("OnTimeLeftChanged"); //update ui
            }
        }
        

        if (game.slowingTime == true) //if player is using slow time button
        {
            if (slowedTime == false)
            {
                lastSlowTime = Time.time;
            }
            slowedTime = true;

            game.speedOfTime = Mathf.Clamp( //smoothly change speed of time
                ((slowingInSpeed - (Time.time - lastSlowTime)) / slowingInSpeed),
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
            game.speedOfTime = Mathf.Clamp( //smoothly change speed of time
                (((Time.time - lastSlowTime)) / slowingSpeed)+game.speedOfSlowedTime,
                game.speedOfSlowedTime,
                1f);
        }

        if (game.exitAction.WasPressedThisFrame()) //if esc key is hit, pause the game
        {
            game.ChangeState(game.PausedState);
        }
        //game.level = (int)(game.playingTimePassed/10); //change level every 200 seconds
    }

    public override void ExitState(GameManager game) {

    }

    public override string GetStateName() => "PlayingState";
}

