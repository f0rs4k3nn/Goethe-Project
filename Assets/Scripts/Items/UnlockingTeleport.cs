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
            if (GameManager.Instance.ScoringSystem.Score >= 10 && name == "TeleporterPad")
            {
                GameManager.Instance.ScoringSystem.Score -= 10;
                GetComponent<Teleport>().enabled = true;
                Destroy(this);
            }

            if (GameManager.Instance.ScoringSystem.Score >= 10 && name == "DoubleJumpUnlocker" )
            {
                GameManager.Instance.ScoringSystem.Score -= 10;
                other.GetComponent<PlayerController>().doubleJumpUnlocked = true;
                Destroy(this);
            }
        }
    }
}
