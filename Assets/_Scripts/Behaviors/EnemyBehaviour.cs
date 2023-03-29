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
        // Moving
        int currentRank;
        int currentFile;
        int nextRank;
        int nextFile;

        (currentRank, currentFile) = GameUnitManager.Instance.GetUnitPosition(transform.gameObject);
        (nextRank, nextFile) = ChooseFromPossibleMoves(GetPossibleMoves());
        
        Debug.Log("Moving " + ToString() + " to (" + nextRank + ", " + nextFile + ")");
        GetComponent<Renderer>().material.color = Color.red;
       
        GameObject objectOnTarget = GameUnitManager.Instance.IsOccupied(nextRank, nextFile);
        yield return new WaitUntil(() => (objectOnTarget == null || objectOnTarget != null));
        if (objectOnTarget == null)
        {
            GetComponent<Renderer>().material.color = originalColor;
            GameUnitManager.Instance.UpdateUnit(currentRank, currentFile, nextRank, nextFile);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        if (objectOnTarget != null && objectOnTarget.CompareTag("Player"))
        {
            Debug.Log(gameObject.ToString() + " strikes the player!");
            PlayerController.Instance.HP--;
            PlayerController.Instance.gameObject.GetComponent<Renderer>().material.color = Color.red;
            
            yield return new WaitForSecondsRealtime(0.1f);

            Debug.Log("Enemy Turn Executed");
            GameUnitManager.Instance.RemoveUnit(gameObject, currentRank, currentFile);
            Destroy(gameObject);
            
        }
    }

    public abstract List<(int, int)> GetPossibleMoves();
    public abstract (int, int) ChooseFromPossibleMoves(List<(int, int)> possibleMoves);
}