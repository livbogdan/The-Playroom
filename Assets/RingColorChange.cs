using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class RingColorChange : MonoBehaviour
{
    // Reference to the ring's material so we can change its color
    private Renderer ringRenderer;

    // Color to change to when the ball passes through
    public Color newColor = Color.blue;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Renderer component of the ring
        ringRenderer = GetComponent<Renderer>();
    }

    // This method is called when another collider enters the trigger collider attached to the object
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the ball
        if (other.CompareTag("Sandball"))
        {
            // Change the color of the ring
            ChangeColor();
        }
    }

    // Method to change the color of the ring
    private void ChangeColor()
    {
        if (ringRenderer != null)
        {
            ringRenderer.material.color = newColor;
        }
    }
}
