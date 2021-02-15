using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    public TranslationController translationController;
    public string Language
    {
        get
        {
            return PlayerPrefs.GetString("Language", "en_GB");
        }
        set
        {
            PlayerPrefs.SetString("Language", value);
            translationController.RefreshLanguage(value);
        }
    }

    public PlayerController player;
    public GameObject playerModel;
    public Transform playerTransform;

    public TimeTravelMechanic TimeTravelMechanic;

    public TouchscreenCameraRotation camera;

    public ScoringSystem ScoringSystem;

    public PurchaseSystem PurchaseSystem;

    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI textSignUp;
    public TextMeshProUGUI textSignDown;
    public TextMeshProUGUI continueText;
    //public TextMeshProUGUI pickUpText;

    public TextMeshProUGUI interactText;
    public GameObject interactBox;

    public GameObject dialogBox;
    public GameObject playerGameObj;
    public GameObject sign;
    public GameObject interactBttn;
    public GameObject[] interactiveObj;
    public GameObject finalDoorPark;
    public GameObject joystick;
    public GameObject timeTravelButton;
    public GameObject jumpButton;
    public GameObject pauseButton;

    public static int currentLevel;

    public static bool hasToInitialize = true;
    public static bool keyPlatformActivated = false;

    public int CurrentCharacterModelIndex
    {
        set
        {
            if(value < 10)
                PlayerPrefs.SetInt("CurrentCharacterIndex", value);
        }
        get { return PlayerPrefs.GetInt("CurrentCharacterIndex", 0); }
    }

    private GameObject levelFinishedMenu;
    private SaveData _save;
    public const int lastLevel = 13; //the index of the final level
   // private Scene

   public int TotalScrap
   {
       get { return PlayerPrefs.GetInt("Scrap", 0); }
       set { if(value > 0) PlayerPrefs.SetInt("Scrap", value);}
   }

   private bool _isMovementEnabled;
    public bool IsMovementEnabled
    {
        set
        {
            _isMovementEnabled = value;
            if(camera == null)
            {
                camera = FindObjectOfType<TouchscreenCameraRotation>();
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
                playerModel = player.PlayerModels[CurrentCharacterModelIndex].gameObject;
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
        Initialize();
    }

   // public void Start()
   // {
   //     Initialize();
   // }

    public void Initialize()
    {
        try
        {
            //Remove fade screen if game started from unity
            if (!LoadingScreenManager.currentlyLoading)
            {
                Destroy(GameObject.Find("LoadFade"));
            }

            // Debug.Log("I am initializing A AH AHA AH and the bool is " + hasToInitialize);
            if (!hasToInitialize) //exit if already initialized
                return;


            _save = GameData.gameData.saveData;
            IsMovementEnabled = true;

            dialogBox = GameObject.Find("DialogBox");

            textDisplay = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
            textSignUp = GameObject.Find("Up Text (TMP)").GetComponent<TextMeshProUGUI>();
            textSignDown = GameObject.Find("Down Text (TMP)").GetComponent<TextMeshProUGUI>();
            interactText = GameObject.Find("Interact Text (TMP)").GetComponent<TextMeshProUGUI>();
            continueText = GameObject.Find("Continue (TMP)").GetComponent<TextMeshProUGUI>();
            interactBox = GameObject.Find("InteractBox");
            
            interactBttn = GameObject.Find("Interact Button");
            playerGameObj = GameObject.Find("Player");
            finalDoorPark = GameObject.Find("FinalDoorPark");

            sign = GameObject.Find("SignOverlay");

            interactiveObj = GameObject.FindGameObjectsWithTag("Interactive");

            playerTransform = GameObject.Find("Player").transform;

            joystick = GameObject.Find("Floating Joystick");
            timeTravelButton = GameObject.Find("TimeTravelButton");
            jumpButton = GameObject.Find("JumpButton");
            pauseButton = GameObject.Find("PauseButton");

            textSignUp.text = "";
            textSignDown.text = "";
            textDisplay.text = "";

            camera = Camera.main.GetComponent<TouchscreenCameraRotation>();

            levelFinishedMenu = FindObjectOfType<LevelFinishedMenu>().gameObject;

            levelFinishedMenu.SetActive(false);
            dialogBox.SetActive(false);
            interactBttn.SetActive(false);
            sign.SetActive(false);
            ScoringSystem.Score = 0;

            hasToInitialize = false;

            _isMovementEnabled = true;

        } catch(System.Exception e)
        {
            Debug.Log("Loading " + e + " incomplete");
        }
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
        TotalScrap += ScoringSystem.Score; // Add score to total scrap for purchases
        
        if(currentLevel > lastLevel)
        {
            StartCoroutine(GameFinished());
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

    IEnumerator GameFinished()
    {
        Image fadeOverlay = GameObject.Find("DeathOverlay").GetComponent<Image>();

        float fadeDelay = 3.0f;

        Color fixedColor = Color.white;
        fixedColor.a = 1;
        fadeOverlay.color = fixedColor;
        fadeOverlay.CrossFadeAlpha(0f, 0f, true);

        fadeOverlay.CrossFadeAlpha(1f, fadeDelay, true);

        yield return new WaitForSeconds(fadeDelay);

        AudioManager.instance.StopAudio();
        SceneManager.LoadScene(lastLevel + 1);
    }
}
