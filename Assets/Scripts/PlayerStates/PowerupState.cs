using UnityEngine;

public class PowerupState : PlayerState
{
    private float powerupTicks;
    public override void EnterState(PlayerController player)
    {
        powerupTicks = 0f;

        //be immune for a little
        player.damageCooldown = true;

        //stop movement
        player.rb.linearVelocity = new Vector2(0, 0);


        GameManager.Instance.powerupActive = true;
    }

    public override void UpdateState(PlayerController player)
    {
        powerupTicks = powerupTicks + Time.deltaTime;
        if (powerupTicks > 0.4f) 
        {
            player.ChangeState(player.IdleState);//switch back to idle state after some time
            player.damageCooldown = false;
            GameManager.Instance.powerupActive = false;
        }
    }

    public override void ExitState(PlayerController player) {
        
    }

    public override string GetStateName() => "Powerup";
}
