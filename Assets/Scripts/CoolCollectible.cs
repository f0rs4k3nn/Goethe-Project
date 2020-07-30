using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolCollectible : MonoBehaviour
{
    public int points = 1;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            other.gameObject.GetComponent<PlayerBehaviour>().AddScore(points);
            Destroy(gameObject);
        }

        
    }
}
