using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class StickToPlatform : MonoBehaviour
{
    private GameObject target = null;
    private Vector3 offset;
    GameObject player;
    float gIntensity;
    Transform empty;

    void Start()
    {
        player = GameObject.Find("Player");
        empty = Instantiate(new GameObject(), transform).transform;
    }

    private void OnTriggerStay(Collider other)
    {
        player.transform.parent = empty.transform;
    }

    void OnTriggerExit(Collider other)
    {
        player.transform.parent = null;
       // player.transform.localScale = Vector3.one;
    }
}
