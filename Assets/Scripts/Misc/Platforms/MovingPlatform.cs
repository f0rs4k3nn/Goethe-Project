using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    /****
     * 
     * 'Platform' NEEDS TO BE THE FIRST CHILD
     * AND 'CheckPoints' NEEDS TO BE THE SECOND ONE
     * 
     * DO NOT CHANGE THE ORDER.
     * 
     *****/


    private Vector3[] checkPoints;
    private Transform platform;
    private Vector3 currentVelocity;
    private bool goingBack = false;
    private float errorThreshold = 0.08f;


    //The current destination
    private int currentPoint;

    //damping of the movement
    public float damping;

    //speed of the moving platform
    //Might need it if you have points with huge gaps in between
    public float maxSpeed;
    public float waitTimeBetweenPoints;
    
    //If it's a cycle, it will return to the first point
    //once the last one is reached
    //Otherwise it will go back in reverse order
    public bool cycles = false;

    //whether or not the sequence repeats automatically
    public bool repeats = true;

    //Whether or not the movement begins 
    public bool waitsForTrigger = false;




    void Start()
    {
        Application.targetFrameRate = 60;
        //checkPoints = transform.GetChild(2).GetComponentInChildren<Transform>(); 
        platform = transform.GetChild(0);
        checkPoints = new Vector3[transform.GetChild(1).transform.childCount];

        for (int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i] = transform.GetChild(1).GetChild(i).position;
        }

        platform.position = checkPoints[0];
        currentPoint = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(waitsForTrigger)
        {
            return;
        }

        Vector3 destination = checkPoints[currentPoint];
        platform.position = Vector3.SmoothDamp(platform.position, destination, ref currentVelocity, damping, maxSpeed, Time.deltaTime);

        Vector3 error = destination - platform.position;
        error = new Vector3( Mathf.Abs(error.x), Mathf.Abs(error.y), Mathf.Abs(error.z));

        if ((error.x < errorThreshold) && (error.y < errorThreshold) && (error.z < errorThreshold))
        {
            currentPoint += (goingBack ? -1 : 1);

            if(currentPoint >= checkPoints.Length)
            {
                if(repeats)
                {
                    if (cycles)
                    {
                        currentPoint = 0;
                    }
                    else
                    {
                        goingBack = true;
                        currentPoint = checkPoints.Length - 2;
                    }
                } else
                {
                    waitsForTrigger = true;
                }
                
            } else if(currentPoint < 0)
            {
                goingBack = false;
                currentPoint = 1;

                if(!repeats)
                {
                    waitsForTrigger = true;
                }
            }
        }
    }

    /**
     * Unpauses the sequence.
     */
    public void Resume()
    {
        waitsForTrigger = false;
    }

    public void Pause()
    {
        waitsForTrigger = true;
    }
}
