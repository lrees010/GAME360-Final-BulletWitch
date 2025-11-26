using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject chargerPrefab;
    
    public Transform[] spawnPoints;

    private float nextSpawnTime = 0f;

    public float spawnRate = 2f;
    void Update()
    {
        SpawnDetermination();
    }

    void SpawnDetermination()
    {
        switch (GameManager.Instance.level)
        {
            case 1: //forest
                spawnRate = 1f;
                if (Time.time >= nextSpawnTime)
                {
                    SpawnEnemy(chargerPrefab);
                    nextSpawnTime = Time.time + spawnRate;
                }
                break;
            case 2: //cave
                spawnRate = 0.1f;
                if (Time.time >= nextSpawnTime)
                {
                    SpawnEnemy(chargerPrefab);
                    //spawn cave creatures
                    nextSpawnTime = Time.time + spawnRate;
                }
                break;
        }
    }
    public void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab && spawnPoints.Length > 0)
        {
            // Check if game is still running through Singleton
            if (GameManager.Instance.lives > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
            }
        }
    }
}