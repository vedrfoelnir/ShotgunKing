using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBehaviour : MonoBehaviour
{

    public float scalingFactor;

    private void Start()
    {
        scalingFactor = GameSetupManager.Instance.GetScaling();
    }

    public IEnumerator MoveAction()
    {
        Debug.Log("Waiting for Turn");
        yield return new WaitUntil(() => (GameStateManager.Instance.State == GameState.TurnOpponent));
        Debug.Log("Executing Turn");
        yield return StartCoroutine(TurnExecution());
    }

    private IEnumerator TurnExecution()
    {
        int currentRank = Mathf.RoundToInt(transform.position.z / scalingFactor + 1);
        int currentFile = Mathf.RoundToInt(transform.position.x / scalingFactor + 1);
        int nextRank;
        int nextFile;
        (nextRank, nextFile) = ChooseFromPossibleMoves(GetPossibleMoves());

        Debug.Log("Moving " + ToString() + " to (" + nextRank + ", " + nextFile + ")" );
        GameUnitManager.Instance.UpdateUnit(currentRank, currentFile, nextRank, nextFile);
        transform.position = new Vector3(nextRank * scalingFactor, 0, nextFile * scalingFactor);
        yield return new WaitForSeconds(1);
    }

    public abstract List<(int, int)> GetPossibleMoves();
    public abstract (int, int) ChooseFromPossibleMoves(List<(int, int)> possibleMoves);

    /**
     * default MoveAction:
     *   - Implement Evaluate Possible Moves
     *   - Implement Move ( Change Transform, returns new rank/file )
     *  UpdateUnit in Manager
     * 
     */
}