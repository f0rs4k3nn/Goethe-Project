using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameranearspawn : MonoBehaviour
{
     public GameObject camera1;
    public Transform spawnpoint;
    void Start()
    {
        camera1 = GameObject.Find("Main Camera");
        camera1.transform.position = spawnpoint.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
