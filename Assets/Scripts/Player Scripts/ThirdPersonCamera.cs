using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    public float mouseSensitivity = 10;
    private Transform target;
    private float dstFromtarget = 2;
    public float pitchMin = -40;
    public float pitchMax = 85;

    public float rotationSmoothTime = 1.2f;
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;

    private float yaw;
    private float pitch;

    private bool canMove = false;

    private UnityStandardAssets.Cameras.ProtectCameraFromWallClip clipControl;

    private void Awake()
    {
        GameManager.Instance.Camera = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("CameraFollow").transform;
        canMove = true;
        clipControl = GetComponent<UnityStandardAssets.Cameras.ProtectCameraFromWallClip>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (canMove)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        }

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
      //  Vector3 targetRotation = new Vector3(pitch, yaw);
        //transform.eulerAngles = targetRotation;
        transform.eulerAngles = currentRotation;


        dstFromtarget = clipControl.GetCurrentDistance();
        transform.position = target.position - transform.forward * dstFromtarget;
    }

    public void SetActive(bool isActive) 
    {
        canMove = isActive;
    }
}
