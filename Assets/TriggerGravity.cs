using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGravity : MonoBehaviour
{   public GameObject movingObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            if (movingObject.GetComponent<Rigidbody>())
            {
                movingObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
