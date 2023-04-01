using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KnightBehaviour : EnemyBehaviour
{

    public override (int, int) ChooseFromPossibleMoves(List<(int, int)> possibleMoves)
    {
        (int, int) chosenMove = (-1, -1);

        // Get the position of the player
        (int playerRank, int playerFile) = GameUnitManager.Instance.GetUnitPosition(GameUnitManager.Instance.player);

        foreach ((int, int) move in possibleMoves)
        {
            Debug.Log("Knight: Evaluated Move: " + move.Item1 + ", " + move.Item2);
            GameObject occupiedtile = GameUnitManager.Instance.IsOccupied(move.Item1, move.Item2);
            // Check if the move is a valid knight move that can reach the player
            if (occupiedtile == null)
            {
                chosenMove = move;
            }
            else if(occupiedtile.CompareTag("Player"))
            {
                chosenMove = move;
                break;
            } else 
            {
                continue;                
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

        // Add all possible knight moves
        possibleMoves.Add((currentRank + 2, currentFile + 1));
        possibleMoves.Add((currentRank + 2, currentFile - 1));
        possibleMoves.Add((currentRank - 2, currentFile + 1));
        possibleMoves.Add((currentRank - 2, currentFile - 1));
        possibleMoves.Add((currentRank + 1, currentFile + 2));
        possibleMoves.Add((currentRank + 1, currentFile - 2));
        possibleMoves.Add((currentRank - 1, currentFile + 2));
        possibleMoves.Add((currentRank - 1, currentFile - 2));

        // Remove moves not on the board
        possibleMoves.RemoveAll(move => move.Item1 < 1 || move.Item1 > 8 || move.Item2 < 1 || move.Item2 > 8);

        Debug.Log("Possible Moves on Knight: " + string.Join(", ", possibleMoves));
        return possibleMoves;
    }
}

