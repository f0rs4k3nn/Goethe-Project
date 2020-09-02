using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider player)
    {
        
        if (player.tag == "Player")
        {
            //Debug.Log("i'm here");
            player.transform.position = target.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
