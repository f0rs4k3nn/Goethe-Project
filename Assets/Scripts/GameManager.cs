using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;
    public TimeTravelMechanic TimeTravelMechanic;
    public new ThirdPersonCamera camera;
    public GameObject dialogueBox;

    //overwrites the last played level and score if 
    //played in the main save
    public static bool isMainSave; 

    private GameData _gameData;
    public const int lastLevel = 6; //the index of the final level
   // private Scene

    private int currentLevel;

    private bool _isMovementEnabled;
    public bool IsMovementEnabled
    {
        set
        {
            _isMovementEnabled = value;
            if(camera == null)
            {
                camera = FindObjectOfType<ThirdPersonCamera>();
            }
            camera.SetActive(value);

            if(player == null)
            {
                player = FindObjectOfType<PlayerController>();
            }
            player.SetActive(value);
        }
        get { return _isMovementEnabled; }
    }

    
    new public void Awake()
    {
        if (instance == null)
        {
            instance = this as GameManager;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _gameData = GameData.gameData;
        IsMovementEnabled = true;
        dialogueBox = GameObject.Find("DialogBox");
        camera = Camera.main.GetComponent<ThirdPersonCamera>();
        dialogueBox.SetActive(false);
    }

    private int _gameScore;
    public int GameScore
    {
        get { return _gameScore; }
        set { _gameScore = value; }
    }

    public void FinishedLevel()
    {
        currentLevel++;
        if(currentLevel > lastLevel)
        {
            Debug.Log("End GAME");
        } else
        {
            Debug.Log("Level finished");

            if(isMainSave)
            {
                _gameData.saveData.lastPlayedLevel = currentLevel;

                if (currentLevel > _gameData.saveData.lastUnlockedLevel)
                {
                    _gameData.saveData.lastUnlockedLevel = currentLevel;
                }

                _gameData.Save();
            } else
            {
                Debug.Log("Yes");
            }
        }
    }
}