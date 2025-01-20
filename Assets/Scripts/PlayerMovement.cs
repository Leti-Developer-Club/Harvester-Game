using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    float horizontalMovement; // stores the input value for the player's movement aloing the x-axis

    public Transform leftBoundary;
    public Transform rightBoundary;

    

    private void Update()
    {
        //apply velocity to the player movement
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);

        //Get the boundary position dynamically
        float leftBoundaryPosition = leftBoundary.position.x;
        float rightBoundaryPosition = rightBoundary.position.x;

        //clamp the basket position to prevent it from going outside the screen
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, leftBoundaryPosition, rightBoundaryPosition);
        transform.position = clampedPosition;

    }

    public void Move(InputAction.CallbackContext context)
    {
        //movement on the x axis
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

}
