using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Look : MonoBehaviour
{
    [Header("Mouse settings")]
    [SerializeField] private float sensitivity;
    [SerializeField] private float clampLookDown;
    [SerializeField] private float clampLookUp;

    private Transform cameraTransform;
    private float newCameraRotation;   

    void Start()
    {
        cameraTransform = GameObject.Find("Main Camera").transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        PlayerLook();
    }


    // The players body will rotate around Y-axis (sideways).
    // The camera will rotate around the X-axis (up and down).
    private void PlayerLook()
    {
        // Get input from mouse
        float xAxisInput = Input.GetAxis("Mouse X");
        float yAxisInput = Input.GetAxis("Mouse Y");

        // Multiply by sense and DT to get stable mouse speed not dependent on FPS.
        xAxisInput *= sensitivity * Time.deltaTime;
        yAxisInput *= sensitivity * Time.deltaTime;
        
        // Rotate player around Y-axis with input from the mouse X-axis.
        transform.Rotate(Vector3.up * xAxisInput);

        // Gather rotation amount from the mouse Y-axis.
        // Clamp rotation
        // Then rotate camera around the X-axis with the amount gathered.
        newCameraRotation -= yAxisInput;
        newCameraRotation = Mathf.Clamp(newCameraRotation, clampLookDown, clampLookUp);
        cameraTransform.localRotation = Quaternion.Euler(newCameraRotation, 0f, 0f);
    }
}
