using UnityEngine;

public class PausedState : GameState
{
    public override void EnterState(GameManager game)
    {
        Debug.Log("Entered PausedState");
    }

    public override void UpdateState(GameManager game)
    {

    }



    public override void ExitState(GameManager game) { }

    public override string GetStateName() => "Moving";
}
