using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab")]
    [Tooltip("Enemy prefab.")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject groundShockWavePrefab;

    [Header("Spawning")]
    [Tooltip("Interval to spwan.")]
    [SerializeField] private float spawnInterval = 30f;
    [Tooltip("Size of spawn area.")]
    [SerializeField] private Vector2 spawnArea = new Vector2(45f, 45f);


    private void OnEnable()
    {
        GameManager.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Playing)
        {
            InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
        }
        else
        {
            CancelInvoke(nameof(SpawnEnemy));
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("No prefab assigned.");
            return;
        }

        float randomX = Random.Range(-spawnArea.x / 2, spawnArea.x / 2);
        float randomZ = Random.Range(-spawnArea.y / 2, spawnArea.y / 2);
        float spawnY = 0.5f; //Assumes 1 height

        Vector3 spawnPosition = new Vector3(randomX, spawnY, randomZ);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);

        Vector3 shockWavePos = new Vector3(spawnPosition.x, 0.1f, spawnPosition.z);
        Instantiate(groundShockWavePrefab, shockWavePos, Quaternion.Euler(90, 0, 0));

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
