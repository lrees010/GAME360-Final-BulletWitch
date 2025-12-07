using UnityEngine;

public class GenericEnemyBulletCollider : MonoBehaviour
{
    //Provides generic behavior for individual bullets from enemies
    private void Start()
    {
        EventManager.Subscribe("OnPlayerDeath", Vanish); //vanish when player dies
        EventManager.Subscribe("OnBomb", Vanish); //vanish on bomb
        EventManager.Subscribe("OnLevelChanged", LevelChanged); //vanish when the level changes
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnPlayerDeath", Vanish);
        EventManager.Unsubscribe("OnBomb", Vanish);
        EventManager.Unsubscribe("OnLevelChanged", LevelChanged);
    }
    private void LevelChanged(object data) 
    {
        Vanish();
    }
    private void Vanish()
    {
        Destroy(gameObject );
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //destroy self when colliding with player
        {
            Destroy(gameObject);
        }



    }
}
