using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    bool isIn = false;

    private void Update()
    {
        if (isIn)
        {
            if (GameManager.Instance.dialogBox.activeSelf || GameManager.Instance.sign.activeSelf)
                GameManager.Instance.interactBttn.SetActive(false);
            else
                GameManager.Instance.interactBttn.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
            isIn = true;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isIn = false;
            GameManager.Instance.interactBttn.SetActive(false);
        }            
    }
}
