using UnityEngine;

public class MainMenuState : GameState
{
    public override void EnterState(GameManager game)
    {
        Debug.Log("Entered MainMenuState");

    }

    public override void UpdateState(GameManager game)
    {

    }



    public override void ExitState(GameManager game) { }

    public override string GetStateName() => "MainMenuState";
}
