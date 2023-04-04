using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBehaviour : MonoBehaviour
{

    private Color originalColor;
    public int HP;

    private void Start()
    {
        originalColor = GetComponent<Renderer>().material.color;
    }

    public IEnumerator MoveAction()
    {
        Debug.Log("Waiting for Turn");
        GetComponent<Renderer>().material.color = Color.yellow; // Turn Yellow if ready to move
        yield return new WaitUntil(() => (GameStateManager.Instance.State == GameState.TurnOpponent));

        if (HP < 1)
        {
            Explode();
            
        } else
        {
            Debug.Log("Executing Turn");
            yield return StartCoroutine(TurnExecution());
        }  
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
        if (nextRank < 1 || nextFile <1)
        {
            Debug.Log("Shit's fucked");
        } else
        {
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

                yield return new WaitForEndOfFrame();

                Debug.Log("Enemy Turn Executed");
                GameUnitManager.Instance.RemoveUnit(gameObject, currentRank, currentFile);
                Explode();
            }
        }
        
    }

    private void Explode()
    {
        int currentRank;
        int currentFile;
        (currentRank, currentFile) = GameUnitManager.Instance.GetUnitPosition(transform.gameObject);
        ParticleSystem exp = GetComponent<ParticleSystem>();
        if (exp)
        {
            exp.Play();
        }  
        GameUnitManager.Instance.RemoveUnit(gameObject, currentRank, currentFile);
        // ParticleS
    }

    public void damage(int amount)
    {
        Debug.Log(this.ToString() + " takes " + amount + " damage.");
        HP -= amount;
    }

    public abstract List<(int, int)> GetPossibleMoves();
    public abstract (int, int) ChooseFromPossibleMoves(List<(int, int)> possibleMoves);
}