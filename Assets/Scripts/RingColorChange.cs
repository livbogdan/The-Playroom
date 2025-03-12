using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class RingColorChange : MonoBehaviour
{
    // Reference to the ring's material so we can change its color
    private Renderer ringRenderer;
    [SerializeField]private Material newMaterial;
    private Material originalMaterial;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Renderer component of the ring
        ringRenderer = GetComponent<Renderer>();

        originalMaterial = ringRenderer.material;
    }

    // This method is called when another collider enters the trigger collider attached to the object
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the ball
        if (other.CompareTag("Sandball"))
        {
            // Change the color of the ring
            //ChangeColor();
            SwapMaterial();
        }
    }

    private void SwapMaterial()
    {
        if (ringRenderer != null && newMaterial != null)
        {
            ringRenderer.material = newMaterial;
        }
    }

    // Optional: Method to revert to original material
    private void RevertMaterial()
    {
        if (ringRenderer != null && originalMaterial != null)
        {
            ringRenderer.material = originalMaterial;
        }
    }
}
