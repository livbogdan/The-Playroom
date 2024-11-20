using UnityEngine;

public class CubeInteraction : MonoBehaviour
{
    public int index;
    private TicTacToeGame game;

    private void Start()
    {
        game = FindObjectOfType<TicTacToeGame>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is allowed to make a move
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            if (game.OnCubeClicked(index, other.tag))
            {
                Debug.Log("Move made by " + other.tag);
            }
            else
            {
                Debug.Log("It's not " + other.tag + "'s turn or the cube is already occupied.");
            }
        }
    }
}
