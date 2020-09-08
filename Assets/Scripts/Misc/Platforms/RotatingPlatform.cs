﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float ySpeed;
    public float xSpeed;
    public float zSpeed;
    public float speedMultiplier = 1;

    private Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        rotation = new Vector3(xSpeed, ySpeed, zSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * speedMultiplier * Time.deltaTime);
    }
}
