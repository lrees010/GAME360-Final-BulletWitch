using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    LevelState currentState;
    public ForestState ForestState = new ForestState();

    private void Start()
    {
        ChangeState(ForestState);
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
