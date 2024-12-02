using UnityEngine;

/// <summary>
/// Represents a target in the shooting game that can receive damage and be destroyed
/// Implements the ITargetable interface for damage handling
/// </summary>
public class ShootingTarget : MonoBehaviour, ITargetable
{
    [Header("Health Configuration")]
    [Tooltip("Maximum health points for the target before destruction")]
    [SerializeField] private float maxHealth = 100f;

    [Tooltip("Optional visual effect to play when target is destroyed")]
    [SerializeField] private GameObject destructionEffect;

    [Header("Debug Settings")]
    [Tooltip("Enable detailed logging for target interactions")]
    [SerializeField] private bool enableDebugLogging = true;

    // Current health of the target
    private float currentHealth;

    /// <summary>
    /// Initialize the target's health when the game object starts
    /// </summary>
    void Start()
    {
        // Set current health to maximum health at start
        currentHealth = maxHealth;
        
        if (enableDebugLogging)
            Debug.Log($"[ShootingTarget] {gameObject.name} initialized with {maxHealth} health");
    }

    /// <summary>
    /// Applies damage to the target and handles destruction logic
    /// </summary>
    /// <param name="damage">Amount of damage to apply</param>
    public void TakeDamage(float damage)
    {
        // Reduce current health by damage amount
        currentHealth -= damage;
        
        if (enableDebugLogging)
            Debug.Log($"[ShootingTarget] {gameObject.name} took {damage} damage. Remaining health: {currentHealth}");

        // Check if target is destroyed
        if (currentHealth <= 0)
        {
            // Spawn destruction effect if configured
            if (destructionEffect != null)
            {
                Instantiate(destructionEffect, transform.position, Quaternion.identity);
                
                if (enableDebugLogging)
                    Debug.Log($"[ShootingTarget] {gameObject.name} destruction effect spawned");
            }

            // Award points for destroying the target
            ScoreManager.Instance.AddScore(10);
            
            if (enableDebugLogging)
                Debug.Log($"[ShootingTarget] {gameObject.name} destroyed. 10 points awarded");

            // Destroy the target game object
            Destroy(gameObject);
        }
    }
}
