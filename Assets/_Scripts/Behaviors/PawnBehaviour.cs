﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PawnBehaviour : EnemyBehaviour
{
    private int forwardDirection;

    void Start()
    {
        forwardDirection = -1; //GameSetupManager.Instance.GetForwardDirection();
    }

    public override (int, int) ChooseFromPossibleMoves(List<(int, int)> possibleMoves)
    {
        (int, int) chosenMove = (-1, -1);
        bool canStrikePlayer = false;

        foreach ((int, int) move in possibleMoves)
        {
            // If there's nothing in the way, move there
            if (!GameUnitManager.Instance.IsOccupied(move.Item1, move.Item2))
            {
                chosenMove = move;
            }
            // If there's a player diagonally in front, strike the player
            else if (GameUnitManager.Instance.IsOccupied(move.Item1, move.Item2).CompareTag("Player"))
            {
                int rankOffset = move.Item1 - Mathf.RoundToInt(transform.position.z / scalingFactor + 1);
                int fileOffset = move.Item2 - Mathf.RoundToInt(transform.position.x / scalingFactor + 1);
                if (rankOffset == forwardDirection && Mathf.Abs(fileOffset) == 1)
                {
                    canStrikePlayer = true;
                    chosenMove = move;
                }
            }
        }

        // Strike the player if possible
        if (canStrikePlayer)
        {
            Debug.Log("Pawn strikes the player!");
            PlayerController.Instance.HP--;
        }

        return chosenMove;
    }

    public override List<(int, int)> GetPossibleMoves()
    {
        List<(int, int)> possibleMoves = new List<(int, int)>();
        int currentRank = Mathf.RoundToInt(transform.position.z / scalingFactor + 1);
        int currentFile = Mathf.RoundToInt(transform.position.x / scalingFactor + 1);

        // Add the two possible moves
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

        return possibleMoves;
    }

}
