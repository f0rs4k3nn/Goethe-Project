﻿using System.Collections;
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
    public TextMeshProUGUI interactText;

    public GameObject dialogBox;
    public GameObject interactBox;
    public GameObject playerGameObj;
    public GameObject sign;
    public GameObject interactBttn;
    public GameObject[] interactiveObj;
    public static int currentLevel;

    public static bool hasToInitialize = true;

    private GameObject levelFinishedMenu;
    private SaveData _save;
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

            if (player == null)
            {
                player = FindObjectOfType<PlayerController>();              
            }


            if (playerModel == null)
            {
                playerModel = GameObject.Find("PlayerModel");
            }

            if (camera != null) camera.SetActive(value);
            if (camera != null) player.SetActive(value);
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

    }

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        try
        {
            // Debug.Log("I am initializing A AH AHA AH and the bool is " + hasToInitialize);
            if (!hasToInitialize) //exit if already initialized
                return;


            _save = GameData.gameData.saveData;
            IsMovementEnabled = true;

            dialogBox = GameObject.Find("DialogBox");

<<<<<<< HEAD
        textDisplay = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
        textSignUp = GameObject.Find("Up Text (TMP)").GetComponent<TextMeshProUGUI>();
        textSignDown = GameObject.Find("Down Text (TMP)").GetComponent<TextMeshProUGUI>();
        interactText = GameObject.Find("Interact Text (TMP)").GetComponent<TextMeshProUGUI>();
=======
            textDisplay = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
            textSignUp = GameObject.Find("Up Text (TMP)").GetComponent<TextMeshProUGUI>();
            textSignDown = GameObject.Find("Down Text (TMP)").GetComponent<TextMeshProUGUI>();
            pickUpText = GameObject.Find("Pick Up Text (TMP)").GetComponent<TextMeshProUGUI>();
>>>>>>> a479fadd80434c9355e68466a7cb4e5563c01d02


            playerGameObj = GameObject.Find("Player");

<<<<<<< HEAD
        interactBox = GameObject.Find("InteractBox");

        sign = GameObject.Find("SignOverlay");
=======
            sign = GameObject.Find("SignOverlay");
>>>>>>> a479fadd80434c9355e68466a7cb4e5563c01d02

            interactBttn = GameObject.Find("Interact Button");

            interactiveObj = GameObject.FindGameObjectsWithTag("Interactive");

            textSignUp.text = "";
            textSignDown.text = "";
            textDisplay.text = "";

            camera = Camera.main.GetComponent<ThirdPersonCamera>();


            levelFinishedMenu = FindObjectOfType<LevelFinishedMenu>().gameObject;

            levelFinishedMenu.SetActive(false);
            dialogBox.SetActive(false);
            interactBttn.SetActive(false);
            sign.SetActive(false);

            hasToInitialize = false;

            Debug.Log("NUTSHACK");


            //Remove fade screen if game started from unity
            if(!LoadingScreenManager.currentlyLoading)
            {
                Destroy(GameObject.Find("LoadFade"));
            }

        } catch(System.Exception e)
        {
            Debug.Log("Loading " + e + " incomplete");
        }
        
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
            if (currentLevel > _save.lastUnlockedLevel)
            {
                _save.lastUnlockedLevel = currentLevel;
            }

            Debug.Log("Level finished");

            GameData.gameData.Save();

            levelFinishedMenu.SetActive(true);
            levelFinishedMenu.GetComponent<LevelFinishedMenu>().finishedLevel = true;
        }
    }
}