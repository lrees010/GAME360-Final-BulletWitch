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
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.rb.position = new Vector2(0, -4);

        //play death sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.DieSound);

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
            //come back!
            
            player.GetComponent<SpriteRenderer>().enabled = true;
            player.damageCooldown = false;

            player.ChangeState(player.IdleState);
            

            
        }
    }

    public override void ExitState(PlayerController player) {

    }

    public override string GetStateName() => "Damaged";
}
