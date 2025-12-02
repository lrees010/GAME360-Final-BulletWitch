using UnityEngine;

public class PausedState : GameState
{
    public override void EnterState(GameManager game)
    {
        Debug.Log("Entered PausedState");
        EventManager.TriggerEvent("OnPause", "Pause");

        AudioManager.Instance.PauseMusic(true);
    }

    public override void UpdateState(GameManager game)
    {
        game.speedOfTime = 0f;
        if (game.exitAction.WasPressedThisFrame())
        {
            game.ChangeState(game.PlayingState);
        }
    }



    public override void ExitState(GameManager game) { 
        game.speedOfTime = 1f;
        EventManager.TriggerEvent("OnPause", "Unpause");
        AudioManager.Instance.PauseMusic(false);
    }

    public override string GetStateName() => "Moving";
}
