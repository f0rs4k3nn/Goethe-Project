using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockingTeleport : MonoBehaviour
{
    private string sceneName;

    private void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && sceneName == "Dani")
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
