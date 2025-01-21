using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    float horizontalMovement; // stores the input value for the player's movement aloing the x-axis

    public Transform leftBoundary;
    public Transform rightBoundary;

    Vector2 previousTouchPosition; //track the last touch position
    bool isTouching; //tracks if the player is actively draging
    

    private void OnEnable()
    {
        //Enable enhanced touch for touch input
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        //disables the wnhanced touch when the object is destroyed or the script is disabled
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        if (!isTouching)
        {
            //apply velocity to the player movement (Keyboard Input)
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        }
        

        //Get the boundary position dynamically
        float leftBoundaryPosition = leftBoundary.position.x;
        float rightBoundaryPosition = rightBoundary.position.x;

        //clamp the basket position to prevent it from going outside the screen
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, leftBoundaryPosition, rightBoundaryPosition);
        transform.position = clampedPosition;

        HandleTouchInput();

    }

    //handle the touch input
    private void HandleTouchInput()
    {
        //check if there are active touches
        if (Touch.activeTouches.Count > 0)
        {
            //get the first active touch
            Touch touch = Touch.activeTouches[0];

            //on touch start, initialize the previous position
            if (touch.phase == TouchPhase.Began)
            {
                isTouching = true;
                previousTouchPosition = touch.screenPosition;
            }

            //on touch move, calculate the movememnt
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                // Difference in touch positions
                Vector2 touchDelta = touch.screenPosition - previousTouchPosition;

                // Translate delta to horizontal movement
                float moveDelta = touchDelta.x * Time.deltaTime;

                // Apply movement to the player
                transform.position += new Vector3(moveDelta, 0, 0);

                // Update the previous position
                previousTouchPosition = touch.screenPosition; 
            }

            // On touch end, stop tracking
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
            }
        }

        #region simulatedMouseInput for testing MobileTouch
        // Simulated mouse input
        else if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            if (!isTouching)
            {
                // Begin "touch" when the mouse is clicked
                isTouching = true;
                previousTouchPosition = mousePosition;
            }
            else
            {
                // Simulate touch movement
                Vector2 touchDelta = mousePosition - previousTouchPosition;
                float moveDelta = touchDelta.x * Time.deltaTime;
                transform.position += new Vector3(moveDelta, 0, 0);
                previousTouchPosition = mousePosition;
            }
        }
        else
        {
            // End simulated "touch" when the mouse is released
            isTouching = false;
        }

        #endregion

    }

    public void Move(InputAction.CallbackContext context)
    {
        //movement on the x axis
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

}
