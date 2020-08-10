using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public GameObject animated;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            animated.GetComponent<Animation>().Play();
        }
    }
}
