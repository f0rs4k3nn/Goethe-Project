using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBossTerminal : MonoBehaviour
{
    public static int shieldCount;
    public static bool shakeCam = false;
    public static bool endgame_initiate = false;
    private bool ending = false;
    private bool done = true;
    private bool startConversation = false;
    public string Finalsentence;
    private int index = 0;

    private void Awake()
    {
        shieldCount = GameObject.FindGameObjectsWithTag("SecurityConsole").Length;
        Debug.Log("Consoles : " + shieldCount);
    } 

    private void Update()
    {
        if(endgame_initiate &&!ending)
        {
            InitiateEndgame();
        }
        
        if (shakeCam)
        {
            ThirdPersonCamera.shakeMagnitude = 0.1f;
        } 

    }  
    IEnumerator Type()
    {
        done = false;
        GameManager.Instance.playerGameObj.GetComponent<PlayerController>().enabled = false;

        foreach (char letter in Finalsentence.ToCharArray())
        {
            GameManager.Instance.textDisplay.text += letter;
            yield return null;
        }

        GameManager.Instance.playerGameObj.GetComponent<PlayerController>().enabled = true;
        done = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" && ending)
        {
            GameManager.Instance.dialogBox.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        shakeCam = false;
        ThirdPersonCamera.shakeMagnitude = 0f;
    }

    private  void InitiateEndgame()
    {
        ending = true;
        shakeCam = true;
        GameObject temp = GameObject.Find("LevelFinish");
        temp.transform.GetChild(0).gameObject.SetActive(true);
        temp.transform.GetChild(1).gameObject.SetActive(true);
        GameManager.Instance.dialogBox.SetActive(true);
        StartCoroutine(Type());
    }


}
