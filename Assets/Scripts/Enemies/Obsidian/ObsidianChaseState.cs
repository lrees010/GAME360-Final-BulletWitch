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

            //limit horizontal speed
            obsidian.rb.linearVelocity = new Vector2(Mathf.Clamp(obsidian.rb.linearVelocity.x, -4f, 4f), obsidian.rb.linearVelocity.y);
        }
        else
        {
            obsidian.ChangeState(obsidian.ObsidianRestingState);
        }
    }

    public override void ExitState(ObsidianManager obsidian) {
        
    }

    public override string GetStateName() => "ObsidianChaseState";
}

