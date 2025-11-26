using System;
using UnityEngine;

public class ForestState : LevelState
{
    public override void EnterState(LevelManager level)
    {
        Debug.Log("Entered ForestState");

    }


    public override void UpdateState(LevelManager level)
    {

    }

    public override void ExitState(LevelManager level) { }

    public override string GetLevelName() => "Forest";
}

