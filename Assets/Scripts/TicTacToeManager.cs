using UnityEngine;
using Oculus.Interaction;
using System;

public class TicTacToeManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The prefab for the Tic-Tac-Toe grid.")]
    [SerializeField] private Transform[] gridPositions;
    [Tooltip("The prefab for the Tic-Tac-Toe pieces.")]
    [SerializeField] private GameObject[] playerPieces;
    
    private int[,] board = new int[3, 3];
    private int currentPlayer = 1;
    private bool gameOver = false;

    public void CheckGridPlacement(Grabbable grabbedObject)
    {
        if (gameOver) return;

        // Find the closest grid position
        Transform closestGrid = FindClosestGridPosition(grabbedObject.transform.position);
        
        if (closestGrid != null)
        {
            int gridIndex = Array.IndexOf(gridPositions, closestGrid);
            int row = gridIndex / 3;
            int col = gridIndex % 3;

            // Check if grid position is empty
            if (board[row, col] == 0)
            {
                // Place piece
                board[row, col] = currentPlayer;
                grabbedObject.transform.position = closestGrid.position;
                
                // Check win condition
                if (CheckWinCondition(row, col))
                {
                    Debug.Log($"Player {currentPlayer} wins!");
                    gameOver = true;
                }
                else if (CheckDrawCondition())
                {
                    Debug.Log("Draw!");
                    gameOver = true;
                }

                // Switch player
                currentPlayer = (currentPlayer == 1) ? 2 : 1;
            }
        }
    }

    private Transform FindClosestGridPosition(Vector3 position)
    {
        Transform closest = null;
        float minDistance = float.MaxValue;

        foreach (Transform gridPos in gridPositions)
        {
            float distance = Vector3.Distance(position, gridPos.position);
            if (distance < minDistance && distance < 0.5f)  // Threshold for placement
            {
                minDistance = distance;
                closest = gridPos;
            }
        }

        return closest;
    }

    private bool CheckWinCondition(int row, int col)
    {
        // Check row
        if (board[row, 0] == currentPlayer && 
            board[row, 1] == currentPlayer && 
            board[row, 2] == currentPlayer)
            return true;

        // Check column
        if (board[0, col] == currentPlayer && 
            board[1, col] == currentPlayer && 
            board[2, col] == currentPlayer)
            return true;

        // Check diagonals
        if ((board[0, 0] == currentPlayer && 
             board[1, 1] == currentPlayer && 
             board[2, 2] == currentPlayer) ||
            (board[0, 2] == currentPlayer && 
             board[1, 1] == currentPlayer && 
             board[2, 0] == currentPlayer))
            return true;

        return false;
    }

    private bool CheckDrawCondition()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == 0)
                    return false;
            }
        }
        return true;
    }

    public void ResetGame()
    {
        board = new int[3, 3];
        currentPlayer = 1;
        gameOver = false;
    }
}
