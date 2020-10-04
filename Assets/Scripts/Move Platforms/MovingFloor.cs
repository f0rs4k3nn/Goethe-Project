using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    public Transform movingPlatform;
    public Transform position1;
    public Transform position2;

    public Vector3 newPosition;
    public string currentState;

    public float smooth = 1;

    private void Update()
    {
        ChangeTarget();

        if (GameManager.keyPlatformActivated)
        {           
            movingPlatform.position = Vector3.Lerp(movingPlatform.position, newPosition, smooth * Time.deltaTime);
        }
        else
        {
            movingPlatform.position = Vector3.Lerp(movingPlatform.position, newPosition, smooth * Time.deltaTime);
        }
    }

    void ChangeTarget()
    {
        if (currentState == "Moving To Position 1" && GameManager.keyPlatformActivated)
        {
            currentState = "Moving To Position 2";
            newPosition = position2.position;
        }
        else if (currentState == "Moving To Position 2" && !GameManager.keyPlatformActivated)
        {
            currentState = "Moving To Position 1";
            newPosition = position1.position;
        }
        else if (currentState == "" && GameManager.keyPlatformActivated)
        {
            currentState = "Moving To Position 2";
            newPosition = position2.position;
        }
    }
}