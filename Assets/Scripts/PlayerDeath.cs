using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject checkPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            //oh no i checkpointed
            checkPoint = other.gameObject;
        }

        if (other.tag=="Death")
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
