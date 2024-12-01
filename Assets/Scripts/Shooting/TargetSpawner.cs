using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the spawning of targets, guns, and game state
/// </summary>
public class TargetSpawner : MonoBehaviour
{
    [Header("Target Spawner Configuration")]
    
    [Tooltip("Array of target prefabs to be randomly spawned")]
    [SerializeField] private GameObject[] targetPrefabs;
    
    [Tooltip("Possible spawn locations for targets")]
    [SerializeField] private Transform[] spawnPoints;

    [Space(5)]
    
    [Tooltip("Time interval between target spawns in seconds")]
    [SerializeField] private float spawnInterval = 3f;

    [Tooltip("Duration (in seconds) before spawned targets are destroyed")]
    [SerializeField] private float targetLifetime = 5f;

    [Space(5)]    

    [Header("Gun Spawning")]
    [SerializeField] private GameObject[] gunPrefabs;
    [SerializeField] private Transform[] gunSpawnPoints;

    [Space(5)]

    [Header("Sound Effects")]
    [Tooltip("Sound effect to play when a target spawns")]
    [SerializeField] private AudioClip targetSpawnSound;

    [Tooltip("Audio source to play spawn sound")]
    [SerializeField] private AudioSource audioSource;

    [Space(5)]

    [Header("Game State")]
    [Tooltip("Duration of the game in seconds")]
    [SerializeField] private float gameDuration = 60f;

    [Space(5)]

    [Header("Timer Visualization")]
    [SerializeField] private TextMeshProUGUI timerText; // Reference to UI Text component
    [SerializeField] private Image timerFillImage; // Optional: fill image for timer bar
    private List<GameObject> spawnedTargets = new List<GameObject>();
    private List<GameObject> spawnedGuns = new List<GameObject>();
    [SerializeField]private bool isGameRunning = false;
    private float gameTimer = 0f;

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

    void  Update() 
    {
        // Manage game timer
        if (isGameRunning)
        {
            gameTimer -= Time.deltaTime;
            UpdateTimerDisplay();

            // End game when timer reaches zero
            if (gameTimer <= 0)
            {
                ResetGame();
            }
        }
    }

    /// <summary>
    /// Starts the game, enabling target and gun spawning
    /// </summary>
    public void StartGame()
    {
        // Reset any existing game state
        ResetGame();

        // Start game
        isGameRunning = true;
        gameTimer = gameDuration;

        // Start spawning targets
        InvokeRepeating(nameof(SpawnTarget), 0f, spawnInterval);

        // Spawn initial guns
        SpawnGuns();

        Debug.Log("Game Started! Spawning targets and guns.");
    }

    // <summary>
    /// Resets the game state, clearing all spawned objects
    /// </summary>
    public void ResetGame()
    {
        // Stop any ongoing spawning
        CancelInvoke(nameof(SpawnTarget));

        // Clear existing targets
        foreach (GameObject target in spawnedTargets)
        {
            if (target != null)
                Destroy(target);
        }
        spawnedTargets.Clear();

        // Clear existing guns
        foreach (GameObject gun in spawnedGuns)
        {
            if (gun != null)
                Destroy(gun);
        }
        spawnedGuns.Clear();

        // Reset game state
        isGameRunning = false;
        gameTimer = 0f;

        Debug.Log("Game Reset. All targets and guns cleared.");
    }

    /// <summary>
    /// Spawns a random target at a random spawn point
    /// </summary>
    void SpawnTarget()
    {
        // Check game state and spawning conditions
        if (!isGameRunning || targetPrefabs.Length == 0 || spawnPoints.Length == 0)
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

        // Play spawn sound effect
        if (targetSpawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(targetSpawnSound);
        }

        // Track and destroy the target
        spawnedTargets.Add(spawnedTarget);
        Destroy(spawnedTarget, targetLifetime);

        Debug.Log($"Spawned target {spawnedTarget.name} at {randomSpawnPoint.name}");
    }

    /// <summary>
    /// Spawns guns at predefined spawn points
    /// </summary>
    void SpawnGuns()
    {
        // Validate gun spawning
        if (gunPrefabs.Length == 0 || gunSpawnPoints.Length == 0)
        {
            Debug.LogWarning("No gun prefabs or spawn points assigned!");
            return;
        }

        // Spawn guns at available spawn points
        foreach (Transform spawnPoint in gunSpawnPoints)
        {
            // Select a random gun prefab
            GameObject randomGun = gunPrefabs[Random.Range(0, gunPrefabs.Length)];

            // Instantiate the gun
            GameObject spawnedGun = Instantiate(
                randomGun, 
                spawnPoint.position, 
                spawnPoint.rotation
            );

            spawnedGuns.Add(spawnedGun);
            Debug.Log($"Spawned gun {spawnedGun.name} at {spawnPoint.name}");
        }
    }
    
    
    #region Timer Management
    /// <summary>
    /// Updates the timer display with current game time
    /// </summary>
    private void UpdateTimerDisplay()
    {
        DisplayTimerText();
        UpdateTimerFill();
    }

    /// <summary>
    /// Displays the remaining time in minutes:seconds format
    /// </summary>
    private void DisplayTimerText()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Updates the timer fill image to represent remaining time
    /// </summary>
    private void UpdateTimerFill()
    {
        if (timerFillImage == null) return;
        
        timerFillImage.fillAmount = gameTimer / gameDuration;
    }

    /// <summary>
    /// Shows the timer display elements
    /// </summary>
    private void ShowTimerDisplay()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(true);
        
        if (timerFillImage != null)
            timerFillImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the timer display elements
    /// </summary>
    private void HideTimerDisplay()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(false);
        
        if (timerFillImage != null)
            timerFillImage.gameObject.SetActive(false);
    }
    #endregion
}
