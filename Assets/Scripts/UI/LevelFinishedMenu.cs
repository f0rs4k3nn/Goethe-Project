using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelFinishedMenu : MonoBehaviour
{
    public Image fadeOverlay;
    public float fadeDelay = 1.0f;

    private CanvasGroup _cv;
    private bool _fadedIn = false;
    public bool finishedLevel;  
  

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        _cv = GetComponent<CanvasGroup>();
        _cv.alpha = 0;        
    }

    private void Update()
    {
        if(!finishedLevel)
        {
            return;
        }

        if(!_fadedIn)
        {
            _fadedIn = true;
            StartCoroutine(changeValueOverTime(0, 1, fadeDelay));
        }
    }

    IEnumerator changeValueOverTime(float fromVal, float toVal, float duration)
    {
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            float val = Mathf.Lerp(fromVal, toVal, counter / duration);
            _cv.alpha = val;
            yield return null;
        }
    }

    // Start is called before the first frame update
    public void GoToMenu()
    {
        StartCoroutine(ChoiceMade(false));
    }

    public void GoToNextLevel()
    {
        StartCoroutine(ChoiceMade(true));
    }

    private IEnumerator ChoiceMade(bool nextLevelSelected)
    {
        AudioManager.instance.Play("Selection");
        if (finishedLevel)
        {
            AudioManager.instance.StopAudio();

            fadeOverlay.gameObject.SetActive(true);

            Color fixedColor = fadeOverlay.color;
            fixedColor.a = 1;
            fadeOverlay.color = fixedColor;
            fadeOverlay.CrossFadeAlpha(0f, 0f, true);

            fadeOverlay.CrossFadeAlpha(1f, fadeDelay, true);

            yield return new WaitForSeconds(fadeDelay);

            if (nextLevelSelected)
            {
                int currentDialogue = GameManager.currentLevel - 3; //already incremented one

                TextBoxBehaviour.currentDialogue = currentDialogue;
                SceneManager.LoadScene(3);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
