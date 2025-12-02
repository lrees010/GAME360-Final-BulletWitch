using System;
using UnityEngine;

public class GameOverState : GameState
{
    public override void EnterState(GameManager game)
    {
        Debug.Log("Entered GameOverState");
        
    }


    public override void UpdateState(GameManager game)
    {
        //GameManager.Instance.speedOfTime = 0f;
    }

    public override void ExitState(GameManager game) { }

    public override string GetStateName() => "GameOverState";
}

