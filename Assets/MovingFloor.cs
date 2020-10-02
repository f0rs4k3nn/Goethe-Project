using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    public Transform targetPosition;
    Transform startPosition;
    public bool moving = false;

    public Transform doorTargetPosition;
    public Transform door;
    Transform doorPosition;


    private void Start()
    {
        startPosition = transform;
        doorPosition = door.transform;
    }

    private void Update()
    {
        if (moving == true)
        {
            transform.position = Vector3.Lerp(startPosition.position, targetPosition.position, 1);
            door.position = Vector3.Lerp(doorPosition.position, doorTargetPosition.position, 1);
        }
    }

}