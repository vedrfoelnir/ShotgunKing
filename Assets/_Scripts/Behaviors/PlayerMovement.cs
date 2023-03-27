using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    public GameObject arrowPrefab; // Reference to the arrow prefab

    private GameObject arrow; // Reference to the arrow game object
    private bool isWaitingForInput = true; // Flag to indicate whether the player is currently waiting for input

    // Update is called once per frame
    void Update()
    {
        if (isWaitingForInput)
        {
            // Get the mouse position in screen space
            Vector3 mousePosition = Input.mousePosition;

            // Convert the mouse position to world space
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Get the direction from the game object to the mouse position
            Vector3 direction = mouseWorldPosition - transform.position;

            // Rotate the game object to face the mouse cursor
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

            // If the arrow game object doesn't exist, create it
            if (arrow == null)
            { 
                arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            }

            // Set the arrow's position and rotation
            arrow.transform.position = transform.position + direction.normalized * 2f; // Offset arrow position from player
            arrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

            // Show/hide the arrow based on whether the mouse cursor is pointing at an enemy
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            arrow.SetActive(hit.collider != null);

            // Check for input to trigger an action
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(arrow.ToString());
                isWaitingForInput = false;
                StartCoroutine(WaitForMoveAction());
            }
            else if (Input.GetMouseButtonDown(1))
            {
                // Shoot action
                isWaitingForInput = false;
                StartCoroutine(WaitForShootAction());
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // Special action
                isWaitingForInput = false;
                StartCoroutine(WaitForSpecialAction());
            }
        }
    }

    IEnumerator WaitForMoveAction()
    {
        // Wait for the player to click on a valid move location
        while (true)
        {
            // Check for input to trigger the move action
            if (Input.GetMouseButtonDown(0))
            {
                // TODO: Implement the move action
                isWaitingForInput = true;
                yield break;
            }

            yield return null;
        }
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
