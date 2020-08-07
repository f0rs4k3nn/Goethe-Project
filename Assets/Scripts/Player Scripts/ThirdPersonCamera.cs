using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject player;

    public float mouseSensitivity = 10;
    public Transform target;
    public float dstFromtarget = 2;
    public float pitchMin = -40;
    public float pitchMax = 85;

    public float rotationSmoothTime = 1.2f;
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;

    private float yaw;
    private float pitch;

    private bool canMove = false;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    void Update()
    {
        if (player.GetComponent<PlayerController>().normalG)
        {
            target.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z);
        }
        else
        {
            target.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 2, player.transform.position.z);
        }
    }

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



        transform.position = target.position - transform.forward * dstFromtarget;
    }

    public void SetActive(bool isActive) 
    {
        canMove = isActive;
    }
}
