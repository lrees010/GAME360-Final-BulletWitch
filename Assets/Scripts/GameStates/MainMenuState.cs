using UnityEngine;

public class MainMenuState : GameState
{
    public override void EnterState(GameManager game)
    {
        if (DialogueManager.Instance != null) //if dialoguemanager is in the scene
        {
            DialogueManager.Instance.EndDialogue(); //end current dialogue
        }
        AudioManager.Instance.PauseMusic(true); //pause music
        Debug.Log("Entered MainMenuState");
    }

    public override void UpdateState(GameManager game)
    {
    }



    public override void ExitState(GameManager game) { }

    public override string GetStateName() => "MainMenuState";
}
