using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;

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

        dialogBox = GameObject.Find("DialogBox");

<<<<<<< HEAD
        textDisplay = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
        textSignUp = GameObject.Find("Up Text (TMP)").GetComponent<TextMeshProUGUI>();
        textSignDown = GameObject.Find("Down Text (TMP)").GetComponent<TextMeshProUGUI>();
        pickUpText = GameObject.Find("Pick Up Text (TMP)").GetComponent<TextMeshProUGUI>();
=======
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
>>>>>>> 15243ef152d07798785876ad54a68a3b7d9d81b6


        playerGameObj = GameObject.Find("Player");

<<<<<<< HEAD
        sign = GameObject.Find("SignOverlay");
=======
<<<<<<< HEAD
        interactBox = GameObject.Find("InteractBox");

        sign = GameObject.Find("SignOverlay");
=======
            sign = GameObject.Find("SignOverlay");
>>>>>>> a479fadd80434c9355e68466a7cb4e5563c01d02
>>>>>>> 15243ef152d07798785876ad54a68a3b7d9d81b6

        interactBttn = GameObject.Find("Interact Button");

        interactiveObj = GameObject.FindGameObjectsWithTag("Interactive");

        camera = Camera.main.GetComponent<ThirdPersonCamera>();

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