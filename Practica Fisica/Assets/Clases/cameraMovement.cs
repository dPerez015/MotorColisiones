using UnityEngine;
using System.Collections;

public class cameraMovement : MonoBehaviour
{
    //
    // VARIABLES
    //

    public float turnSpeed = 4.0f;      // Speed of camera turning when mouse moves in along an axis
    public float panSpeed = 4.0f;       // Speed of the camera when being panned
    public float zoomSpeed = 4.0f;      // Speed of the camera going back and forth

    private Vector3 mouseOrigin;    // Position of cursor when mouse dragging starts
    private float isPanning;     // Is the camera being panned?
    private bool isRotating;    // Is the camera being rotated?
    private float isZooming;     // Is the camera zooming?
    private float isGoingUp;

    //
    // UPDATE
    //

    void Update()
    {
        // Get the left mouse button
        if (Input.GetMouseButtonDown(1))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isRotating = true;
        }

        // Get the right mouse button 
            isPanning = Input.GetAxisRaw("Horizontal");
        

        // Get the middle mouse button

            // Get mouse origin
            isZooming = Input.GetAxisRaw("Vertical");

        isGoingUp = Input.GetAxisRaw("Actuar");
        // Disable movements on button release
        if (!Input.GetMouseButton(1)) isRotating = false;

        // Rotate camera along X and Y axis
        if (isRotating)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
            transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
        }

        // Move the camera on it's XY plane
    
            
            Vector3 move = transform.right*isPanning*panSpeed * Time.deltaTime;
            transform.Translate(move, Space.Self);

        // Move the camera linearly along Z axis
            move = transform.forward*isZooming*panSpeed*Time.deltaTime;
            transform.Translate(move, Space.World);

            move = transform.up * isGoingUp * panSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);

    }
}
