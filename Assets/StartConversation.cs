using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConversation : MonoBehaviour
{  
         

    void Start()
    {
        StartCoroutine(StartConversationDelayed());
    }
    IEnumerator StartConversationDelayed()
    {
        //GameManager.Instance.IsMovementEnabled = false;        
        yield return new WaitForSeconds(2);
        GetComponent<Dialog>().StartConversationAtSpawn();
        //GameManager.Instance.IsMovementEnabled = true;        
    }




    
}
