using UnityEngine;
using Oculus.Interaction;

public class TicTacToePiece : MonoBehaviour
{
    [SerializeField] private TicTacToeManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        // Assuming the grid positions have a specific tag or layer
        if (other.CompareTag("GridPosition"))
        {
            Grabbable grabbable = GetComponent<Grabbable>();
            if (grabbable != null)
            {
                gameManager.CheckGridPlacement(grabbable);
            }
        }
    }
}
