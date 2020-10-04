using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KeyItem")
        {
            GameManager.keyPlatformActivated = true;
            Debug.Log(GameManager.keyPlatformActivated);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "KeyItem")
        {
            GameManager.keyPlatformActivated = false;
            Debug.Log(GameManager.keyPlatformActivated);
        }
    }
}
