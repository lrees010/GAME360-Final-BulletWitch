using System;
using UnityEngine;

public class ObsidianFiringState : ObsidianState
{
    private float startTime;
    public override void EnterState(ObsidianManager obsidian)
    {
        startTime = Time.time;

    }


    public override void UpdateState(ObsidianManager obsidian)
    {

        if (Time.time - startTime < 10f)//if enough time passes, move to chase state
        {
            //move left and right and wave up and down, using sine functions
            float xPos = (float)(
                Math.Sin(Time.time)
                * 9.5);
            obsidian.MoveTo(new Vector3(xPos, 3+ (float)Math.Sin((Time.time-startTime)*3), 0), 2f, 5f);
            //obsidian.SnapTo(new Vector3(xPos, 3 + (float)Math.Sin((Time.time - startTime) * 3), 0));
            obsidian.Shoot(0.15f);
        }
        else
        {
            obsidian.ChangeState(obsidian.ObsidianChaseState);
        }
    }


    public override void ExitState(ObsidianManager obsidian) {
    }

    public override string GetStateName() => "ObsidianFiringState";
}

