using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignText : MonoBehaviour
{
    public string[] sentencesSign;
    bool isIn = false;

    void Start()
    {
        GameManager.Instance.textSignUp.text = "";
        GameManager.Instance.textSignDown.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isIn)
        {
            GameManager.Instance.textSignUp.text = "";
            GameManager.Instance.textSignDown.text = "";
            GameManager.Instance.sign.SetActive(true);
            GameManager.Instance.textSignUp.text += sentencesSign[0];
            GameManager.Instance.textSignDown.text += sentencesSign[1];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isIn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isIn = false;
        GameManager.Instance.sign.SetActive(false);
    }
}
