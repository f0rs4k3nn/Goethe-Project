using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    { 
        
        if(other.gameObject.name.CompareTo("Player") == 0)
        {
            other.gameObject.GetComponent<Transform>().position = spawnPoint.position;
        }

    }

}
