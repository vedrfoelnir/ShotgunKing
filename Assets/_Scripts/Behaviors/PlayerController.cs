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

    [SerializeField]
    private GameObject crosshairPrefab;
    private GameObject crosshair;

    float hitRank;
    float hitFile;

    void Start()
    {
        HP = 3;
        scalingFactor = GameSetupManager.Instance.GetScaling();
    }

    public IEnumerator SetActiveAndWaitForInputToMove()
    {
        Debug.Log("Waiting for Movement");
        isWaitingForInput = true;
        yield return StartCoroutine(WaitForMoveAction(new KeyCode[] {
            KeyCode.W, KeyCode.A, KeyCode.X, KeyCode.D , KeyCode.Q, KeyCode.E, KeyCode.Y, KeyCode.C // Mouse: KeyCode.Alpha0, KeyCode.Alpha1,
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

    public IEnumerator WaitForInputToShoot()
    {
        Debug.Log("Waiting for Target");
        isWaitingForInput = true;
        yield return StartCoroutine(WaitForShootAction());

        Debug.Log("Hit on: " + hitRank + ", " + hitFile);
        yield return new WaitForEndOfFrame();
        
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
            case (KeyCode.X):
                direction = Vector3.back;
                break;
            case (KeyCode.A):
                direction = Vector3.left;
                break;
            case (KeyCode.D):
                direction = Vector3.right;
                break;
            case (KeyCode.Q):
                direction = Vector3.forward + Vector3.left;
                break;
            case (KeyCode.E):
                direction = Vector3.forward + Vector3.right;
                break;
            case (KeyCode.Y):
                direction = Vector3.back + Vector3.left;
                break;
            case (KeyCode.C):
                direction = Vector3.back + Vector3.right;
                break;
        }
        Debug.Log("Key Pressed: " + keyCode.ToString());
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
        GetComponent<Renderer>().material.color = Color.cyan;
        while (isWaitingForInput)
        {
            // Wait for player input to select a target
            if (Input.GetKey(KeyCode.S))
            {
                Debug.Log("isWaitingForInput");
                // Cast a ray from the mouse position to the game world
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.Log("Ray: " + ray.ToString());
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    hitRank = hit.transform.position.z / scalingFactor;
                    hitFile = hit.transform.position.x / scalingFactor;

                    // (hitRank, hitFile) = GameUnitManager.Instance.GetUnitPosition(hit.transform.gameObject);
                    // crosshair = Instantiate(crosshairPrefab, new Vector3(hitFile * scalingFactor, 0.5f, hitRank * scalingFactor), Quaternion.Euler(-90, 0, 0));
                    crosshair = Instantiate(crosshairPrefab, new Vector3(hitFile * scalingFactor, 0.5f, hitRank * scalingFactor), Quaternion.Euler(-90, 0, 0));
                    yield return new WaitForEndOfFrame();

                    // Exit the coroutine
                    yield break;
                    
                }
            }

            yield return new WaitForEndOfFrame();
        }
        GetComponent<Renderer>().material.color = Color.black;
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