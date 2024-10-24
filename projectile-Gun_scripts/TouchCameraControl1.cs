using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCameraControl1 : MonoBehaviour
{public Transform player; // Player (Capsule)
 public Transform cameraTransform; // Camera
 public Transform gunTransform; // Gun object
 public float sensitivity = 0.5f;
 private float rotationY = 0f;
 private float rotationX = 0f;
 
 void Update()
 {
     if (Input.touchCount > 0 || Input.GetMouseButton(0)) // Check for touch or mouse
     {
         // Get input from touch or mouse
         Vector3 touchDelta = Input.touchCount > 0 ? Input.GetTouch(0).deltaPosition : new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
 
         // Adjust Y-axis rotation for player (horizontal)
         rotationY += touchDelta.x * sensitivity;
 
         // Adjust X-axis rotation for camera (vertical)
         rotationX -= touchDelta.y * sensitivity;
         rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Limit vertical rotation to avoid camera flipping
 
         // Apply Y-axis rotation to the player (capsule)
         player.localRotation = Quaternion.Euler(0, rotationY, 0);
 
         // Apply X-axis rotation to the camera (rotate only on X-axis)
         cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
 
         // Rotate the gun around the player based on the camera's horizontal rotation (Y-axis)
         gunTransform.RotateAround(player.position, Vector3.up, touchDelta.x * sensitivity);
 
         // Optionally rotate the gun's pitch (X-axis) to match camera's up/down tilt
        // gunTransform.localRotation = Quaternion.Euler(rotationX, gunTransform.localRotation.eulerAngles.y, 0);
     }
 }

}
