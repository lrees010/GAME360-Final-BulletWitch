using UnityEngine;

public class PausedState : GameState
{
    public override void EnterState(GameManager game)
    {
        Debug.Log("Entered PausedState");
        EventManager.TriggerEvent("OnPause", "Pause"); //trigger pause even

        AudioManager.Instance.PauseMusic(true); //pause music
    }

    public override void UpdateState(GameManager game)
    {
        game.speedOfTime = 0f; //freeze time
        if (game.exitAction.WasPressedThisFrame()) //if esc is pressed, change back to playingstate (does same thing as resume button)
        {
            game.ChangeState(game.PlayingState);
        }
    }



    public override void ExitState(GameManager game) { 
        game.speedOfTime = 1f; //set speed of time back to normal when unpausing
        EventManager.TriggerEvent("OnPause", "Unpause");
        AudioManager.Instance.PauseMusic(false); //unpause music
    }

    public override string GetStateName() => "Moving";
}
