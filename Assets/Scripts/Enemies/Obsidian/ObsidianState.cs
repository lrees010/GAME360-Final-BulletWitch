using UnityEngine;

public abstract class ObsidianState 
{
    public abstract void EnterState(ObsidianManager obsidian);
    public abstract void UpdateState(ObsidianManager obsidian);
    public abstract void ExitState(ObsidianManager obsidian);
    public abstract string GetStateName();
}