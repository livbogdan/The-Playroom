using UnityEngine;
using Oculus.Interaction;
using System.Collections;
using TMPro; // Add TextMeshPro namespace

/// <summary>
/// Manages gun mechanics including shooting, grabbing, and input handling for VR interactions
/// </summary>
public class GunController : MonoBehaviour
{
    [Header("Shooting Configuration")]
    [Tooltip("Transform point where bullets will be spawned and fired from")]
    [SerializeField] private Transform muzzlePoint;

    [Tooltip("Prefab of the bullet to be instantiated when shooting")]
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Rigidbody bulletRb;
    
    [Header("Bullet Limits")]
    [Tooltip("Maximum number of bullets that can be fired")]
    [SerializeField] private int maxBullets = 10;
    
    [Tooltip("Text to display current bullet count")]
    [SerializeField] private TextMeshProUGUI bulletCountText;
    
    [Tooltip("Text to display cooldown status")]
    [SerializeField] private TextMeshProUGUI cooldownText;


    [Tooltip("Cooldown time between shots in seconds")]
    [SerializeField] private float shootCooldown = 0.3f;

    [Header("Interaction References")]
    [Tooltip("Grab interaction component for the gun")]
    [SerializeField] private GrabInteractable grabInteractable; 

    // Internal state tracking
    private bool isGrabbed = false;
    private bool canShoot = true;
    private int currentBulletCount;
    
    /// <summary>
    /// Initialize grab interaction when the script starts
    /// </summary>
    private void Awake()
    {
        // Initialize bullet count
        currentBulletCount = maxBullets;
        UpdateBulletCountText();

        // Automatically find GrabInteractable if not manually assigned
        if (grabInteractable == null)
        {
            grabInteractable = GetComponent<GrabInteractable>();
        }
        
        // Subscribe to grab events
        grabInteractable.WhenPointerEventRaised += HandleGrabEvents;
    }

    /// <summary>
    /// Handles gun grab and release events
    /// </summary>
    /// <param name="event">Pointer interaction event</param>
    private void HandleGrabEvents(PointerEvent @event)
    {
        switch (@event.Type)
        {
            case PointerEventType.Select:
                isGrabbed = true; // Gun is grabbed
                break;
            case PointerEventType.Unselect:
                isGrabbed = false; // Gun is released
                break;
        }
    }

    /// <summary>
    /// Check for shooting input when gun is grabbed
    /// </summary>
    private void Update()
    {
        if (isGrabbed && InputIsPressed())
        {
            Shoot();
        }
    }

    /// <summary>
    /// Checks if the trigger is pressed on either hand
    /// </summary>
    /// <returns>True if trigger is pressed, false otherwise</returns>
    private bool InputIsPressed()
    {
        return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || 
               OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
    }

    /// <summary>
    /// Instantiates and fires a bullet with force
    /// </summary>
    private void Shoot()
    {
        // Check if can shoot and bullets remain
        if (!canShoot || currentBulletCount <= 0) return;

        // Create bullet at muzzle point
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        
        
        // Reduce bullet count
        currentBulletCount--;
        UpdateBulletCountText();
        
        // Start shooting cooldown
        StartCoroutine(ShootCooldown());
    }

    /// <summary>
    /// Manages shooting cooldown to prevent rapid fire
    /// </summary>
    /// <returns>Coroutine that waits for cooldown duration</returns>
    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        
        // Update cooldown text
        if (cooldownText != null)
        {
            cooldownText.text = $"Cooling Down... {currentBulletCount}";
        }

        yield return new WaitForSeconds(shootCooldown);
        
        canShoot = true;
        
        // Clear cooldown text
        if (cooldownText != null)
        {
            cooldownText.text = "Ready to Shoot";
        }

        // If bullets are full, update bullet count text
        if (currentBulletCount == maxBullets)
        {
            cooldownText.text = " ";
        }
    }
    private void UpdateBulletCountText()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = $"Bullets: {currentBulletCount}/{maxBullets}";
        }

        // Hide cooldown text when bullets are full
        if (cooldownText != null)
        {
            cooldownText.gameObject.SetActive(currentBulletCount < maxBullets);
        }
    }

    // Optional: Method to reload bullets
    public void Reload()
    {
        currentBulletCount = maxBullets;
        UpdateBulletCountText();
    }
}