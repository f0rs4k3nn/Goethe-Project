using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpText : MonoBehaviour
{
    public string pickUpText;

    static bool pickUpKey;

    private void OnTriggerStay(Collider player)
    {
        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "Box")
        {
            if (!pickUpKey)
            {
                pickUpText = "You found a key!";
                pickUpKey = true;               
            }
            else
            {
                return;
            }
        }
    }
}
