using UnityEngine;

/// <summary>
/// Manages the behavior of a bullet in the game
/// Handles damage, lifetime, and collision interactions
/// </summary>
public class BulletController : MonoBehaviour
{
    [Header("Bullet Configuration")]
    [Tooltip("Amount of damage the bullet will inflict on impact")]
    [SerializeField] private float damage = 25f;

    [Tooltip("Time in seconds before the bullet is automatically destroyed")]
    [SerializeField] private float lifetime = 5f;

    [Tooltip("Visual effect to spawn upon bullet collision")]
    [SerializeField] private GameObject hitEffect;

    /// <summary>
    /// Initialize bullet self-destruction timer
    /// </summary>
    private void Start()
    {
        // Destroy bullet after specified lifetime to prevent infinite existence
        Destroy(gameObject, lifetime);
        Debug.Log($"Bullet spawned with {damage} damage and {lifetime}s lifetime");
    }

    /// <summary>
    /// Handle collision with other game objects
    /// </summary>
    /// <param name="collision">Collision data from the impact</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object can take damage
        if (collision.gameObject.TryGetComponent<ITargetable>(out var target))
        {
            // Apply damage to the target
            target.TakeDamage(damage);
            Debug.Log($"Bullet hit {collision.gameObject.name}, dealt {damage} damage");

            // Spawn hit effect if configured
            if (hitEffect != null)
            {
                Instantiate(hitEffect, 
                    collision.contacts[0].point, 
                    Quaternion.LookRotation(collision.contacts[0].normal));
                
                Debug.Log("Hit effect spawned on collision");
            }
        }
        else
        {
            Debug.Log($"Bullet hit non-damageable object: {collision.gameObject.name}");
        }

        // Destroy bullet on impact
        Destroy(gameObject);
    }

    /// <summary>
    /// Optional method to modify bullet behavior before destruction
    /// </summary>
    private void OnDestroy()
    {
        Debug.Log("Bullet destroyed");
    }
}