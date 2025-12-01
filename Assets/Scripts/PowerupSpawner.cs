using System.Collections;
using UnityEngine;
public class PowerupSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] powerupPrefabs;

    [Header("Spawning Controls")]
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 4f;

    public float spawnRangeY = 2f;

    public float spawnDistanceX = 20f;

    private Transform playerTransform;

    void Start()
    {
        // Find the player's transform (assuming your player is tagged "Player")
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Start the infinite spawning routine
        StartCoroutine(SpawnPowerupsRoutine());
    }

    IEnumerator SpawnPowerupsRoutine()
    {
        while (true) // Loop forever
        {
            // 1. Wait for a random time interval
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // 2. Decide if a powerup should spawn at all (optional)
            // Example: 75% chance to spawn
            if (Random.value > 0.25f)
            {
                SpawnRandomPowerup();
            }
        }
    }

    void SpawnRandomPowerup()
    {
        if (powerupPrefabs.Length == 0 || playerTransform == null)
        {
            Debug.LogError("Powerup Prefabs not set up or Player not found!");
            return;
        }

        // --- WHAT to Spawn (Randomly select a prefab) ---
        int randomIndex = Random.Range(0, powerupPrefabs.Length);
        GameObject powerupToSpawn = powerupPrefabs[randomIndex];


        // --- WHERE to Spawn (Calculate the position) ---

        Vector3 spawnPosition = playerTransform.position;

        spawnPosition.y += Random.Range(-spawnRangeY, spawnRangeY);
        spawnPosition.x += spawnDistanceX;

        Instantiate(powerupToSpawn, spawnPosition, Quaternion.identity);
    }
}
