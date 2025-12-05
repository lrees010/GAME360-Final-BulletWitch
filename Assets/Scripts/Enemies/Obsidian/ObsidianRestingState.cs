using System;
using UnityEngine;

public class ObsidianRestingState : ObsidianState
{
    private float startTime;
    public override void EnterState(ObsidianManager obsidian)
    {
        startTime = Time.time;

    }

    private void IdleMovement(ObsidianManager obsidian)
    {
        obsidian.MoveTo(new Vector3(0, 3, 0), 1f, 3f);
    }

    public override void UpdateState(ObsidianManager obsidian)
    {
        if (Time.time - startTime > 3f)
        {
            obsidian.ChangeState(obsidian.ObsidianFiringState);
        }
        else
        {
            IdleMovement(obsidian);
        }
    }


    public override void ExitState(ObsidianManager obsidian) { }

    public override string GetStateName() => "ObsidianRestingState";
}

