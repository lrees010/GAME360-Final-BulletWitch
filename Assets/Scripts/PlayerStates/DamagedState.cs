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

        //disappear
        player.rb.position = new Vector2(0, 40);

        //kill everything
        EventManager.TriggerEvent("OnPlayerDeath");
        //already doing "OnPlayerStateChanged", "Damaged"
    }

    public override void UpdateState(PlayerController player)
    {
        player.HandleSlowTime(player);
        damagedTicks = damagedTicks + Time.deltaTime;
        if (damagedTicks > 0.4f)
        {
            player.ChangeState(player.IdleState);
            player.damageCooldown = false;

            //come back!
            player.rb.position = new Vector2(0, -4);
        }
    }

    public override void ExitState(PlayerController player) { }

    public override string GetStateName() => "Damaged";
}
