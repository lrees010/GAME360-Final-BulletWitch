using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    LevelState currentState;
    public ForestState ForestState = new ForestState();
    public CaveState CaveState = new CaveState();
    public ClearingState ClearingState = new ClearingState();
    public LakeState LakeState = new LakeState();
    public BeachState BeachState = new BeachState();
    public MountainState MountainState = new MountainState();

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

        //debug level skipping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //EnemyGoal = 0;
            ChangeState(MountainState);
            if (DialogueManager.Instance.isDialogueActive)
            {
                DialogueManager.Instance.EndDialogue();
            }
        }
    }
}
