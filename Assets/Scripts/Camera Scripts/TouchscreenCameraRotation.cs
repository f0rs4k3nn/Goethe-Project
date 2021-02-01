using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchscreenCameraRotation : MonoBehaviour
{
    public float Yaxis;
    public float Xaxis;

    //public float RotationMinY = -80f;
    //public float RotationMaxY = 80f;
    public float RotationMinX = -5f;
    public float RotationMaxX = 50f;

    public float rotationSensitivity = 0.1f;
    public float smooothTime = 0.12f;

    public float distance = 15.0f;
    private float dstFromtarget = 15.0f;

    public FixedTouchField touchField;
    public Transform target;

    Vector3 targetRotation;
    Vector3 currentVelocity;

    private bool canMove = false;
    private UnityStandardAssets.Cameras.ProtectCameraFromWallClip clipControl;
    public static float shakeMagnitude = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        GameManager.Instance.camera = this;
        shakeMagnitude = 0;
    }

    private void Start()
    {
        target = GameObject.Find("CameraRotationTarget").transform;
        canMove = true;
        clipControl = GetComponent<UnityStandardAssets.Cameras.ProtectCameraFromWallClip>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // For PC
        /*Yaxis += Input.GetAxis("Mouse X") * rotationSensitivity;
        Xaxis -= Input.GetAxis("Mouse Y") * rotationSensitivity;*/
        
        Yaxis += touchField.TouchDist.x * rotationSensitivity;
        Xaxis -= touchField.TouchDist.y * rotationSensitivity;


        Xaxis = Mathf.Clamp(Xaxis, RotationMinX, RotationMaxX);
        //Yaxis = Mathf.Clamp(Yaxis, RotationMinY, RotationMaxY);

        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVelocity, smooothTime);
        transform.eulerAngles = targetRotation;

        dstFromtarget = clipControl.GetCurrentDistance();
        Vector3 offset = UnityEngine.Random.insideUnitSphere * shakeMagnitude;

        transform.position = (target.position - transform.forward * dstFromtarget) + offset;
    }

    public void SetActive(bool isActive)
    {
        canMove = isActive;
    }
}
