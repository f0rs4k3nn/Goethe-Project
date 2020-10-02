using UnityEngine;
using System.Collections;

public class PickUpObject : MonoBehaviour {
    public Transform player;
    public float throwForce = 10;
    bool hasPlayer = false;
    bool beingCarried = false;

    
	void Start()
	
	{
		player = GameManager.Instance.playerGameObj.transform;


		
		
		
	}
	private void OnTriggerStay(Collider other)
    {
        if (!hasPlayer)
            other.GetComponent<MovingFloor>().moving = true;
		Debug.Log("trigger");
    }

    void OnTriggerEnter(Collider other)
    {
       if (other.tag == "Player")
			hasPlayer = true;
    }

    void OnTriggerExit(Collider other)
    {
		if (other.tag == "Player")
			hasPlayer = false;	
		
		
	}

    void Update()
    {
        if(beingCarried)
        {
            if(Input.GetMouseButtonDown(0))
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
                GetComponent<Rigidbody>().AddForce(player.forward * throwForce);
				
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0) && hasPlayer)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                transform.parent = player;
                beingCarried = true;
            }
        }
    }
}