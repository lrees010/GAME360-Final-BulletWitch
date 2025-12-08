using UnityEngine;

public abstract class LevelState //aka wave
{
    public abstract void EnterState(LevelManager level);
    public abstract void UpdateState(LevelManager level);
    public abstract void ExitState(LevelManager level);
    public abstract string GetLevelName();
}