using UnityEngine;

public class PausedState : GameState
{
    public override void EnterState(GameManager game)
    {
        Debug.Log("Entered PausedState");
        EventManager.TriggerEvent("OnPause", "Pause");

    }

    public override void UpdateState(GameManager game)
    {
        Time.timeScale = 0f;
        if (game.exitAction.WasPressedThisFrame())
        {
            game.ChangeState(game.PlayingState);
        }
    }



    public override void ExitState(GameManager game) { 
        game.speedOfTime = 1f;
        EventManager.TriggerEvent("OnPause", "Unpause");
    }

    public override string GetStateName() => "Moving";
}
