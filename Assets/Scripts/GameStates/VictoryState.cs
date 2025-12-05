using System;
using UnityEngine;

public class VictoryState : GameState
{
    public override void EnterState(GameManager game)
    {
        Debug.Log("Entered VictoryState");
 

        EventManager.TriggerEvent("OnVictory");
        EventManager.ClearAllEvents();
        //Time.timeScale = 0f; // Pause the game

        AudioManager.Instance.PauseMusic(true);
    }


    public override void UpdateState(GameManager game)
    {
        GameManager.Instance.speedOfTime = 0f;
    }

    public override void ExitState(GameManager game) { }

    public override string GetStateName() => "VictoryState";
}

