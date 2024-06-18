using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CameraMode
{

    Free,
    Rotating

}

public class CameraOperations : MonoBehaviour
{

    [SerializeField] Transform rotatePointCamera, rotatePointLight;

    [SerializeField] GameObject helpMenuRef, lightA, lightB;

    [SerializeField, Range(0.05f, 0.1f)] private float sensitivity = 0.1f;

    [SerializeField, Range(5f, 50f)] private float rotateStateSpeed = 10f;

    [SerializeField] private CameraMode mode = CameraMode.Rotating;

    private Vector3 mouseInput = Vector3.zero, keyInput = Vector3.zero, mouseStartPos = Vector3.zero;

    bool rotateModeInitialized = false, helpVisible = false, allowRotation = true;

    void Start()
    {

        lightA.transform.parent = rotatePointLight;

        lightA.GetComponent<MeshRenderer>().material.color = Color.blue;

        lightB.GetComponent<MeshRenderer>().material.color = Color.red;

        helpMenuRef.SetActive(helpVisible);

        if (mode is CameraMode.Free)
        {

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }

    }

    void Update()
    {

        mouseInput = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);

        keyInput = new(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        Operate();

        RotateLight();

    }

    #region Button Methods

    public void SetModeFree() => mode = CameraMode.Free;
    public void SetModeRotating() => mode = CameraMode.Rotating;
    public void ToggleLightA() => lightA.SetActive(!lightA.activeInHierarchy);
    public void ToggleLightB() => lightB.SetActive(!lightB.activeInHierarchy);

    #endregion

    private void Operate()
    {

        //Help Menu Toggle
        if (Input.GetKeyDown(KeyCode.H))
        {

            helpVisible = !helpVisible;

            helpMenuRef.SetActive(helpVisible);

        }

        if (mode is CameraMode.Free)
        {

            transform.parent = null;
            rotateModeInitialized = false;
            allowRotation = true;

            //Cursor Controls
            if (Input.GetKeyDown(KeyCode.C))
            {

                if (Cursor.visible is false)
                {

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                }
                else
                {

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                }

            }

            //Free Camera Y-axis Movement
            keyInput.y = 0f;

            if (Input.GetKey(KeyCode.Space))
                keyInput.y += 1f;

            if (Input.GetKey(KeyCode.LeftControl))
                keyInput.y += -1f;


            transform.Translate(5f * Time.deltaTime * keyInput);

        }
        else if (mode is CameraMode.Rotating)
        {

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            //Sets initial fixed rotation state
            if (!rotateModeInitialized)
            {

                transform.position = new(-10f, 10f, 30f);
                transform.rotation = Quaternion.Euler(15f, 132f, 0f);
                transform.parent = rotatePointCamera;
                rotateModeInitialized = true;

            }

            if (Input.GetMouseButtonDown(1))
            {

                mouseStartPos = Input.mousePosition;

            }    
            else if (Input.GetMouseButton(1))
            {

                Vector3 lMousePosition = Input.mousePosition;

                Vector3 deltaMousePos = lMousePosition - mouseStartPos;

                lightB.transform.position += sensitivity/100f * new Vector3(deltaMousePos.x, 0f, deltaMousePos.y);

            }
            else if (Input.GetMouseButtonUp(1))
            {

                lightB.transform.position = new Vector3(10f, 5f, 15f);

            }

        }


        if (mode is CameraMode.Rotating && Input.GetKeyDown(KeyCode.C))
        {

            allowRotation = !allowRotation;

        }

        if (allowRotation)
            Rotate();

    }

    private void Rotate()
    {

        if (mode is CameraMode.Free)
        {

            Vector3 rotateVec = new(-mouseInput.y, mouseInput.x, 0f);
            transform.Rotate(50f * sensitivity * rotateVec);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);

        }
        else if (mode is CameraMode.Rotating)
        {

            transform.parent.Rotate(rotateStateSpeed * Time.deltaTime * Vector3.up);

        }

    }

    private void RotateLight()
    {

        lightA.transform.parent.Rotate(-50f * Time.deltaTime * Vector3.up);

    }

}
