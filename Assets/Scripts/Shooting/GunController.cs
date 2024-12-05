using UnityEngine;
using Oculus.Interaction;
using System.Collections;
using TMPro;

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
    
    [Header("Bullet Limits")]
    [Tooltip("Maximum number of bullets that can be fired")]
    [SerializeField] private int maxBullets = 10;

    [Tooltip("Enable infinite bullets")]
    [SerializeField] private bool infiniteBullets = false;
    
    [Tooltip("Text to display current bullet count")]
    [SerializeField] private TextMeshProUGUI bulletCountText;
    
    [Tooltip("Text to display cooldown status")]
    [SerializeField] private TextMeshProUGUI cooldownText;


    [Tooltip("Cooldown time between shots in seconds")]
    [SerializeField] private float shootCooldown = 0.3f;

    [Header("Interaction References")]
    [Tooltip("Grab interaction component for the gun")]
    [SerializeField] private GrabInteractable grabInteractable; 

    [Header("Audio")]
    [Tooltip("Audio source for shooting sound")]
    [SerializeField] private AudioClip shootingAudioSource;
    [Tooltip("Audio clip for reloading weapon")]
    [SerializeField] private AudioClip reloadAudioClip;

    [Header("Reload Settings")]
    [Tooltip("Time required to reload the weapon")]
    [SerializeField] private float reloadTime = 1.5f;

private bool isReloading = false;

    private OVRHand leftHand;
    private OVRHand rightHand;


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

        // Hide controllers when gun is grabbed
        FindHandReferences();
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
                HideControllers();
                break;
            case PointerEventType.Unselect:
                isGrabbed = false; // Gun is released
                ShowControllers();
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

        // New reload input - use B button on Oculus Touch controllers
        if (OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Four)) // B button
        {
            Reload();
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
        // Check if can shoot
        // Check if can shoot and has bullets
        if (!canShoot || currentBulletCount <= 0) 
        {
            if (currentBulletCount <= 0 && cooldownText != null)
            {
                cooldownText.text = "Need to reload!";
            }
            return;
        }

        // Create bullet at muzzle point
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        
        // Only reduce bullet count if not in infinite mode
        if (!infiniteBullets)
        {
            currentBulletCount--;
            UpdateBulletCountText();
        }
        
        // Start shooting cooldown
        StartCoroutine(ShootCooldown());

        // Add sound effect when shooting
        AudioSource.PlayClipAtPoint(shootingAudioSource, muzzlePoint.position);

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
    
    /// <summary>
    /// Updates the bullet count text display
    /// </summary> 
    private void UpdateBulletCountText()
    {
        if (bulletCountText != null)
        {
            if (infiniteBullets)
            {
                bulletCountText.text = "Bullets: ∞";
            }
            else
            {
                // Ensure bullet count doesn't go below 0
                currentBulletCount = Mathf.Max(0, currentBulletCount);
                bulletCountText.text = $"Bullets: {currentBulletCount}/{maxBullets}";
            }
        }
    }

    /// <summary>
    /// Reloads the gun to full capacity
    /// </summary>
    private void Reload()
    {
        // Prevent reloading if already reloading or has full ammo
        if (isReloading || currentBulletCount == maxBullets)
            return;

        StartCoroutine(ReloadCoroutine());
    }

    /// <summary>
    /// Find and store references to the left and right hand components
    /// </summary>
    #region Hide/Show Controllers

    /// <summary>
    /// Hides the controllers when the gun is grabbed
    /// </summary>
    private void HideControllers()
    {
        if (leftHand != null)
            leftHand.gameObject.SetActive(false);
        
        if (rightHand != null)
            rightHand.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows the controllers when the gun is released
    /// </summary>
    private void ShowControllers()
    {
        if (leftHand != null)
            leftHand.gameObject.SetActive(true);
        
        if (rightHand != null)
            rightHand.gameObject.SetActive(true);
    }

    /// <summary>
    /// Find and store references to the left and right hand components
    /// </summary>
    private void FindHandReferences()
    {
    // Find OVRHand components in the scene
    OVRHand[] hands = FindObjectsOfType<OVRHand>();
    
    foreach (OVRHand hand in hands)
    {
        // Check by name or transform hierarchy
        if (hand.gameObject.name.ToLower().Contains("left"))
        {
            leftHand = hand;
        }
        else if (hand.gameObject.name.ToLower().Contains("right"))
        {
            rightHand = hand;
        }
    }

    // Alternative method if name-based detection fails
    if (leftHand == null || rightHand == null)
    {
        // Assuming hands are direct children of a specific parent
        Transform handParent = transform.root.Find("OVRCameraRig/TrackingSpace");
        if (handParent != null)
        {
            leftHand = handParent.Find("LeftHandAnchor")?.GetComponent<OVRHand>();
            rightHand = handParent.Find("RightHandAnchor")?.GetComponent<OVRHand>();
        }
    }

    // Log warning if hands are still not found
    if (leftHand == null || rightHand == null)
    {
        Debug.LogWarning("Could not find both left and right hands in the scene.");
    }
    }
    
    #endregion

    #region Reloading
    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        canShoot = false;

        // Play reload sound
        // AudioSource.PlayClipAtPoint(reloadAudioClip, transform.position);

        // Update UI to show reloading
        if (cooldownText != null)
        {
            cooldownText.text = "Reloading...";
        }

        // Reload animation or delay
        yield return new WaitForSeconds(reloadTime);

        // Reset bullet count
        currentBulletCount = maxBullets;
        UpdateBulletCountText();

        // Reset shooting ability
        canShoot = true;
        isReloading = false;

        // Clear reload text
        if (cooldownText != null)
        {
            cooldownText.text = "Reloaded!";
            
            // Optional: Clear text after a short delay
            StartCoroutine(ClearReloadText());
        }
    }

    private IEnumerator ClearReloadText()
    {
        yield return new WaitForSeconds(1f);
        if (cooldownText != null)
        {
            cooldownText.text = "";
        }
    }
    #endregion
}