using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class LevelFinisher : MonoBehaviour
{
    public GameObject finishParticle;
    public GameManager gameManager;
    private AudioSource song;


    private void Start()
    {
        song = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
    }


    private void OnTriggerEnter(Collider other)
    {
        //song.Stop();
        AudioManager.instance.Play("FinishLevel");

        gameManager.joystick.SetActive(false);
        gameManager.timeTravelButton.SetActive(false);
        gameManager.jumpButton.SetActive(false);

        Instantiate(finishParticle, transform);


        GameManager.Instance.FinishedLevel();
    }
}
