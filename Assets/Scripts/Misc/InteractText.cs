using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InteractText : MonoBehaviour
{
    string interactText;
    private static bool hasTools;
    private static bool hasRustyKey;
    private bool isIn;
    private void Awake()
    {
        hasTools = false;
        hasRustyKey = false;
    }
    void Start()
    {
        interactText = "";
    }

    private void Update()
    {

        //Interact with objects in scene
        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "ToolBox" && isIn)
        {
            interactText = "You found some tools!";
            hasTools = true;
            SlideTweenIn();
            GameManager.Instance.interactText.text = interactText;
            StartCoroutine(DestroyInteractScript());
        }

        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "Teleport_Terminal" && isIn)
        {
            if (!hasTools)
            {
                interactText = "Terminal is broken! If only you had some tools to fix it...";
                GameManager.Instance.interactText.text = interactText;
                SlideTweenIn();
            }

            if (hasTools)
            {
                interactText = "Terminal is now repaired!";
                GameManager.Instance.interactText.text = interactText;
                CustomTeleporter.teleportPadOn = true;
                GameManager.Instance.interactBttn.SetActive(false);
                SlideTweenIn();
                StartCoroutine(DestroyInteractScript());
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "Rusty_Key" && isIn)
        {
            interactText = "You found a rusty key!";
            hasRustyKey = true;
            GameManager.Instance.interactText.text = interactText;
            SlideTweenIn();
            StartCoroutine(DestroyInteractScript());
            Destroy(gameObject);            
        }

        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "MetalCabinetRusty" && isIn)
        {
            if (!hasRustyKey)
            {
                interactText = "This rusty locker is locked.";
                GameManager.Instance.interactText.text = interactText;
                SlideTweenIn();
            }

            if (hasRustyKey)
            {
                interactText = "The key worked! The locker is now open!";
                GameManager.Instance.interactText.text = interactText;
                GameManager.Instance.interactBttn.SetActive(false);
                SlideTweenIn();
                StartCoroutine(DestroyInteractScript());
            }
        }
        //Nivel MoJo
        if (Input.GetKeyDown(KeyCode.E) && name == "BossConsole" && isIn)
        {
            if(KillBossTerminal.shieldCount == 0)
            {
                interactText = "YOU DID IT! ... YOU ACTUALLY DID IT!";
                GameManager.Instance.interactText.text = interactText;
                GameManager.Instance.interactBttn.SetActive(false);
                SlideTweenInVictory();
                StartCoroutine(DestroyInteractScript());
            }
            else
            {
                interactText = "The terminal is still shielded. " + KillBossTerminal.shieldCount + " security terminals remain to deactivate";
                GameManager.Instance.interactText.text = interactText;
                GameManager.Instance.interactBttn.SetActive(false);
                SlideTweenIn();
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && tag == "SecurityConsole" && isIn)
        {
            KillBossTerminal.shieldCount --;             
            interactText = "Security Console Deactivated. ";
            FollowMeConsoles.Waypoints.Remove(transform);
            Debug.Log("Removed " + transform);
            if (KillBossTerminal.shieldCount <= 0)
            {
                KillBossTerminal.shieldCount = 0;
                interactText += "The Shutdown terminal is now ready to be used";
            }
            else
            {
                interactText += " " + KillBossTerminal.shieldCount + " security terminals left to deactivate"; 
            }
            
            GameManager.Instance.interactText.text = interactText;
            GameManager.Instance.interactBttn.SetActive(false);
            SlideTweenIn();
            StartCoroutine(DestroyInteractScript());
        }
        ////////////

            // Show the interact button on screen
        if (isIn)
        {
            if (GameManager.Instance.dialogBox.activeSelf || GameManager.Instance.sign.activeSelf)
                GameManager.Instance.interactBttn.SetActive(false);
            else
                GameManager.Instance.interactBttn.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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

    IEnumerator DestroyInteractScript()
    {
        GameManager.Instance.interactBttn.SetActive(false);
        isIn = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(this);
    }

    void SlideTweenIn()
    {
        LeanTween.moveX(GameManager.Instance.interactBox, 250f, 2.5f).setEaseOutExpo().setOnComplete(SlideTweenOut);
    }

    void SlideTweenOut()
    {
        LeanTween.moveX(GameManager.Instance.interactBox, -250f, 2.5f).setEaseOutExpo();
    }

    void SlideTweenInVictory()
    {
        LeanTween.moveX(GameManager.Instance.interactBox, 250f, 2.5f).setEaseOutExpo().setOnComplete(SlideTweenOutVictory);
    }

    void SlideTweenOutVictory()
    {
        LeanTween.moveX(GameManager.Instance.interactBox, -250f, 0.5f).setEaseOutExpo().setOnComplete(SlideTweenInEndGame);
    }

    void SlideTweenInEndGame()
    {
        interactText = "Quickly, Enter the portal before the building colapses!";
        KillBossTerminal.shakeCam = true;
        GameManager.Instance.interactText.text = interactText;
        GameObject temp = GameObject.Find("LevelFinish");
        temp.transform.GetChild(0).gameObject.SetActive(true);
        temp.transform.GetChild(1).gameObject.SetActive(true);
        LeanTween.moveX(GameManager.Instance.interactBox, 250f, 5f).setEaseOutExpo().setOnComplete(SlideTweenOutEndGame);
    }
    void SlideTweenOutEndGame()
    {
        LeanTween.moveX(GameManager.Instance.interactBox, -250f, 2.5f).setEaseOutExpo();
    }
}
