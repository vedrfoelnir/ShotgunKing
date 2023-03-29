using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBehaviour : MonoBehaviour
{

    private Color originalColor;

    private void Start()
    {
        originalColor = GetComponent<Renderer>().material.color;
    }

    public IEnumerator MoveAction()
    {
        Debug.Log("Waiting for Turn");
        GetComponent<Renderer>().material.color = Color.yellow; // Turn Yellow if ready to move
        yield return new WaitUntil(() => (GameStateManager.Instance.State == GameState.TurnOpponent));
        Debug.Log("Executing Turn");
        
        yield return StartCoroutine(TurnExecution());
    }

    private IEnumerator TurnExecution()
    {
        int currentRank;
        int currentFile;
        int nextRank;
        int nextFile;

        (currentRank, currentFile) = GameUnitManager.Instance.GetUnitPosition(transform.gameObject);
        (nextRank, nextFile) = ChooseFromPossibleMoves(GetPossibleMoves());

        // Moving
        Debug.Log("Moving " + ToString() + " to (" + nextRank + ", " + nextFile + ")");
        GetComponent<Renderer>().material.color = Color.red;
        GameUnitManager.Instance.UpdateUnit(currentRank, currentFile, nextRank, nextFile);

        yield return new WaitForSecondsRealtime(0.1f);
        GetComponent<Renderer>().material.color = originalColor;
    }

    public abstract List<(int, int)> GetPossibleMoves();
    public abstract (int, int) ChooseFromPossibleMoves(List<(int, int)> possibleMoves);
}