using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleSpawner : MonoBehaviour 
{
    //Singleton to provide quick collectible spawning methods for LevelStates


    [Header("Spawning")]
    public GameObject coinPrefab;
    public GameObject lifePrefab;
    public GameObject bombPrefab;

    public Transform[] spawnPoints; //possible spawn points

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

    private void refreshSpawnPoints() //Refreshes to get children Transforms automatically, and use their y positions as spawn points for collectibles
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

    private void SpawnCollectible(GameObject collectiblePrefab) //method for instantiating collectible prefabs onto random spawn points, and randomizing their x position
    {
        if (collectiblePrefab && spawnPoints.Length > 0) //if the prefab exists, and the spawnpoints exist
        {
            // Check if game is still running through Singleton
            if (GameManager.Instance.lives > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                int randomX = Random.Range(-7, 7); //randomize x position
                Instantiate(collectiblePrefab, spawnPoints[randomIndex].position+(new Vector3(randomX,0,0)), Quaternion.identity);
            }
        }
    }

    //public methods for spawning certain collectibles
    public void SpawnCoin() => SpawnCollectible(coinPrefab);
    public void SpawnLife() => SpawnCollectible(lifePrefab);

    public void SpawnBomb() => SpawnCollectible(bombPrefab);
}