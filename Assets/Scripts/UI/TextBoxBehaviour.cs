using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxBehaviour : MonoBehaviour
{
    public float timeBetweenLetters;
    public GameObject nextSignal;
    public Text nameText;

    private bool _finished;
    private Text _textBox;
    private string[] _currentDialogue;
    private string[] _currentSpeakingCharacter;
    private int _currentIndex;

    private static int currentDialogue = 1;

    // Start is called before the first frame update
    void Start()
    {
        _textBox = GetComponent<Text>();
        DialogueScript.FetchDialogue(currentDialogue);
        _currentDialogue = DialogueScript.GetDialogueArray();
        _currentSpeakingCharacter = DialogueScript.GetSpeakingCharArray();
        _currentIndex = 0;

        PressedNext();
    }

    private void Update()
    {
        
    }

    public void PressedNext()
    {
        _textBox.text = "";
        nextSignal.SetActive(false);
        

        if(_currentIndex < _currentDialogue.Length)
        {
            StartCoroutine(WriteCurrentLine(_currentDialogue[_currentIndex]));
        } else // dialogue finished
        {
            Debug.Log("I finished :)");
        }
    }

    private IEnumerator WriteCurrentLine(string line)
    {
        nameText.text = _currentSpeakingCharacter[_currentIndex];

        for (int i = 0; i < line.Length; i++)
        {
            yield return new WaitForSeconds(timeBetweenLetters);
            _textBox.text += line[i];
        }

        nextSignal.SetActive(true);
        _currentIndex++;
    }
}
