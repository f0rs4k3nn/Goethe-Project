using UnityEngine;
using UnityEngine.UI;

public class Translatable : MonoBehaviour
{
    [HideInInspector] public string key;
    private Text _text;

    public string Text
    {
        set
        { 
            _text.text = value; 
        }
        get 
        {
            return _text.text;
        }
    }
    
    private void Awake()
    {
        _text = GetComponent<Text>();
        key = Text;
    }

    public void debug()
    {
        Debug.Log(name + " Has " +  _text + " and the key is " + Text );
    }
}
