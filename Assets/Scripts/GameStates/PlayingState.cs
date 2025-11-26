using System;
using UnityEngine;

public class PlayingState : GameState
{
    public override void EnterState(GameManager game)
    {
        Debug.Log("Entered PlayingState");
        
    }


    public override void UpdateState(GameManager game)
    {
        if (game.speedOfTime > 0f)
        {
            game.timePassed = game.timePassed + (Time.deltaTime / game.speedOfTime); //time passed always counts real seconds no matter what the speed of time is
            game.playingTimePassed = game.playingTimePassed + Time.deltaTime;
        }
        //game.level = (int)(game.playingTimePassed/10); //change level every 200 seconds
    }

    public override void ExitState(GameManager game) { }

    public override string GetStateName() => "PlayingState";
}

