using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxBehaviour : MonoBehaviour
{
    public float timeBetweenLetters;
    public GameObject nextSignal;
    public Text nameText;
    public Image fadeOverlay;
    public float fadeDelay = 1.0f;
    public Image docImage;

    
    private AudioManager _audio;
    private bool _finished = true;
    private Text _textBox;
    private string[] _currentDialogue;
    private string[] _currentSpeakingCharacter;
    private int _currentIndex;

    public static int currentDialogue = 1;

    // Start is called before the first frame update
    void Start()
    {
        _audio = AudioManager.instance;
        _textBox = GetComponent<Text>();
        DialogueScript.FetchDialogue(currentDialogue);
        _currentDialogue = DialogueScript.GetDialogueArray();
        _currentSpeakingCharacter = DialogueScript.GetSpeakingCharArray();
        _currentIndex = 0;
        _textBox.text = "";
        nameText.text = "";
        nextSignal.SetActive(false);
        Debug.Log("The current dialogue is " + currentDialogue);

        if(currentDialogue == 6)
        {
            docImage.enabled = false;
        }

        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        Color fixedColor = fadeOverlay.color;
        fixedColor.a = 1;
        fadeOverlay.color = fixedColor;
        fadeOverlay.CrossFadeAlpha(1f, 0f, true);

        yield return new WaitForSeconds(1); //wait 1 second for the scene to load

        _audio.Play("Tension");

        fadeOverlay.CrossFadeAlpha(0f, fadeDelay, true);   
        
        yield return new WaitForSeconds(fadeDelay);

        fadeOverlay.gameObject.SetActive(false);

        PressedNext();
    }

    private IEnumerator FinishDialogue()
    {
        fadeOverlay.gameObject.SetActive(true);
        fadeOverlay.CrossFadeAlpha(0f, 0f, true);
        fadeOverlay.CrossFadeAlpha(1, fadeDelay, true);

        yield return new WaitForSeconds(fadeDelay);
        _audio.StopAudio();

        LoadingScreenManager.LoadScene(currentDialogue + 3);
    }

    public void PressedNext()
    {
        Debug.Log("NEXT");

        if(_finished) //play next line
        {
            if (_currentIndex < _currentDialogue.Length)
            {
                StartCoroutine(WriteCurrentLine(_currentDialogue[_currentIndex]));
            }
            else // dialogue finished
            {
                StartCoroutine(FinishDialogue());
            }
        } else //skip current line
        {   
            _textBox.text = _currentDialogue[_currentIndex];
            LineFinished();      
        }    
    }

    private IEnumerator WriteCurrentLine(string line)
    {
        _textBox.text = "";
        nextSignal.SetActive(false);
        _finished = false;
        nameText.text = _currentSpeakingCharacter[_currentIndex];

        for (int i = 0; i < line.Length; i++)
        {
            if(_finished)
                break;
            
            _audio.Play("LetterSound");
            _textBox.text += line[i];

            yield return new WaitForSeconds(timeBetweenLetters);
        }

        if(!_finished) //checks if the text wasn't skipped
        {
            LineFinished();
        }      
    }

    private void LineFinished()
    {
        _finished = true;
        _currentIndex++;
        nextSignal.SetActive(true);
    }


}
