using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpText : MonoBehaviour
{
    public string pickUpText;

    private void Start()
    {
        pickUpText = "";
    }

    private void OnTriggerStay(Collider player)
    {
        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "ToolBox")
        {
            if (!CustomTeleporter.teleportPadOn)
            {
                pickUpText = "You found some tools!";
                GameManager.Instance.pickUpText.text = pickUpText;
                CustomTeleporter.teleportPadOn = true;               
            }
            else
            {
                return;
            }
        }
    }
}
