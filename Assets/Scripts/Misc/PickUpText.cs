using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpText : MonoBehaviour
{
    public string pickUpText;

    public static bool pickUpTools = false;

    private void Start()
    {
        pickUpText = "";
    }

    private void OnTriggerStay(Collider player)
    {
        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "ToolBox")
        {
            if (!pickUpTools)
            {
                pickUpText = "You found some tools!";
                GameManager.Instance.pickUpText.text = pickUpText;
                pickUpTools = true;               
            }
            else
            {
                return;
            }
        }
    }
}
