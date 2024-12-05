using UnityEngine;

public class Respawn : MonoBehaviour
{
    [Header("Respawn Configuration")]
    [Tooltip("Objects to respawn")]
    [SerializeField] private GameObject[] objectsToRespawn;

    [Tooltip("Original spawn transforms for each object")]
    [SerializeField] private Transform[] originalTransforms;

    [Tooltip("Prefabs to spawn when objects go out of bounds")]
    [SerializeField] private GameObject[] respawnPrefabs;

    [Tooltip("Distance threshold to trigger respawn")]
    [SerializeField] private float respawnThreshold = 10f;

    private void Start()
    {
        // If original transforms not set, use current object transforms
        if (originalTransforms == null || originalTransforms.Length == 0)
        {
            originalTransforms = new Transform[objectsToRespawn.Length];
            for (int i = 0; i < objectsToRespawn.Length; i++)
            {
                if (objectsToRespawn[i] != null)
                {
                    originalTransforms[i] = objectsToRespawn[i].transform;
                }
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < objectsToRespawn.Length; i++)
        {
            if (objectsToRespawn[i] == null) continue;

            // Check if object has moved beyond respawn threshold
            if (Vector3.Distance(objectsToRespawn[i].transform.position, originalTransforms[i].position) > respawnThreshold)
            {
                RespawnObject(i);
            }
        }
    }

    public void RespawnObject(int index)
    {
        // Destroy current object
        Destroy(objectsToRespawn[index]);

        // Spawn new object if prefab is available
        if (respawnPrefabs[index] != null)
        {
            objectsToRespawn[index] = Instantiate(
                respawnPrefabs[index], 
                originalTransforms[index].position, 
                originalTransforms[index].rotation
            );
        }
    }
}