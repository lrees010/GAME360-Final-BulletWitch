using UnityEngine;

public class PlayingState : GameState
{
    public override void EnterState(GameManager game)
    {
        Debug.Log("Entered PlayingState");
        
    }


    public override void UpdateState(GameManager game)
    {

   
    }

    public override void ExitState(GameManager game) { }

    public override string GetStateName() => "PlayingState";
}

