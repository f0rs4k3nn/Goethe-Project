using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Vector3 checkpoint;
    private GameManager game;
    
    void Awake()
    {
        game = GameManager.Instance;
        checkpoint = transform.position;
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            //oh no i checkpointed
            checkpoint = other.transform.position;
        } else if (other.tag=="Death")
        {
            //oh no i dieded
           // GetComponent<PlayerController>().parentTransform = null;
            transform.position = checkpoint;
        }
    }

    public void CheckPoint()
    {
       // GetComponent<PlayerController>().parentTransform = null;
        transform.position = checkpoint;
    }
}
