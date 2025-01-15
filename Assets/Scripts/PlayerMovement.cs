using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    float horizontalMovement; // stores the input value for the player's movement aloing the x-axis

    private void Update()
    {
        //apply velocity to the payer movement
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        //movement on the x axis
        horizontalMovement = context.ReadValue<Vector2>().x;
    }
}
