using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Dialog : MonoBehaviour
{
    /*TextMeshProUGUI textDisplay;*/
    /*TextMeshProUGUI continueTextDisplay;*/

    GameObject player;
    GameObject dialogBox;

    public string[] sentencesNpc;
    public int index = 0;
    bool startConversation = false;
    public float typingSpeed;

    void Start()
    {
        /*textDisplay = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();*/

        GameManager.Instance.textDisplay.text = "";
        
        player = GameObject.Find("Player");

        /*dialogBox = GameObject.Find("DialogBox");
       
        dialogBox.SetActive(false);*/
    }

    private void OnTriggerStay(Collider player)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!startConversation)
            {
                startConversation = true;
                /*dialogBox.SetActive(true);*/
                GameManager.Instance.dialogBox.SetActive(true);
                StartCoroutine(Type());
            }
        }

        if (GameManager.Instance.textDisplay.text == sentencesNpc[index] && startConversation && Input.GetKeyDown(KeyCode.E))
        {
            NextSentence();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        startConversation = false;
        /*dialogBox.SetActive(false);*/
        GameManager.Instance.dialogBox.SetActive(false);
        ResetConversation();
    }

    IEnumerator Type()
    {
        //player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;

        foreach (char letter in sentencesNpc[index].ToCharArray())
        {
            GameManager.Instance.textDisplay.text += letter;
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
            GameManager.Instance.textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            GameManager.Instance.textDisplay.text = sentencesNpc[index];
        }
    }

    public void ResetConversation()
    {
        GameManager.Instance.textDisplay.text = "";
        index = sentencesNpc.Length - 1;
    }
}
