using UnityEngine;
using System.Collections;

public class PlayerMovement : Singleton<PlayerMovement>
{
    private Color originalColor;
    private bool isWaitingForInput = false;

    void Start() => originalColor = GetComponent<Renderer>().material.color;


    public void TurnGreenAndWaitForInputToMove()
    {
        Debug.Log("Waiting for Movement");
        // Change the color of the GameObject to green
        GetComponent<Renderer>().material.color = Color.green;
        isWaitingForInput = true;
    }

    void Update()
    {
        
        if (isWaitingForInput)
        {
            WaitForMoveAction();

        }
    }

    IEnumerator WaitForMoveAction()
    {
        while (true)
        {
            // Check for input to trigger the move action
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 direction = Vector3.zero;

                // Set the direction based on the WASD keys pressed
                if (Input.GetKeyDown(KeyCode.W))
                {
                    direction = Vector3.forward;
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    direction = Vector3.back;
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    direction = Vector3.left;
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    direction = Vector3.right;
                }

                // Move the player in the indicated direction
                if (direction != Vector3.zero)
                {
                    transform.position += direction * 3.0f;
                }

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