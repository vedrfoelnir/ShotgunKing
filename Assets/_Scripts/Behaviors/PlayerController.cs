using UnityEngine;
using System.Collections;

public class PlayerController : Singleton<PlayerController>
{
    // export
    [HideInInspector]
    public int HP { get; set; }

    private bool isWaitingForInput = false;
    private Vector3 direction = Vector3.zero;
    private float scalingFactor;

    void Start()
    {
        scalingFactor = GameSetupManager.Instance.GetScaling();
        HP = 1;
    }


    public IEnumerator SetActiveAndWaitForInputToMove()
    {
        Debug.Log("Waiting for Movement");
        isWaitingForInput = true;
        yield return StartCoroutine(WaitForMoveAction(new KeyCode[] {
            KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D // Mouse: KeyCode.Alpha0, KeyCode.Alpha1,
        })); 

        if (direction != Vector3.zero)
        {
            Debug.Log("Movement Chosen: " + direction.ToString());
            MovePlayer(direction);
        }
        else
        {
            Debug.Log("Movement Error");
        }
    }

    private void MovePlayer(Vector3 direction)
    {
        int currentRank;
        int currentFile;
        (currentRank, currentFile) = GameUnitManager.Instance.GetUnitPosition(transform.gameObject);
        int newRank = Mathf.RoundToInt(currentRank + direction.z);
        int newFile = Mathf.RoundToInt(currentFile + direction.x);

        GameObject unitAtNewPosition = GameUnitManager.Instance.IsOccupied(newRank, newFile);
        if (unitAtNewPosition != null)
        {
            Debug.Log("New position is occupied! by: " + unitAtNewPosition.ToString());
            return;
        }
        GameUnitManager.Instance.UpdateUnit(currentRank, currentFile, newRank, newFile);  
    }


    private void SetChoiceTo(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case (KeyCode.W):
                direction = Vector3.forward;
                break;
            case (KeyCode.S):
                direction = Vector3.back;
                break;
            case (KeyCode.A):
                direction = Vector3.left;
                break;
            case (KeyCode.D):
                direction = Vector3.right;
                break;
        }
    }

    IEnumerator WaitForMoveAction(KeyCode[] codes)
    {
        GetComponent<Renderer>().material.color = Color.green;
        while (isWaitingForInput)
        {
            foreach (KeyCode k in codes)
            {
                if (Input.GetKey(k))
                {
                    isWaitingForInput = false;
                    SetChoiceTo(k);                    
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        GetComponent<Renderer>().material.color = Color.black;

    }

    IEnumerator WaitForShootAction()
    {
        // Wait for the player to click on a valid target to shoot
        while (true)
        {
            // Check for input to trigger the shoot action
            if (Input.GetMouseButtonDown(0))
            {
                // TODO: Implement the shoot action
                isWaitingForInput = true;
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator WaitForSpecialAction()
    {
        // Wait for the player to press a key to trigger the special action
        while (true)
        {
            // Check for input to trigger the special action
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // TODO: Implement the special action
                isWaitingForInput = true;
                yield break;
            }

            yield return null;
        }
    }
}

/**
    // Get the mouse position in screen space
    Vector3 mousePosition = Input.mousePosition;
    // Convert the mouse position to world space
    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
    // Get the direction from the game object to the mouse position
    Vector3 direction = mouseWorldPosition - transform.position;
    // Rotate the game object to face the mouse cursor
    transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
 */