using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InteractText : MonoBehaviour
{
    string interactText;
    private static bool hasTools;
    private static bool hasRustyKey;
    private static bool hasScissors;
    private bool isIn;
    private GameManager game;

    private void Awake()
    {
        hasTools = false;
        hasRustyKey = false;
        hasScissors = false;
    }

    void Start()
    {
        interactText = "";
        game = GameManager.Instance;
    }

    private void Update()
    {

        //Interact with objects in scene
        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "ToolBox" && isIn)
        {
            interactText = "You found some tools!";
            hasTools = true;
            SlideTweenIn();
            game.interactText.text = interactText;
            StartCoroutine(DestroyInteractScript());
        }

        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "Teleport_Terminal" && isIn)
        {
            if (!hasTools)
            {
                interactText = "Terminal is broken! If only you had some tools to fix it...";
                game.interactText.text = interactText;
                SlideTweenIn();
            }

            if (hasTools)
            {
                interactText = "Terminal is now repaired!";
                game.interactText.text = interactText;
                CustomTeleporter.teleportPadOn = true;
                game.interactBttn.SetActive(false);
                SlideTweenIn();
                StartCoroutine(DestroyInteractScript());
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "Rusty_Key" && isIn)
        {
            interactText = "You found a rusty key!";
            hasRustyKey = true;
            game.interactText.text = interactText;
            SlideTweenIn();
            StartCoroutine(DestroyInteractScript());
            Destroy(gameObject);            
        }

        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "MetalCabinetRusty" && isIn)
        {
            if (!hasRustyKey)
            {
                interactText = "This rusty locker is locked.";
                game.interactText.text = interactText;
                SlideTweenIn();
            }

            if (hasRustyKey)
            {
                hasScissors = true;
                interactText = "The key worked! You found a pair of scissors!";
                game.interactText.text = interactText;
                game.interactBttn.SetActive(false);
                SlideTweenIn();
                StartCoroutine(DestroyInteractScript());
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && gameObject.name == "FinalDoorPark" && isIn)
        {
            if (!hasScissors)
            {
                interactText = "The door is closed tight by some ropes.";
                game.interactText.text = interactText;
                SlideTweenIn();
            }

            if (hasScissors)
            {
                interactText = "The ropes are cut! The door is open now.";
                game.interactText.text = interactText;
                game.interactBttn.SetActive(false);
                SlideTweenIn();
                RotateDoorTween();
                Destroy(transform.GetChild(0).gameObject);
                StartCoroutine(DestroyInteractScript());
            }
        }

        //Nivel MoJo
        if (Input.GetKeyDown(KeyCode.E) && name == "BossConsole" && isIn)
        {
            if(KillBossTerminal.shieldCount == 0)
            {
                KillBossTerminal.endgame_initiate = true;
                StartCoroutine(DestroyInteractScript());
            }
            else
            {
                interactText = "The terminal is still shielded. " + KillBossTerminal.shieldCount + " security terminals remain to deactivate";
                game.interactText.text = interactText;
                game.interactBttn.SetActive(false);
                SlideTweenIn();
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && tag == "SecurityConsole" && isIn)
        {
            KillBossTerminal.shieldCount --;             
            interactText = "Security Console Deactivated. ";
            FollowMeConsoles.Waypoints.Remove(transform);

            if (KillBossTerminal.shieldCount <= 0)
            {
                KillBossTerminal.shieldCount = 0;
                interactText += "The Shutdown terminal is now ready to be used";
            }
            else
            {
                interactText += " " + KillBossTerminal.shieldCount + " security terminals left to deactivate";
            }

            game.interactText.text = interactText;
            game.interactBttn.SetActive(false);
            SlideTweenIn();
            StartCoroutine(DestroyInteractScript());
        }
        ////////////

            // Show the interact button on screen
        if (isIn)
        {
            if (game.dialogBox.activeSelf || game.sign.activeSelf)
                game.interactBttn.SetActive(false);
            else
                game.interactBttn.SetActive(true);
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
            game.interactBttn.SetActive(false);
        }
    }

    IEnumerator DestroyInteractScript()
    {
        game.interactBttn.SetActive(false);
        isIn = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(this);
    }

    void SlideTweenIn()
    {
        LeanTween.moveX(game.interactBox, 250f, 2.5f).setEaseOutExpo().setOnComplete(SlideTweenOut);
    }

    void SlideTweenOut()
    {
        LeanTween.moveX(game.interactBox, -250f, 2.5f).setEaseOutExpo();
    }

    void RotateDoorTween()
    {
        LeanTween.rotateLocal(game.finalDoorPark, new Vector3(0f, -90f, 0f), 2.5f).setEaseOutExpo();
    }

    void SlideTweenInVictory()
    {
        LeanTween.moveX(game.interactBox, 250f, 2.5f).setEaseOutExpo().setOnComplete(SlideTweenOutVictory);
    }

    void SlideTweenOutVictory()
    {
        LeanTween.moveX(game.interactBox, -250f, 0.5f).setEaseOutExpo().setOnComplete(SlideTweenInEndGame);
    }

    void SlideTweenInEndGame()
    {
        interactText = "Quickly, Enter the portal before the building colapses!";
        KillBossTerminal.shakeCam = true;
        game.interactText.text = interactText;
        GameObject temp = GameObject.Find("LevelFinish");
        temp.transform.GetChild(0).gameObject.SetActive(true);
        temp.transform.GetChild(1).gameObject.SetActive(true);
        LeanTween.moveX(game.interactBox, 250f, 5f).setEaseOutExpo().setOnComplete(SlideTweenOutEndGame);
    }
    void SlideTweenOutEndGame()
    {
        LeanTween.moveX(game.interactBox, -250f, 2.5f).setEaseOutExpo();
    }
}
