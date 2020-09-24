using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractText : MonoBehaviour
{
    public string interactText;
    private static bool hasTools;
    private bool isIn;

    void Start()
    {
        interactText = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "ToolBox" && isIn)
        {
            interactText = "You found some tools!";
            hasTools = true;
            Debug.Log(hasTools);
            GameManager.Instance.interactText.text = interactText;
            Destroy(this);
        }

        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "Teleport_Terminal" && isIn)
        {
            if (hasTools == true)
            {
                interactText = "Terminal is now repaired!";
                GameManager.Instance.interactText.text = interactText;
                CustomTeleporter.teleportPadOn = true;
                Destroy(this);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            isIn = true;
    }
}
