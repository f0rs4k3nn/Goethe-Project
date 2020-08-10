using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boostSpeed : MonoBehaviour
{
    GameObject player;
    float boost; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(SpeedBoost());
        }
    }

    IEnumerator SpeedBoost()
    {
        boost = 500f;
        player.GetComponent<Rigidbody>().AddForce(player.GetComponent<Rigidbody>().velocity.normalized * boost * Time.deltaTime, ForceMode.Impulse);
        yield return new WaitForSeconds(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
