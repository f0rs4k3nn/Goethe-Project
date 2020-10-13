using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCheckpoint : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.name == "Player")
        {
            Die.checkpoint = gameObject;
        }
    }
}
