using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleSpawner : MonoBehaviour //provide quick collectible spawning and ui for prefab
{
    [Header("Spawning")]
    public GameObject coinPrefab;
    public GameObject lifePrefab;
    public GameObject bombPrefab;

    public Transform[] spawnPoints;

    public static CollectibleSpawner Instance { get; private set; }
    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManagers
        }

        //get spawn points automatically
        refreshSpawnPoints();
    }

    private void refreshSpawnPoints()
    {
        spawnPoints = new Transform[Instance.transform.childCount];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = Instance.transform.GetChild(i);
        }
    }

    private void OnTransformChildrenChanged()
    {
        refreshSpawnPoints();
    }

    private float nextSpawnTime = 0f;

    //public float spawnRate = 2f;
    void Update()
    {
        //SpawnDetermination();
    }
    /*
    void SpawnDetermination()
    {
        switch (GameManager.Instance.level) //TEMPORARY
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
    }*/

    private void SpawnCollectible(GameObject collectiblePrefab)
    {
        if (collectiblePrefab && spawnPoints.Length > 0)
        {
            // Check if game is still running through Singleton
            if (GameManager.Instance.lives > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                int randomX = Random.Range(-7, 7);
                Instantiate(collectiblePrefab, spawnPoints[randomIndex].position+(new Vector3(randomX,0,0)), Quaternion.identity);
            }
        }
    }

    public void SpawnCoin() => SpawnCollectible(coinPrefab); //yup again
    public void SpawnLife() => SpawnCollectible(lifePrefab);
}