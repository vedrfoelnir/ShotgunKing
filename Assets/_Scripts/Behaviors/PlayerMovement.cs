using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Color originalColor;
    private bool isWaitingForInput = false;

    // Start is called before the first frame update
    void Start() => originalColor = GetComponent<Renderer>().material.color;

    public void TurnGreenAndWaitForInputToMove()
    {
        // Change the color of the GameObject to green
        GetComponent<Renderer>().material.color = Color.green;

        // Set the flag to indicate that we are waiting for input to move
        isWaitingForInput = true;
    }

    // Update is called once per frame

    void Update()
    {
        
        if (isWaitingForInput)
        {
            // Check for input to trigger the move action
            if (Input.GetKeyDown(KeyCode.W))
            {
                // Move up
                transform.position += Vector3.up * 3.5f;
                // Reset the color of the GameObject
                GetComponent<Renderer>().material.color = originalColor;
                // Reset the flag
                isWaitingForInput = false;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                // Move left
                transform.position += Vector3.left * 3.5f;
                // Reset the color of the GameObject
                GetComponent<Renderer>().material.color = originalColor;
                // Reset the flag
                isWaitingForInput = false;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                // Move down
                transform.position += Vector3.down * 3.5f;
                // Reset the color of the GameObject
                GetComponent<Renderer>().material.color = originalColor;
                // Reset the flag
                isWaitingForInput = false;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // Move right
                transform.position += Vector3.right * 3.5f;
                // Reset the color of the GameObject
                GetComponent<Renderer>().material.color = originalColor;
                // Reset the flag
                isWaitingForInput = false;
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
                    transform.position += direction * 3.5f;
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