using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTalksATM : MonoBehaviour
{
    public string[] Sentences;
    void Start()
    {
        GetComponent<Dialog>().sentencesNpc = Sentences;
    }   
    
}
