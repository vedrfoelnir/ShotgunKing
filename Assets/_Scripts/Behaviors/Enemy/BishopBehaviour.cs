using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BishopBehaviour : EnemyBehaviour
{

    private void Start()
    {
        this.HP = 2;    
    }

    public override (int, int) ChooseFromPossibleMoves(List<(int, int)> possibleMoves)
    {
        (int, int) chosenMove = (-1, -1);

        // Get the position of the player
        (int playerRank, int playerFile) = GameUnitManager.Instance.GetUnitPosition(GameUnitManager.Instance.player);

        foreach ((int, int) move in possibleMoves)
        {
            Debug.Log("Bishop: Evaluated Move: " + move.Item1 + ", " + move.Item2);
            // If the move is on the same diagonal as the player, choose it
            if (Mathf.Abs(move.Item1 - playerRank) == Mathf.Abs(move.Item2 - playerFile))
            {
                chosenMove = move;
                break;
            }
        }

        Debug.Log("Bishop: Chosen Move: " + chosenMove);

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
            int fileDiff = Mathf.Abs(i - currentFile);
            int rankDiff = Mathf.Abs(i - currentRank);
            if (fileDiff == rankDiff)
            {
                possibleMoves.Add((i, i));
                possibleMoves.Add((i, 9 - i));
            }
        }

        // Remove moves not on the board
        possibleMoves.RemoveAll(move => move.Item1 < 1 || move.Item1 > 8 || move.Item2 < 1 || move.Item2 > 8);

        Debug.Log("Possible Moves on Bishop: " + string.Join(", ", possibleMoves));
        return possibleMoves;
    }
}
