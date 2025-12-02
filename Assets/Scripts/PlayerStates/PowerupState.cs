using UnityEngine;

public class PowerupState : PlayerState
{
    private float powerupTicks;
    public override void EnterState(PlayerController player)
    {
        //Debug.Log("Entered BombingState");
        powerupTicks = 0f;

        //be immune for a little
        player.damageCooldown = true;

        //stop movement
        player.rb.linearVelocity = new Vector2(0, 0);

        //already doing "OnPlayerStateChanged", "Damaged"
    }

    public override void UpdateState(PlayerController player)
    {
        powerupTicks = powerupTicks + Time.deltaTime;
        if (powerupTicks > 0.4f)
        {
            player.ChangeState(player.IdleState);
            player.damageCooldown = false;
        }
    }

    public override void ExitState(PlayerController player) { }

    public override string GetStateName() => "Powerup";
}
