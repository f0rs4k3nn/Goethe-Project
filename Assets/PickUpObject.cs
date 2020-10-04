using UnityEngine;
using System.Collections;

public class PickUpObject : MonoBehaviour {
    public float throwForce = 10;
    bool hasPlayer = false;
    bool beingCarried = false;

    void Update()
    {
        if(beingCarried)
        {
            if(Input.GetMouseButtonDown(0))
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                GetComponent<Rigidbody>().AddForce(GameManager.Instance.playerTransform.forward * throwForce);
				
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0) && hasPlayer)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                transform.parent = GameManager.Instance.playerTransform;
                beingCarried = true;
            }
        }
    }
}