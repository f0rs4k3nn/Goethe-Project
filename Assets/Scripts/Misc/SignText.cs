using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignText : MonoBehaviour
{
    public string[] sentencesSign;
    bool isIn = false;

    private GameManager game;

    void Start()
    {
        game = GameManager.Instance;  
    }

    private void Update()
    {
        if (clickScript.clicked == true && isIn)
        {
            clickScript.clicked = false;
            game.textSignUp.text = "";
            game.textSignDown.text = "";
            game.sign.SetActive(true);
            game.textSignUp.text += sentencesSign[0];
            game.textSignDown.text += sentencesSign[1];
        }
    }

    private void OnTriggerStay(Collider other)
    {
        isIn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isIn = false;
        game.sign.SetActive(false);
    }
}
