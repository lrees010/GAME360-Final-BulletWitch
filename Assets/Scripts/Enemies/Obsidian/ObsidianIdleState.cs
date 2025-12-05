using System;
using UnityEngine;

public class ObsidianIdleState : ObsidianState
{
    private float startTime;
    public override void EnterState(ObsidianManager obsidian)
    {
        startTime = Time.time;

    }

    private void IdleMovement(ObsidianManager obsidian)
    {
        obsidian.MoveTo(new Vector3(0, 3 + (float)(Math.Sin(Time.time)*0.5), 0), 0.2f, 2f);
    }

    public override void UpdateState(ObsidianManager obsidian)
    {
        if(DialogueManager.Instance.isDialogueActive == true)
        {
            IdleMovement(obsidian);
            return;
        }
        else
        {
            obsidian.ChangeState(obsidian.ObsidianRestingState);
        }
    }


    public override void ExitState(ObsidianManager obsidian) { }

    public override string GetStateName() => "ObsidianIdleState";
}

