using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public GameObject viewContext;  // The object to be rotated
    public bool useMouse = false;   // Flag to control whether the mouse is being used for rotation

    private Vector3 lastMousePosition;  // To store the position of the mouse in the last frame
    private Vector3 screenCenter;       // Center point of the screen

    // Method to enable mouse control
    public void UseMouse(string msg)
    {
        useMouse = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (useMouse)
        {
            // Calculate mouse movement
            float mouseDeltaX = Input.GetAxis("Mouse X");  // Horizontal mouse movement
            float mouseDeltaY = Input.GetAxis("Mouse Y");  // Vertical mouse movement

            // Rotate around the local Y axis based on horizontal mouse movement
            if (mouseDeltaX != 0)
            {
                float rotationY = mouseDeltaX * 5f;  // Adjust rotation speed as needed
                viewContext.transform.Rotate(0, rotationY, 0, Space.Self);
            }

            // Rotate around the local Z axis based on vertical mouse movement
            if (mouseDeltaY != 0)
            {
                float rotationZ = -mouseDeltaY * 5f;  // Inverted to match the direction specified
                viewContext.transform.Rotate(0, 0, rotationZ, Space.Self);
            }
        }
    }

    // Optional: Method to disable mouse control
    public void DisableMouseControl(string msg)
    {
        useMouse = false;
    }
}
