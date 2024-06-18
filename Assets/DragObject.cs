/*  CSCI-B481/B581 - Fall 2022 - Mitja Hmeljak
	This script provides "dragging" functionality.
	The mouse screen coordinates are transformed to scene (World) coordinates,
	before using the mouse position to "drag" an object in the scene.
	Original demo code by CSCI-B481 alumnus Rajin Shankar, IU Computer Science.
 */

using System;
using UnityEngine;

[ExecuteInEditMode]
public class DragObject : MonoBehaviour
{

    private Camera myMainCamera;
    private Vector2 myObjectStartPosition, myMouseStartWorldPosition;

    private Vector2 startMousePosition;

    private Transform _transform;
    new public Transform transform
    {
        get
        {

            // GetComponent<type>()
            //    returns the component with name type
            //        if the game object has one attached,
            //    null if it doesn't.
            return _transform ?? (_transform = GetComponent<Transform>());
            // Note: in C# the "null coalescing" operator does the following:
            //    x ?? y   Evaluates to y if x is null, to x otherwise.
        } // end of get
    } // end of public Transform transform


    // Awake is called when the script instance is being loaded.
    //   It is used to initialize any variables or state before the game starts.
    //   Awake is called after all objects are initialized,
    //   so it can be used to communicate with other objects.
    private void Awake()
    {
        // obtain the main Camera used in the scene:
        myMainCamera = Camera.main;
    }

    // OnMouseDown() is an event handler, it is called when
    //   the mouse button is clicked while over any GUIElement or Collider object in the scene
    private void OnMouseDown()
    {

        startMousePosition = Input.mousePosition;

        // if debug is necessary, uncomment these lines:
        // Debug.Log("OnMouseDown() lMousePosition = " + lMousePosition);
        // Debug.Log("OnMouseDown() myMouseStartWorldPosition = " + myMouseStartWorldPosition);
        // Debug.Log("OnMouseDown() myObjectStartPosition = " + myObjectStartPosition);
    }

    // OnMouseDrag() is an event handler, it is called when
    //   the mouse button is "dragging" any GUIElement or Collider object in the scene
    private void OnMouseDrag()
    {

        Vector2 lMousePosition = Input.mousePosition;

        Vector2 deltaMousePos = lMousePosition - startMousePosition;

        float radius = transform.GetComponent<BoxCollider>().bounds.extents.x;

        float dr = deltaMousePos.magnitude;

        Vector3 rotationAxis = new Vector3(-deltaMousePos.y/dr, deltaMousePos.x/dr, 0f).normalized;

        float angle = dr / radius;

        transform.rotation = Quaternion.AngleAxis(angle, rotationAxis);

    // Debug.Log("OnMouseDown() lMousePosition = " + lMousePosition);
    // Debug.Log("OnMouseDrag() lMouseCurrentWorldPosition = " + lMouseCurrentWorldPosition);
    // Debug.Log("OnMouseDrag() transform.position = " + transform.position);
}

} // end of class DragObject
