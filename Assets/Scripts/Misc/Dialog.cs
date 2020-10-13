using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Dialog : MonoBehaviour
{
    public string[] sentencesNpc;
    public int index = 0;
    bool startConversation = false;
    public float typingSpeed;

    private GameManager game;

    void Start()
    {
        game = GameManager.Instance;       
    }

    private void OnTriggerStay(Collider player)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!startConversation)
            {
                startConversation = true;
                game.dialogBox.SetActive(true);
                StartCoroutine(Type());
            }
        }

        if (game.textDisplay.text == sentencesNpc[index] && startConversation && Input.GetKeyDown(KeyCode.E))
        {
            NextSentence();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        startConversation = false;
        game.dialogBox.SetActive(false);
        ResetConversation();
    }

    IEnumerator Type()
    {
        game.playerGameObj.GetComponent<PlayerController>().enabled = false;

        foreach (char letter in sentencesNpc[index].ToCharArray())
        {
            game.textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        game.playerGameObj.GetComponent<PlayerController>().enabled = true;
    }

    public void NextSentence()
    {
        if(index < sentencesNpc.Length - 1)
        {
            index++;
            game.textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            game.textDisplay.text = sentencesNpc[index];
        }
    }
    public void StartConversationAtSpawn()
    {
        Debug.Log("started dialog");
        startConversation = true;
        game.dialogBox.SetActive(true);
        StartCoroutine(Type());
    }
    public void ResetConversation()
    {
        game.textDisplay.text = "";
        index = sentencesNpc.Length - 1;
    }
}
