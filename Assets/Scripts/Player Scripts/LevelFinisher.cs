using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class LevelFinisher : MonoBehaviour
{
    public GameObject finishParticle;
    private AudioSource song;
    private GameObject jump;
    private GameObject time;
    private GameObject walk;

    private void Start()
    {
        song = GetComponent<AudioSource>();
        jump = GameObject.Find("JumpButton");
        time = GameObject.Find("TimeTravelButton");
        walk = GameObject.Find("MobileJoystick");

    }


    private void OnTriggerEnter(Collider other)
    {
        //song.Stop();
        AudioManager.instance.Play("FinishLevel");
        
        Instantiate(finishParticle, transform);

        jump.SetActive(false);
        time.SetActive(false);
        walk.SetActive(false);

        GameManager.Instance.FinishedLevel();
    }
}
