using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ThirdPersonCamera : MonoBehaviour
{
    public bool shouldRotate = true;

    // The target we are following
    public Transform target;
    // The distance in the x-z plane to the target
    public float distance = 15.0f;
    // the height we want the camera to be above the target
    public float height = 6.0f;
    public float rotation = 30.0f;
    // How much we
    public float heightDamping = 2.0f;
    public float rotationDamping = 7.0f;
    float wantedRotationAngleY;
    float wantedRotationAngleX;
    float wantedHeight;
    float currentRotationAngleY;
    float currentRotationAngleX;
    float currentHeight;
    Quaternion currentRotation;

    void LateUpdate()
    {
         if (target)
         {
          
             //Calculate the current rotation angles
             wantedRotationAngleY = target.eulerAngles.y;
             wantedRotationAngleX = target.eulerAngles.x + rotation;
             wantedHeight = target.position.y + height;
             currentRotationAngleY = transform.eulerAngles.y;
             currentRotationAngleX = transform.eulerAngles.x;
             currentHeight = transform.position.y;
             // Damp the rotation around the y-axis
             currentRotationAngleY = Mathf.LerpAngle(currentRotationAngleY, wantedRotationAngleY, rotationDamping * Time.deltaTime);
             currentRotationAngleX = Mathf.LerpAngle(currentRotationAngleX, wantedRotationAngleX, rotationDamping * Time.deltaTime);
             // Damp the height
             currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
            // Convert the angle into a rotation
             currentRotation = Quaternion.Euler(currentRotationAngleX, currentRotationAngleY, 0f);
             // Set the position of the camera on the x-z plane to:
             // distance meters behind the target
             transform.position = target.position;
             transform.position -= currentRotation * Vector3.forward * distance;
            // Set the height of the camera
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
             // Always look at the target
             if (shouldRotate)
                 transform.LookAt(target);
         }
    }

    public static float shakeMagnitude = 0f;
    private bool canMove = false;

    private UnityStandardAssets.Cameras.ProtectCameraFromWallClip clipControl;

    private void Awake()
    {
        GameManager.Instance.camera = this;
        shakeMagnitude = 0;

         if (shouldRotate)
         {
                transform.LookAt(target);
            }
    }

    void Start()
    {
        canMove = true;
    }

    public void SetActive(bool isActive)
    {
        canMove = isActive;
    }
}
