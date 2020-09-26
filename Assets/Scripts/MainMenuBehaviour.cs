using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuBehaviour : MonoBehaviour
{
    public int firstLevel;

    public void StartGame()
    {
        GameManager.isMainSave = true;
        //LoadingScreenManager.LoadScene(4);

        TextBoxBehaviour.currentDialogue = 1;
        SceneManager.LoadScene(3);
    }

    public void Continue()
    {
        GameManager.isMainSave = true;
        LoadingScreenManager.LoadScene(3);
    }

    public void LoadLevel(int level)
    {
        GameManager.isMainSave = false;
        LoadingScreenManager.LoadScene(firstLevel + level - 1);
    }

}
