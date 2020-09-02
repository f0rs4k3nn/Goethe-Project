using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockingTeleport : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (ScoringSystem.theScore >= 10 && name == "TeleporterPad")
            {
                ScoringSystem.theScore -= 10;
                GetComponent<Teleport>().enabled = true;
                Destroy(this);
            }

            if (ScoringSystem.theScore >= 10 && name == "DoubleJumpUnlocker" )
            {
                ScoringSystem.theScore -= 10;
                other.GetComponent<PlayerController>().doubleJumpUnlocked = true;
                Destroy(this);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
