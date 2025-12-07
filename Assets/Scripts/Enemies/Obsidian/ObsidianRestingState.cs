using System;
using UnityEngine;

public class ObsidianRestingState : ObsidianState
{
    private float startTime;
    public override void EnterState(ObsidianManager obsidian)
    {
        startTime = Time.time;

    }

    private void RestingMovement(ObsidianManager obsidian)
    {
        obsidian.MoveTo(new Vector3(0, 3, 0), 1f, 3f); //stay in one place
    }

    public override void UpdateState(ObsidianManager obsidian)
    {
        if (Time.time - startTime > 3f)
        {
            obsidian.ChangeState(obsidian.ObsidianFiringState); //move to firing state after some time of resting
        }
        else
        {
            RestingMovement(obsidian); //perform resting movement while time isnt up
        }
    }


    public override void ExitState(ObsidianManager obsidian) { }

    public override string GetStateName() => "ObsidianRestingState";
}

