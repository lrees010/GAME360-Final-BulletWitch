using UnityEngine;

public class IdleState : PlayerState
{
    public override void EnterState(PlayerController player)
    {
        //Debug.Log("Entered IdleState");
    }

    public override void UpdateState(PlayerController player)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0.1f)
        {
            player.ChangeState(player.MovingState);
        }

        player.HandleShooting(player);
    }


    public override void ExitState(PlayerController player) { }

    public override string GetStateName() => "Idle";

}
