using UnityEngine;
using System.Collections;

public class SeagullSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject seagullPrefab;     
    public GameObject indicatorPrefab;   

    private Transform playerTransform;

    [Header("Spawn Settings (Runtime Adjusted)")]
    private float minSpawnTime;
    private float maxSpawnTime;
    public float spawnAheadDistance = 30f; 

    [Header("Vertical Range")]
    public float minHeightY = 2f;
    public float maxHeightY = 4f;

    [Header("Difficulty Scaling")]
    public float initialMinSpawnTime = 8f;   
    public float initialMaxSpawnTime = 15f;
    public float finalMinSpawnTime = 3f;     
    public float finalMaxSpawnTime = 5f;
    public float difficultyRampUpTime = 120f; 

    private float timeElapsed = 0f;

    void Start()
    {
        // Initialize the runtime spawn variables with the starting values
        minSpawnTime = initialMinSpawnTime;
        maxSpawnTime = initialMaxSpawnTime;

        // Find the player and start the routine
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerTransform == null)
        {
            Debug.LogError("Player not found! Tag the player GameObject as 'Player'.");
            return;
        }

        StartCoroutine(SpawnSeagullRoutine());
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        AdjustDifficulty();
    }

    void AdjustDifficulty()
    {
        float difficultyPercent = Mathf.Clamp01(timeElapsed / difficultyRampUpTime);

        minSpawnTime = Mathf.Lerp(initialMinSpawnTime, finalMinSpawnTime, difficultyPercent);
        maxSpawnTime = Mathf.Lerp(initialMaxSpawnTime, finalMaxSpawnTime, difficultyPercent);
    }


    IEnumerator SpawnSeagullRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            Vector3 seagullSpawnPoint = CalculateSpawnPosition();

            yield return StartCoroutine(ShowWarningThenSpawn(seagullSpawnPoint));
        }
    }

    Vector3 CalculateSpawnPosition()
    {
        float randomY = Random.Range(minHeightY, maxHeightY);
        float spawnX = playerTransform.position.x + spawnAheadDistance;

        return new Vector3(spawnX, randomY, 0f);
    }

    IEnumerator ShowWarningThenSpawn(Vector3 spawnPoint)
    {
        GameObject indicatorInstance = Instantiate(indicatorPrefab, spawnPoint, Quaternion.identity);
        WarningIndicator indicatorScript = indicatorInstance.GetComponent<WarningIndicator>();

        if (indicatorScript == null)
        {
            Debug.LogError("WarningIndicator script missing on prefab!");
            Destroy(indicatorInstance);
            yield break;
        }

        indicatorScript.SetupWarning(spawnPoint);
        yield return new WaitForSeconds(indicatorScript.warningDuration);

        Instantiate(seagullPrefab, spawnPoint, Quaternion.identity);

        Destroy(indicatorInstance);
    }
}
