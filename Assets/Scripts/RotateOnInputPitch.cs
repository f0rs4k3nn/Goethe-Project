using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnInputPitch : MonoBehaviour
{
   
    public FloatingJoystick RotationJoystick;
    public float speed = 100.0f;
    
    private float rotationPitch = 0.0f;
    // Start is called before the first frame update


    // Update is called once per frame
    void  Update()
    {   
        Transform partent = gameObject.GetComponentInParent<Transform>();
        rotationPitch += RotationJoystick.Vertical * speed *Time.deltaTime;
        transform.eulerAngles = new Vector3(rotationPitch,partent.transform.eulerAngles.y,0.0f);
        
        //transform.eulerAngles = new Vector3(0.0f,0.0f,rotationPitch);
    }
}
