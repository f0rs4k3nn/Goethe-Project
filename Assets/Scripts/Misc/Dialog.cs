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

    void Start()
    {
        GameManager.Instance.textDisplay.text = "";       
    }

    private void OnTriggerStay(Collider player)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!startConversation)
            {
                startConversation = true;
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
        GameManager.Instance.dialogBox.SetActive(false);
        ResetConversation();
    }

    IEnumerator Type()
    {
        GameManager.Instance.playerGameObj.GetComponent<PlayerController>().enabled = false;

        foreach (char letter in sentencesNpc[index].ToCharArray())
        {
            GameManager.Instance.textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        GameManager.Instance.playerGameObj.GetComponent<PlayerController>().enabled = true;
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
