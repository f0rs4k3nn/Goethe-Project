using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    TextMeshProUGUI continueTextDisplay;

    GameObject player;
    GameObject dialogueBox;

    public string[] sentencesNpc;
    public int index = 0;
    bool startConversation = false;
    public float typingSpeed;

    void Start()
    {
      
        textDisplay.text = "";
        
        player = GameObject.Find("Player");
        dialogueBox = GameObject.Find("DialogBox");
        dialogueBox.SetActive(false);
        
      //  dialogueBox.active = false;

    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!startConversation)
            {
                startConversation = true;
                dialogueBox.SetActive(true);
                StartCoroutine(Type());
            }
        }

        if (textDisplay.text == sentencesNpc[index] && startConversation && Input.GetKeyDown(KeyCode.E))
        {
            NextSentence();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        startConversation = false;
        dialogueBox.SetActive(false);
        ResetConversation();
    }

    IEnumerator Type()
    {
        //player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;

        foreach (char letter in sentencesNpc[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

       // player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerController>().enabled = true;
    }

    public void NextSentence()
    {
        if(index < sentencesNpc.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = sentencesNpc[index];
        }
    }

    public void ResetConversation()
    {
        textDisplay.text = "";
        index = 2;
    }

  
}
