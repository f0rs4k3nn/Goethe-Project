using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public Image fadeOverlay;
    public float fadeDelay = 1.0f;
    public GameObject pauseMenu;

    private Toggle m_MenuToggle;
    private float m_TimeScaleRef = 1f;
    private float m_VolumeRef = 1f;
    private bool m_Paused;

    private GameObject levelFinish;


    private void Awake()
    {
        levelFinish = FindObjectOfType<LevelFinishedMenu>().gameObject;
    }

    // Start is called before the first frame update
    public void GoToMenu()
    {
        Time.timeScale = m_TimeScaleRef;
        StartCoroutine(GoToMenuRoutine());
    }


    private IEnumerator GoToMenuRoutine()
    {

        AudioManager.instance.Play("Selection");
        

        AudioManager.instance.StopAudio();

        fadeOverlay.gameObject.SetActive(true);

        Color fixedColor = fadeOverlay.color;
        fixedColor.a = 1;
        fadeOverlay.color = fixedColor;
        fadeOverlay.CrossFadeAlpha(0f, 0f, true);

        fadeOverlay.CrossFadeAlpha(1f, fadeDelay, true);

        yield return new WaitForSeconds(fadeDelay);

        
        SceneManager.LoadScene(0);
    }



    private void MenuOn()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        m_TimeScaleRef = Time.timeScale;
        Time.timeScale = 0f;

        m_VolumeRef = AudioListener.volume;
        AudioListener.volume = 0f;

        m_Paused = true;

        pauseMenu.SetActive(true);
    }


    public void MenuOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenu.SetActive(false);
        Time.timeScale = m_TimeScaleRef;
        AudioListener.volume = m_VolumeRef;
        m_Paused = false;
        AudioManager.instance.Play("Selection");
    }

#if !MOBILE_INPUT
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !levelFinish.active)
        {
            if(m_Paused)
            {
                MenuOff();
            } else
            {
                MenuOn();
            }
        }
    }
#endif
}
