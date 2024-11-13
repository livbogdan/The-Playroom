using UnityEngine;

public class SimpleScript : MonoBehaviour
{

    [SerializeField]
    private Material[] material;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("box"))
        {
            Debug.Log("I'm in the box");
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = material[Random.Range(0, material.Length)];
            }
            Destroy(other.gameObject);
            
        }
    }
}
