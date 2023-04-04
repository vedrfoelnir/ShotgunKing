using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PawnBehaviour : EnemyBehaviour
{
    private int forwardDirection;

    void Start()
    {
        forwardDirection = -1; //GameSetupManager.Instance.GetForwardDirection(); -> Can chose whether black or white
        this.HP = 1;
    }

    public override (int, int) ChooseFromPossibleMoves(List<(int, int)> possibleMoves)
    {
        (int, int) chosenMove = (-1, -1);

        foreach ((int, int) move in possibleMoves)
        {
            Debug.Log("Pawn Evaluated Move: " + move.Item1 + ", " + move.Item2);
            // If there's nothing in the way, move there
            if (!GameUnitManager.Instance.IsOccupied(move.Item1, move.Item2))
            {
                chosenMove = move;
            }
            // If there's a player diagonally in front, strike the player
            else if (GameUnitManager.Instance.IsOccupied(move.Item1, move.Item2).CompareTag("Player"))
            {
                chosenMove = move;      
            }
        }
        
        Debug.Log("Pawn Chosen Move: " + chosenMove);

        // TODO Promotion


        return chosenMove;
    }

    public override List<(int, int)> GetPossibleMoves()
    {
        List<(int, int)> possibleMoves = new List<(int, int)>();

        int currentRank;
        int currentFile;
        (currentRank, currentFile) = GameUnitManager.Instance.GetUnitPosition(transform.gameObject);

        // Add legal chess moves
        possibleMoves.Add((currentRank + forwardDirection, currentFile));
        

        // Check if the pawn can strike the player diagonally in front
        GameObject objectOnLeft = GameUnitManager.Instance.IsOccupied(currentRank + forwardDirection, currentFile - 1);
        if (objectOnLeft != null && objectOnLeft.CompareTag("Player"))
        {
            possibleMoves.Add((currentRank + forwardDirection, currentFile - 1));
        }
        GameObject objectOnRight = GameUnitManager.Instance.IsOccupied(currentRank + forwardDirection, currentFile + 1);
        if (objectOnRight != null && objectOnRight.CompareTag("Player"))
        {
            possibleMoves.Add((currentRank + forwardDirection, currentFile + 1));
        }

        Debug.Log("Possible Moves on Pawn: " + string.Join(", " , possibleMoves));
        return possibleMoves;
    }

}