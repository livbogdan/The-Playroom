using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a physical button that can be pressed and triggers events
/// </summary>
public class PhysicalButton : MonoBehaviour
{
    [Header("Button Configuration")]
    [Tooltip("How deep the button will be pressed down")]
    [SerializeField] private float pressDepth = 0.1f;

    [Tooltip("Force applied when the button is pressed")]
    [SerializeField] private float pressForce = 10f;

    [Header("Button Events")]
    [Tooltip("Event triggered when the button is pressed")]
    [SerializeField] private UnityEvent onButtonPressed;

    [Tooltip("Event triggered when the button resets to its original position")]
    [SerializeField] private UnityEvent onButtonReset;

    // Internal tracking variables
    private Vector3 originalPosition;
    private Rigidbody buttonRigidbody;
    private bool isPressed = false;

    /// <summary>
    /// Initialize button components and store original position
    /// </summary>
    private void Awake()
    {
        // Cache Rigidbody component for performance
        buttonRigidbody = GetComponent<Rigidbody>();
        
        // Store the initial local position of the button
        originalPosition = transform.localPosition;
    }

    /// <summary>
    /// Detect collision and press the button if not already pressed
    /// </summary>
    /// <param name="collision">Collision information</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Prevent multiple presses
        if (!isPressed)
        {
            PressButton();
        }
    }

    /// <summary>
    /// Performs the button press action
    /// </summary>
    private void PressButton()
    {
        // Translate button downwards by press depth
        transform.localPosition = originalPosition - new Vector3(0, pressDepth, 0);
        
        // Apply downward impulse force
        buttonRigidbody.AddForce(Vector3.down * pressForce, ForceMode.Impulse);

        // Update button state
        isPressed = true;

        // Trigger press event
        onButtonPressed?.Invoke();

        // Schedule automatic reset
        Invoke(nameof(ResetButton), 1f);
    }

    /// <summary>
    /// Resets the button to its original position
    /// </summary>
    private void ResetButton()
    {
        // Restore original position
        transform.localPosition = originalPosition;
        
        // Reset pressed state
        isPressed = false;

        // Trigger reset event
        onButtonReset?.Invoke();
    }
}