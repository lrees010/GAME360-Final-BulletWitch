using System;
using UnityEngine;

public class ObsidianIdleState : ObsidianState
{
    //idle state for dialogue

    private float startTime;
    public override void EnterState(ObsidianManager obsidian)
    {
        startTime = Time.time;

    }

    private void IdleMovement(ObsidianManager obsidian)
    {
        obsidian.MoveTo(new Vector3(0, 3 + (float)(Math.Sin(Time.time)*0.5), 0), 0.2f, 2f); //hover up and down
    }

    public override void UpdateState(ObsidianManager obsidian)
    {
        if(DialogueManager.Instance.isDialogueActive == true) //while dialogue is happening, remain idle
        {
            IdleMovement(obsidian);
            return;
        }
        else
        {
            obsidian.ChangeState(obsidian.ObsidianRestingState); //once dialogue is finished, move to resting state which begins attack patterns
        }
    }


    public override void ExitState(ObsidianManager obsidian) { }

    public override string GetStateName() => "ObsidianIdleState";
}

