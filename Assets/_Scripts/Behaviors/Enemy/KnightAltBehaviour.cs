using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KnightAltBehaviour : EnemyBehaviour
{
    public override (int, int) ChooseFromPossibleMoves(List<(int, int)> possibleMoves)
    {
        (int, int) chosenMove = (-1, -1);

        // Get the position of the player
        (int playerRank, int playerFile) = GameUnitManager.Instance.GetUnitPosition(GameUnitManager.Instance.player);

        foreach ((int, int) move in possibleMoves)
        {
            Debug.Log("Knight: Evaluated Move: " + move.Item1 + ", " + move.Item2);

            // If the move captures the player, choose it
            if (move.Item1 == playerRank && move.Item2 == playerFile)
            {
                chosenMove = move;
                break;
            }

            // Otherwise, choose a random move that doesn't capture the player
            if (chosenMove == (-1, -1) && !GameUnitManager.Instance.IsOccupied(move.Item1, move.Item2))
            {
                chosenMove = move;
            }
        }

        Debug.Log("Knight: Chosen Move: " + chosenMove);

        return chosenMove;
    }

    public override List<(int, int)> GetPossibleMoves()
    {
        List<(int, int)> possibleMoves = new List<(int, int)>();

        int currentRank;
        int currentFile;
        (currentRank, currentFile) = GameUnitManager.Instance.GetUnitPosition(transform.gameObject);

        // Add legal chess moves
        possibleMoves.Add((currentRank - 1, currentFile - 2));
        possibleMoves.Add((currentRank - 2, currentFile - 1));
        possibleMoves.Add((currentRank - 2, currentFile + 1));
        possibleMoves.Add((currentRank - 1, currentFile + 2));
        possibleMoves.Add((currentRank + 1, currentFile + 2));
        possibleMoves.Add((currentRank + 2, currentFile + 1));
        possibleMoves.Add((currentRank + 2, currentFile - 1));
        possibleMoves.Add((currentRank + 1, currentFile - 2));

        Debug.Log("Possible Moves on Knight: " + string.Join(", ", possibleMoves));
        return possibleMoves;
    }
}
