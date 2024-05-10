using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 50f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(1))
        {
            float horizontalInput = Input.GetAxis("CameraHorizontal");
            this.transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
            float verticalInput = Input.GetAxis("CameraVertical");
            mainCamera.transform.Rotate(Vector3.left, verticalInput * rotationSpeed *0.5f* Time.deltaTime);
        }
        
    }
}
