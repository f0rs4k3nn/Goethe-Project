using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Translatable : MonoBehaviour
{
    public TranslationController.KEY key;
    private Text _text;

    private void Awake()
    {
        _text = this.GetComponent<Text>();
    }

    public string GetText()
    {
        return _text.text;
    }
    
    public void SetText(string value)
    {
        _text.text = value;
    }

    public void debug()
    {
        Debug.Log(name + " Has " +  _text + " and the key is " + key );
    }
}
