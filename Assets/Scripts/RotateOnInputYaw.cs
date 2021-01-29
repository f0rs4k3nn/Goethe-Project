using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnInputYaw : MonoBehaviour
{
  
    public FloatingJoystick RotationJoystick;
    public float speed = 5.0f;
    private float rotationYaw = 0.0f;
   
    void  Update()
    {
        rotationYaw += RotationJoystick.Horizontal *speed*Time.deltaTime;
        transform.eulerAngles = new Vector3(0.0f,rotationYaw,0.0f);
    }
}
