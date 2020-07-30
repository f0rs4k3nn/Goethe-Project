using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetScript : MonoBehaviour
{
    private bool isFollowingPlayer = false;

    private Transform playerT;
    private Transform parentT;
    public int speed = 20;

    private void Start()
    {
        parentT = transform.parent.transform;
    }

    private void Update()
    {
        if (!isFollowingPlayer)
            return;


        parentT.position = Vector3.MoveTowards(parentT.position,
                            playerT.position,
                            speed * Time.deltaTime);
    }




    private void OnTriggerEnter(Collider other)
    {
        playerT = other.transform;
        transform.parent.GetComponent<BoxCollider>().enabled = false;
        isFollowingPlayer = true;
    }
}
