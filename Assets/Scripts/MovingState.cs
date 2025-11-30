using UnityEngine;
using UnityEngine.InputSystem;

public class MovingState : PlayerState
{
    public override void EnterState(PlayerController player)
    {
        //Debug.Log("Entered MovingState");
    }

    public override void UpdateState(PlayerController player)
    {
        HandleMovement(player);
        player.HandleShooting(player);
        player.HandleSlowTime(player);
    }

    private void HandleMovement(PlayerController player)
    {
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = player.moveAction.ReadValue<Vector2>();
        player.rb.linearVelocity = movement * player.moveSpeed;

        if (Mathf.Abs(movement.x) + Mathf.Abs(movement.y) < 0.1f)
        {
            player.ChangeState(player.IdleState);
        }
    }



    public override void ExitState(PlayerController player) { }

    public override string GetStateName() => "Moving";

}
