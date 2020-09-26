using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;
    public GameObject playerModel;

    public TimeTravelMechanic TimeTravelMechanic;

    public new ThirdPersonCamera camera;
    
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI textSignUp;
    public TextMeshProUGUI textSignDown;
    public TextMeshProUGUI pickUpText;

    public GameObject dialogBox;
    public GameObject playerGameObj;
    public GameObject sign;
    public GameObject interactBttn;
    public GameObject[] interactiveObj;
    public static int currentLevel;

    //overwrites the last played level and score if 
    //played in the main save
    public static bool isMainSave;

    private GameObject levelFinishedMenu;
    private GameData _gameData;
    public const int lastLevel = 8; //the index of the final level
   // private Scene

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

            if (playerModel == null)
            {
                playerModel = GameObject.Find("PlayerModel");
            }

            player.SetActive(value);
        }
        get { return _isMovementEnabled; }
    }

    private bool _playerModelVisible;
    public bool playerModelVisible
    {
        set
        {
            _playerModelVisible = value;
            if (playerModel == null)
            {
                playerModel = GameObject.Find("PlayerModel");
            }

            playerModel.SetActive(value);
        }
        get { return _playerModelVisible; }
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

        dialogBox = GameObject.Find("DialogBox");

        textDisplay = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
        textSignUp = GameObject.Find("Up Text (TMP)").GetComponent<TextMeshProUGUI>();
        textSignDown = GameObject.Find("Down Text (TMP)").GetComponent<TextMeshProUGUI>();
        pickUpText = GameObject.Find("Pick Up Text (TMP)").GetComponent<TextMeshProUGUI>();


        playerGameObj = GameObject.Find("Player");

        sign = GameObject.Find("SignOverlay");

        interactBttn = GameObject.Find("Interact Button");

        interactiveObj = GameObject.FindGameObjectsWithTag("Interactive");

        camera = Camera.main.GetComponent<ThirdPersonCamera>();


        levelFinishedMenu = FindObjectOfType<LevelFinishedMenu>().gameObject;

        dialogBox.SetActive(false);
        interactBttn.SetActive(false);
        sign.SetActive(false);
    }

    private int _gameScore;
    public int GameScore
    {
        get { return _gameScore; }
        set { _gameScore = value; }
    }

    public void FinishedLevel()
    {
        IsMovementEnabled = false;

        try
        {
            currentLevel = FindObjectOfType<LevelManager>().levelIndex;
        }catch(System.Exception e)
        {
            Debug.Log(e.StackTrace);
        }


        playerModelVisible = false;
        currentLevel++;

        Debug.Log("I finished the level " + currentLevel);
        
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

            levelFinishedMenu.SetActive(true);
            levelFinishedMenu.GetComponent<LevelFinishedMenu>().finishedLevel = true;
        }
    }
}