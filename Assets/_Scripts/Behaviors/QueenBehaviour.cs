using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QueenBehaviour : EnemyBehaviour
{
    public override (int, int) ChooseFromPossibleMoves(List<(int, int)> possibleMoves)
    {
        (int, int) chosenMove = (-1, -1);

        // Get the position of the player
        (int playerRank, int playerFile) = GameUnitManager.Instance.GetUnitPosition(GameUnitManager.Instance.player);

        foreach ((int, int) move in possibleMoves)
        {
            Debug.Log("Queen: Evaluated Move: " + move.Item1 + ", " + move.Item2);
            // If the move is on the same rank, file, or diagonal as the player, choose it
            if (move.Item1 == playerRank || move.Item2 == playerFile || Mathf.Abs(move.Item1 - playerRank) == Mathf.Abs(move.Item2 - playerFile))
            {
                // If there's nothing in the way, move there
                if (!GameUnitManager.Instance.IsOccupied(move.Item1, move.Item2))
                {
                    chosenMove = move;
                }
                // If there's a player in the way, capture the player
                else if (GameUnitManager.Instance.IsOccupied(move.Item1, move.Item2).CompareTag("Player"))
                {
                    chosenMove = move;
                    break;
                }
            }
        }

        Debug.Log("Queen: Chosen Move: " + chosenMove);

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
            // Add moves along the rank and file
            possibleMoves.Add((i, currentFile));
            possibleMoves.Add((currentRank, i));

            // Add moves along the diagonals
            possibleMoves.Add((currentRank + i, currentFile + i));
            possibleMoves.Add((currentRank - i, currentFile + i));
            possibleMoves.Add((currentRank + i, currentFile - i));
            possibleMoves.Add((currentRank - i, currentFile - i));
        }

        Debug.Log("Possible Moves on Queen: " + string.Join(", ", possibleMoves));
        return possibleMoves;
    }
}
