using System;
using UnityEngine;

public class ObsidianChaseState : ObsidianState
{
    private float startTime;
    public override void EnterState(ObsidianManager obsidian)
    {
        startTime = Time.time;

    }


    public override void UpdateState(ObsidianManager obsidian)
    {
        if (Time.time-startTime<10f)
        {
            obsidian.SideShoot(0.4f);
            obsidian.ChasePlayer();
        }
        else
        {
            obsidian.ChangeState(obsidian.ObsidianFiringState);
        }
    }

    public override void ExitState(ObsidianManager obsidian) { }

    public override string GetStateName() => "ObsidianChaseState";
}

