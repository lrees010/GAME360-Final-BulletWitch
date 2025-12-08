using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    //Singleton to provide quick enemy spawning methods for LevelStates

    [Header("Spawning")]
    public GameObject chargerPrefab;
    public GameObject spiderPrefab;
    public GameObject sharkPrefab;

    public GameObject spitterPrefab;

    public GameObject obsidianPrefab;

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

    private void refreshSpawnPoints() //Refreshes to get children Transforms automatically
    {
        spawnPoints = new Transform[Instance.transform.childCount];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = Instance.transform.GetChild(i);
        }
    }

    private void OnTransformChildrenChanged() //automatically refresh spawn point references if they change
    {
        refreshSpawnPoints();
    }
    private void SpawnEnemy(GameObject enemyPrefab) //method for instantiating enemy prefabs onto random spawn points
    {
        if (enemyPrefab && spawnPoints.Length > 0 && GameManager.Instance.powerupActive == false) //if prefab & spawnpoints exist, and player isn't using bomb powerup
        {
            // Check if game is still running through Singleton
            if (GameManager.Instance.lives > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
            }
        }
    }

    private void SpawnBoss(GameObject enemyPrefab) //method for instantiating boss prefab onto random spawn point, with no pass through so boss always spawns
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }

    //public methods for spawning certain enemies and bosses
    public void SpawnCharger() => SpawnEnemy(chargerPrefab); 

    public void SpawnObsidian() => SpawnBoss(obsidianPrefab);
    public void SpawnSpider() => SpawnEnemy(spiderPrefab); 

    public void SpawnShark() => SpawnEnemy(sharkPrefab);

    public void SpawnSpitter() => SpawnEnemy(spitterPrefab);
}