using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class LevelFinisher : MonoBehaviour
{
    public GameObject finishParticle;
    private AudioSource song;
    private GameObject joystick;
    private GameObject timeTravelButton;
    private GameObject jumpButton;

    private void Start()
    {
        song = GetComponent<AudioSource>();
        joystick = GameObject.Find("Floating Joystick");
        timeTravelButton = GameObject.Find("TimeTravelButton");
        jumpButton = GameObject.Find("JumpButton");
    }


    private void OnTriggerEnter(Collider other)
    {
        //song.Stop();
        AudioManager.instance.Play("FinishLevel");

        joystick.SetActive(false);
        timeTravelButton.SetActive(false);
        jumpButton.SetActive(false);

        Instantiate(finishParticle, transform);


        GameManager.Instance.FinishedLevel();
    }
}
