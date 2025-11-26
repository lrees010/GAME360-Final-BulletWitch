using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject enemyPrefab;
    public float spawnRate = 2f;
    public Transform[] spawnPoints;

    private float nextSpawnTime = 0f;

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
        spawnRate = 10f/((float)GameManager.Instance.score/50f);
        spawnRate = Mathf.Clamp(spawnRate, 0.1f, 3f);
    }

    private void SpawnEnemy()
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