using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RookBehaviour : EnemyBehaviour
{

    public override (int, int) ChooseFromPossibleMoves(List<(int, int)> possibleMoves)
    {
        (int, int) chosenMove = (-1, -1);

        // Get the position of the player
        (int playerRank, int playerFile) = GameUnitManager.Instance.GetUnitPosition(GameUnitManager.Instance.player);

        foreach ((int, int) move in possibleMoves)
        {
            Debug.Log("Rook: Evaluated Move: " + move.Item1 + ", " + move.Item2);
            // If the move is on the same rank or file as the player, choose it
            if (move.Item1 == playerRank || move.Item2 == playerFile)
            {
                chosenMove = move;
                break;
            }
        }

        Debug.Log("Rook: Chosen Move: " + chosenMove);

        return chosenMove;
    }


    public override List<(int, int)> GetPossibleMoves()
    {
        List<(int, int)> possibleMoves = new List<(int, int)>();

        int currentRank;
        int currentFile;
        (currentRank, currentFile) = GameUnitManager.Instance.GetUnitPosition(transform.gameObject);

        // Add legal chess moves
        for (int i = 1; i <= 8; i++)
        {
            possibleMoves.Add((i, currentFile));
            possibleMoves.Add((currentRank, i));
        }

        Debug.Log("Possible Moves on Rook: " + string.Join(", ", possibleMoves));
        return possibleMoves;
    }

}