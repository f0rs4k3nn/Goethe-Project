using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int levelIndex;
    private bool fetchIndexAutomatically = false;

    void Awake()
    {
        if(fetchIndexAutomatically)
        {
            levelIndex = SceneManager.GetActiveScene().buildIndex;
        }
        Debug.Log("My index is " + levelIndex);
    }

   
}
