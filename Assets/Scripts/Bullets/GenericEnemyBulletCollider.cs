using UnityEngine;

public class GenericEnemyBulletCollider : MonoBehaviour
{

    private void Start()
    {
        EventManager.Subscribe("OnPlayerDeath", Vanish);
        EventManager.Subscribe("OnBomb", Vanish);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnPlayerDeath", Vanish);
        EventManager.Unsubscribe("OnBomb", Vanish);
    }

    private void Vanish()
    {
        Destroy(gameObject );
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }



    }
}
