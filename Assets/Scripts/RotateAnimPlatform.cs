using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimPlatform : MonoBehaviour
{
    public float rotateSpeed = 0.4f;   
    void FixedUpdate()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);
    }
}
