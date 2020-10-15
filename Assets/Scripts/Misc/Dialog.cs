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
    private bool done = true;
    private bool isin = false;

    private GameManager game;

    void Start()
    {
        game = GameManager.Instance;       
    }

    private void Update()
    {
        if (isin)
        {
            if (GameManager.Instance.dialogBox.activeSelf || GameManager.Instance.sign.activeSelf)
                GameManager.Instance.interactBttn.SetActive(false);
            else
                GameManager.Instance.interactBttn.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.E) && isin)
        {
            if(done)
            {
                if (!startConversation)
                {
                    startConversation = true;
                    game.dialogBox.SetActive(true);
                    StartCoroutine(Type());
                    //Debug.Log("start convo");
                }
                else
                {
                    //Debug.Log("next convo")
                    NextSentence();
                }
            } else
            {
                game.textDisplay.text = sentencesNpc[index];
            }
            
        }       
    }
    private void OnTriggerStay(Collider player)
    {
        if (player.tag == "Player")
        {
            isin = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.interactBttn.SetActive(false);
            isin = false;
            startConversation = false;
            game.dialogBox.SetActive(false);
            ResetConversation();
        }
    }

    IEnumerator Type()
    {
        done = false;
      //  game.playerGameObj.GetComponent<PlayerController>().enabled = false;

        if (tag == "RandomTalk")
            index = Random.Range(0, sentencesNpc.Length - 1);

        foreach (char letter in sentencesNpc[index].ToCharArray())
        {
            if(game.textDisplay.text.Length == sentencesNpc[index].Length)
            {
                break;
            }

            AudioManager.instance.Play("LetterSound");
            game.textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        //game.playerGameObj.GetComponent<PlayerController>().enabled = true;
        done = true;
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
            //Debug.Log("ended convo");            
            startConversation = false;
            game.dialogBox.SetActive(false);
            ResetConversation();
        }
    }
    public void StartConversationAtSpawn()
    {
        
        if(!startConversation)
        {
            //Debug.Log("started dialog");
            startConversation = true;
            game.dialogBox.SetActive(true);
            StartCoroutine(Type());
        }
    }
    public void ResetConversation()
    {
        game.textDisplay.text = "";
        index = sentencesNpc.Length - 1;
    }
}
