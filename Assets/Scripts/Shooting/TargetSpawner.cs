using UnityEngine;

/// <summary>
/// Manages the spawning of targets at random intervals and locations
/// </summary>
public class TargetSpawner : MonoBehaviour
{
    [Header("Target Spawner Configuration")]
    
    [Tooltip("Array of target prefabs to be randomly spawned")]
    [SerializeField] private GameObject[] targetPrefabs;
    
    [Tooltip("Possible spawn locations for targets")]
    [SerializeField] private Transform[] spawnPoints;
    
    [Tooltip("Time interval between target spawns in seconds")]
    [SerializeField] private float spawnInterval = 3f;

    [Header("Sound Effects")]
    [Tooltip("Sound effect to play when a target spawns")]
    [SerializeField] private AudioClip targetSpawnSound;

    [Tooltip("Audio source to play spawn sound")]
    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// Initialize repeated target spawning when the script starts
    /// </summary>
    void Start()
    {
        // Validate spawn configuration
        if (targetPrefabs.Length == 0)
        {
            Debug.LogWarning("No target prefabs assigned to TargetSpawner!");
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned to TargetSpawner!");
        }

        // Create AudioSource if not assigned
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Start spawning targets at regular intervals
        InvokeRepeating(nameof(SpawnTarget), 0f, spawnInterval);
        Debug.Log($"Target Spawner initialized. Spawning targets every {spawnInterval} seconds");
    }

    /// <summary>
    /// Spawns a random target at a random spawn point
    /// </summary>
    void SpawnTarget()
    {
        // Check if spawning is possible
        if (targetPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            return;
        }

        // Select random target and spawn point
        GameObject randomTarget = targetPrefabs[Random.Range(0, targetPrefabs.Length)];
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate the target
        GameObject spawnedTarget = Instantiate(
            randomTarget, 
            randomSpawnPoint.position, 
            randomSpawnPoint.rotation
        );

         // Play spawn sound effect if configured
        if (targetSpawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(targetSpawnSound);
        }

        Debug.Log($"Spawned target {spawnedTarget.name} at {randomSpawnPoint.name}");
    }
}
