using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBehaviour : MonoBehaviour
{

    public float scalingFactor;
    private Color originalColor;

    private void Start()
    {
        originalColor = GetComponent<Renderer>().material.color;
        scalingFactor = GameSetupManager.Instance.GetScaling();
        Debug.Log("Scaling received in general: " + scalingFactor);
    }

    public IEnumerator MoveAction()
    {
        Debug.Log("Waiting for Turn");
        GetComponent<Renderer>().material.color = Color.yellow; // Turn Yellow if ready to move
        yield return new WaitUntil(() => (GameStateManager.Instance.State == GameState.TurnOpponent));
        Debug.Log("Executing Turn");
        int currentRank = Mathf.RoundToInt(transform.position.z / scalingFactor + 1);
        int currentFile = Mathf.RoundToInt(transform.position.x / scalingFactor + 1);

        Debug.Log("Position of Pawn 3 on Action: " + currentRank + ", " + currentFile);
        
        yield return StartCoroutine(TurnExecution());
    }

    private IEnumerator TurnExecution()
    {
        int currentRank = GameUnitManager.Instance.GetUnitPosition();
        int currentFile = Mathf.RoundToInt(transform.position.x / scalingFactor + 1);
        Debug.Log("Position of Pawn on 4 General: " + currentRank + ", " + currentFile);
        int nextRank;
        int nextFile;
        (nextRank, nextFile) = ChooseFromPossibleMoves(GetPossibleMoves());
        GetComponent<Renderer>().material.color = Color.red; // Turn Red when move moving
        Debug.Log("Moving " + ToString() + " to (" + nextRank + ", " + nextFile + ")" );
        GameUnitManager.Instance.UpdateUnit(currentRank, currentFile, nextRank, nextFile);
        transform.position = new Vector3((nextFile-1) * scalingFactor, 0, (nextRank-1) * scalingFactor);
        yield return new WaitForSecondsRealtime(0.1f);
        GetComponent<Renderer>().material.color = originalColor;
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