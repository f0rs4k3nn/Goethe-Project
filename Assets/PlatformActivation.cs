using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivation : MonoBehaviour
{
    private void Awake()
    {
        GameManager.keyPlatformActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KeyItem")
        {
            GameManager.keyPlatformActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "KeyItem")
        {
            GameManager.keyPlatformActivated = false;
        }
    }
}
