using UnityEngine;

public class GenericBulletCollider : MonoBehaviour
{
    //Provides generic behavior for individual bullets from Bloom and Wave bullet
    private void Start()
    {
        EventManager.Subscribe("OnPlayerDeath", Vanish); //vanish when player dies
        EventManager.Subscribe("OnBomb", Vanish); //vanish when bomb used
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnPlayerDeath", Vanish);
        EventManager.Unsubscribe("OnBomb", Vanish);
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) //destroy when colliding with enemy
        {
            Destroy(gameObject);
        }
    }
}
