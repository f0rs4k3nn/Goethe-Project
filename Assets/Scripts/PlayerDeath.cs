using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Transform checkPoint;
    private GameManager game;
    
    void Awake()
    {
        game = GameManager.Instance;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            //oh no i checkpointed
            checkPoint = other.transform;
        } else if (other.tag=="Death")
        {
            //oh no i dieded
           // GetComponent<PlayerController>().parentTransform = null;
            transform.position = checkPoint.transform.position;
        }
    }

    public void CheckPoint()
    {
       // GetComponent<PlayerController>().parentTransform = null;
        transform.position = checkPoint.transform.position;
    }
}
