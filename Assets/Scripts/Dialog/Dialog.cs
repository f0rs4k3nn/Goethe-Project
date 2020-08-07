using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    GameObject dialogBox;
    public string[] sentencesNpc;
    public int index = 0;
    bool startConversation = false;
    public float typingSpeed;

    void Start()
    {
        dialogBox = GameObject.Find("DialogBox");
        dialogBox.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!startConversation)
            {
                startConversation = true;
                dialogBox.SetActive(true);
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
        dialogBox.SetActive(false);
        ResetConversation();
    }

    private void Update()
    {
        
    }

    IEnumerator Type()
    {
        foreach (char letter in sentencesNpc[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
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
