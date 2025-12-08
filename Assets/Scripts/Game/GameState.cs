using UnityEngine;

public abstract class GameState
{
    public abstract void EnterState(GameManager game);
    public abstract void UpdateState(GameManager game);
    public abstract void ExitState(GameManager game);
    public abstract string GetStateName();
}