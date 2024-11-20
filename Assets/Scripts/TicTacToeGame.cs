using System;
using UnityEngine;

public class TicTacToeGame : MonoBehaviour
{
    public GameObject[] gridCubes; // Assign the 9 cubes in the inspector
    private int[] gridState = new int[9]; // 0 = empty, 1 = X, 2 = O
    private bool isPlayerXTurn = true;

    private void Start()
    {
        ResetGame();
    }

    private void ResetGame()
    {
        for (int i = 0; i < gridState.Length; i++)
        {
            gridState[i] = 0;
            gridCubes[i].GetComponent<Renderer>().material.color = Color.white;
        }
        isPlayerXTurn = true;
    }

    public bool OnCubeClicked(int index, string playerTag)
    {
        // Check if it's the correct player's turn
        if ((isPlayerXTurn && playerTag != "Player1") || (!isPlayerXTurn && playerTag != "Player2"))
            return false;

        if (gridState[index] != 0)
            return false;

        gridState[index] = isPlayerXTurn ? 1 : 2;
        gridCubes[index].GetComponent<Renderer>().material.color = isPlayerXTurn ? Color.red : Color.blue;

        if (CheckWinCondition())
        {
            Debug.Log((isPlayerXTurn ? "Player X" : "Player O") + " wins!");
            ResetGame();
        }
        else if (IsDraw())
        {
            Debug.Log("It's a draw!");
            ResetGame();
        }
        else
        {
            isPlayerXTurn = !isPlayerXTurn;
        }

        return true;
    }

    private bool CheckWinCondition()
    {
        int[,] winConditions = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Rows
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
            {0, 4, 8}, {2, 4, 6}             // Diagonals
        };

        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            int a = winConditions[i, 0];
            int b = winConditions[i, 1];
            int c = winConditions[i, 2];

            if (gridState[a] != 0 && gridState[a] == gridState[b] && gridState[a] == gridState[c])
                return true;
        }

        return false;
    }

    private bool IsDraw()
    {
        foreach (int state in gridState)
        {
            if (state == 0)
                return false;
        }
        return true;
    }

    public int GetCubeState(int index)
    {
        if (index < 0 || index >= gridState.Length)
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

        return gridState[index];
    }
}