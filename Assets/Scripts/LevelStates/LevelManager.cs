using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    LevelState currentState;
    public ForestState ForestState = new ForestState();
    public CaveState CaveState = new CaveState();
    public ClearingState ClearingState = new ClearingState();

    private void Start()
    {
        ChangeState(ClearingState);
    }
    public void ChangeState(LevelState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);

        EventManager.TriggerEvent("OnLevelChanged", currentState.GetLevelName());
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }
}
