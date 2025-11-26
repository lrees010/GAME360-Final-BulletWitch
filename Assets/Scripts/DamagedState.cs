using UnityEngine;

public class DamagedState : PlayerState
{
    private float damagedTicks;
    public override void EnterState(PlayerController player)
    {
        //Debug.Log("Entered DamagedState");
        damagedTicks = 0f;
        player.damageCooldown = true;

        //stop movement
        player.rb.linearVelocity = new Vector2(0, 0);
    }

    public override void UpdateState(PlayerController player)
    {
        damagedTicks = damagedTicks + Time.deltaTime;
        if (damagedTicks > 0.4f)
        {
            player.ChangeState(player.IdleState);
            player.damageCooldown = false;
        }
    }

    public override void ExitState(PlayerController player) { }

    public override string GetStateName() => "Damaged";
}
