using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class EndingScript : MonoBehaviour
{
    public float timeBetweenLetters;
    public float timeBetweenLines;
    public Image fadeOverlay;
    public Image blackFade;
    public float fadeDelay = 3.0f;
    public string[] lines;
    public Text textBox;


    private AudioManager _audio;
    
    private int _currentIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        _audio = AudioManager.instance;

        _audio.Play("Opera");
        textBox.text = "";
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        Color fixedColor = fadeOverlay.color;
        fixedColor.a = 1;
        fadeOverlay.color = fixedColor;
        fadeOverlay.CrossFadeAlpha(1f, 0f, true);

        fadeOverlay.CrossFadeAlpha(0f, fadeDelay, true);

        yield return new WaitForSeconds(fadeDelay);

         foreach(string s in lines)
         {
             textBox.text = "";

             for (int i = 0; i < s.Length; i++)
             { 
                 //_audio.Play("LetterSound");
                 textBox.text += s[i];

                 yield return new WaitForSeconds(timeBetweenLetters);
             }

            yield return new WaitForSeconds(timeBetweenLines);
        }

        fadeOverlay.CrossFadeAlpha(0f, 0f, true);
        fadeOverlay.CrossFadeAlpha(1, fadeDelay, true);

        yield return new WaitForSeconds(fadeDelay);

        yield return new WaitForSeconds(0.5f);

        Color fixedColor1 = blackFade.color;
        fixedColor1.a = 1;
        blackFade.color = fixedColor1;
        blackFade.CrossFadeAlpha(0f, 0f, true);

        blackFade.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);

        SceneManager.LoadScene(14);

        _audio.StopAudio();

        Debug.Log("Switch to credits");
    }

}
