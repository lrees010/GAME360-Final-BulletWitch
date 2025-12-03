using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour //provide quick enemy spawning and ui for prefab
{
    [Header("Spawning")]
    public GameObject chargerPrefab;
    public GameObject spiderPrefab;
    public GameObject sharkPrefab;

    public Transform[] spawnPoints;

    public static EnemySpawner Instance { get; private set; }
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

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab && spawnPoints.Length > 0 && GameManager.Instance.powerupActive == false)
        {
            // Check if game is still running through Singleton
            if (GameManager.Instance.lives > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
            }
        }
    }

    public void SpawnCharger() => SpawnEnemy(chargerPrefab); //yup

    public void SpawnSpider() => SpawnEnemy(spiderPrefab); //yup

    public void SpawnShark() => SpawnEnemy(sharkPrefab);
}