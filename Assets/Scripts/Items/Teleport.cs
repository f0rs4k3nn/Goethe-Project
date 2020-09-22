using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject target;

    private void OnTriggerEnter(Collider player)
    {
        if(this.isActiveAndEnabled)

        if (player.tag == "Player")
        {
            player.transform.position = target.transform.position;
        }
    }
}
