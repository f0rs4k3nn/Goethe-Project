using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    TextMeshProUGUI continueTextDisplay;

    GameObject player;

    public string[] sentencesNpc;
    public int index = 0;
    bool startConversation = false;
    bool freezeMovement;
    public float typingSpeed;

    void Start()
    {
      
        textDisplay.text = "";
        
        player = PlayerManager.instance.player;
        PlayerManager.instance.DialogBox.SetActive(false);   
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!startConversation)
            {
                startConversation = true;
                PlayerManager.instance.DialogBox.SetActive(true);
                StartCoroutine(Type());
            }
        }

        if (textDisplay.text == sentencesNpc[index] && startConversation && Input.GetKeyDown(KeyCode.E))
        {
            NextSentence();
        }
    }

    private void Update()
    {
        FreezeMovement();
    }

    private void OnTriggerExit(Collider other)
    {
        startConversation = false;
        PlayerManager.instance.DialogBox.SetActive(false);
        ResetConversation();
    }

    IEnumerator Type()
    {
        freezeMovement = true;

        foreach (char letter in sentencesNpc[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        freezeMovement = false;
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
        index = 1;
    }

    public void FreezeMovement()
    {
        if (freezeMovement)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<PlayerController>().enabled = false;
        }
        else
        {
            player.GetComponent<PlayerController>().enabled = true;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
