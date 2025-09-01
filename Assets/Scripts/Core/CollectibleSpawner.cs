using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public static CollectibleSpawner Instance { get; private set; }

    [Header("Prefab")]
    [Tooltip("Collectivle prefab to spawn.")]
    [SerializeField] private GameObject collectiblePrefab;

    [Header("Spawn Settings")]
    [Tooltip("Number of collectibles to spawn.")]
    [SerializeField] private int numberOfCollectibles = 20;

    [Tooltip("Size of area to spawn collectibles in (scale of level plane).")]
    [SerializeField] private Vector2 spawnArea = new Vector2(45f, 45f);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Check if prefab has been assigned before trying to spawn.
        if(collectiblePrefab == null)
        {
            Debug.LogError("Prefab not assigned to spawner.");
            return;
        }

        SpawnCollectibles();
    }

    private void SpawnCollectibles()
    {
        for (int i = 0; i < numberOfCollectibles; i++)
        {
            // Calc random position within defined area.
            float randomX = Random.Range(-spawnArea.x / 2, spawnArea.x / 2);
            float randomZ = Random.Range(-spawnArea.y / 2, spawnArea.y / 2);

            // Value based on collectible height to appear resting on plane.
            float spawnY = 0.25f;

            Vector3 spawnPosition = new Vector3(randomX, spawnY, randomZ);

            // Instatiate collectible. Transform -> children of spawner GO.
            Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity, transform);
        }
    }

    // Respawn single collectible.
    public void SpawnSingleCollectible()
    {
        float randomX = Random.Range(-spawnArea.x / 2, spawnArea.x / 2);
        float randomZ = Random.Range(-spawnArea.y / 2, spawnArea.y / 2);

        float spawnY = 0.25f;

        Vector3 spawnPosition = new Vector3(randomX, spawnY, randomZ);

        Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity, transform);
    }
}

// @Author F.B.