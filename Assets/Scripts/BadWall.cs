using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadWall : MonoBehaviour
{

    private Transform spawnPoint;

    private void Start()
    {
        spawnPoint = GameObject.Find("SpawnPoint").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.name == "Player")
        {
            other.transform.position = spawnPoint.position;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

}
