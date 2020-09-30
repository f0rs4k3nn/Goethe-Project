using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class LevelFinisher : MonoBehaviour
{
    public GameObject finishParticle;
    private AudioSource song;

    private void Start()
    {
        song = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        //song.Stop();
        AudioManager.instance.Play("FinishLevel");
        Instantiate(finishParticle, transform);

        GameManager.Instance.FinishedLevel();
    }
}
