using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int levelIndex;
    public string ambientMusic;
    public string music;

    private bool fetchIndexAutomatically = false;

    public TimeTravelPreset currentPreset;

    void Awake()
    {
        if(fetchIndexAutomatically)
        {
            levelIndex = SceneManager.GetActiveScene().buildIndex;
        }
        Debug.Log("My index is " + levelIndex);
    }

    private void Start()
    {
        AudioManager audio = AudioManager.instance;

        audio.PlayAmbient(ambientMusic);
        audio.Play(music);
    }
}

public class TimeTravelPreset
{
    [Space]
    public Color pastSkyBoxColour;
    public Color pastLightColour;
    public Color pastFogColour;
    public float pastLightIntensity;
    public float pastFogEndDist;
    [Space]
    public Color futureSkyBoxColour;
    public Color futureLightColour;
    public Color futureFogColour;
    public float futureLightIntensity;
    public float futureFogEndDist;

    
}
