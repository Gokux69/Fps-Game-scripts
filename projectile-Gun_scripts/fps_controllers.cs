using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_controllers : MonoBehaviour
{
// public float walkingSpeed;
   // public float runningSpeed;
   // public float jumpSpeed;
   // public float gravity;
   // public Camera playerCamera;
   // public float lookSpeed;
   // public float lookXLimit;
   // CharacterController characterController;
   // Vector3 moveDirection = Vector3.zero;
    //float rotationX = 0;
   // [HideInInspector]
   // public bool canMove = true;
   
   public float walkingSpeed = 5f;  // Speed for walking
   public float runningSpeed = 10f;  // Speed for running
   [SerializeField] private FixedJoystick joystick; // Reference to the joystick

   private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
       // Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        // Get joystick input values
        float joystickVertical = joystick.Vertical; // Left stick Y-axis (forward/backward)
        float joystickHorizontal = joystick.Horizontal; // Left stick X-axis (left/right)

        // Determine if the player is running
        bool isRunning = Input.GetKey(KeyCode.LeftShift); // You can keep this for keyboard input if needed

        // Calculate current speed based on input
        float currentSpeed = isRunning ? runningSpeed : walkingSpeed;

        // Calculate movement direction
        Vector3 forward = transform.TransformDirection(Vector3.forward) * currentSpeed * joystickVertical;
        Vector3 right = transform.TransformDirection(Vector3.right) * currentSpeed * joystickHorizontal;
        
        // Combine forward and right movements
        Vector3 moveDirection = forward + right;

        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);
        }
    }



