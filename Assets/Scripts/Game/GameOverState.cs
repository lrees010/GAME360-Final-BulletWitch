using System;
using UnityEngine;

public class GameOverState : GameState
{
    public override void EnterState(GameManager game)
    {
        Debug.Log("Entered GameOverState");
 
        Debug.Log("GAME OVER!");
        EventManager.TriggerEvent("OnGameOver"); //trigger ongameover event
        //EventManager.ClearAllEvents();
        //Time.timeScale = 0f; // Pause the game
        AudioManager.Instance.PlaySFX(AudioManager.Instance.GameoverSound); //play gameover sound
        AudioManager.Instance.PauseMusic(true); //pause the music
    }


    public override void UpdateState(GameManager game)
    {
        GameManager.Instance.speedOfTime = 0f; //stop time
    }

    public override void ExitState(GameManager game) { }

    public override string GetStateName() => "GameOverState";
}

