using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuBehaviour : MonoBehaviour
{
    public int firstLevelIndex = 4;
    public Image fadeOverlay;
    public float fadeDelay = 1.0f;
    public GameObject mainScreen;
    public GameObject selectionScreen;
    public GameObject levelsParent;


    private SaveData _save;

    private void Start()
    {
        _save = GameData.gameData.saveData;
        Color fixedColor = fadeOverlay.color;
        fixedColor.a = 1;
        fadeOverlay.color = fixedColor;
        fadeOverlay.CrossFadeAlpha(1f, 0f, true);
        //foreach(GameObject g in levelsParent.transform.)

        StartCoroutine(StartMenu());
    }

    private IEnumerator StartMenu()
    {
        yield return new WaitForSeconds(fadeDelay);

        fadeOverlay.CrossFadeAlpha(0f, fadeDelay, true);

        yield return new WaitForSeconds(fadeDelay);

        fadeOverlay.gameObject.SetActive(false);
    }

    public void SwitchScreen()
    {
        AudioManager.instance.Play("Selection");
        mainScreen.SetActive(!mainScreen.active);
        selectionScreen.SetActive(!selectionScreen.active);
    }

    public void Continue()
    {
        StartCoroutine(ChoiceMade(_save.lastUnlockedLevel - firstLevelIndex + 1));
    }

    public void LoadLevel(int level)
    {
        StartCoroutine(ChoiceMade(level));
        //LoadingScreenManager.LoadScene(firstLevelIndex + level - 1);
    }

    private IEnumerator ChoiceMade(int levelIndex)
    {
        AudioManager.instance.Play("Selection");

        fadeOverlay.gameObject.SetActive(true);

        Color fixedColor = fadeOverlay.color;
        fixedColor.a = 1;
        fadeOverlay.color = fixedColor;
        fadeOverlay.CrossFadeAlpha(0f, 0f, true);

        fadeOverlay.CrossFadeAlpha(1f, fadeDelay, true);

        yield return new WaitForSeconds(fadeDelay);

        TextBoxBehaviour.currentDialogue = levelIndex;

        SceneManager.LoadScene(3);
    }

}
