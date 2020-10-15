using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    private Vector3 checkpoint;
    private GameManager game;
    public float fadeDuration;
    public float respawnDelay;
    public Image screenOverlay;

    public GameObject deathParticle;
    public GameObject respawnParticle;

    private AudioManager audioManager;



    private bool _isRespawning = false;
    
    void Awake()
    {
        game = GameManager.Instance;  
    }

    private void Start()
    {
        audioManager = AudioManager.instance;

        GameObject spawnpoint = GameObject.Find("SpawnPoint");

        if (spawnpoint != null)
        {
            transform.position = spawnpoint.transform.position;
        }
        else
        {
            Debug.Log("DIDN'T FIND SPAWNPOINT AAa");
        }

        checkpoint = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "CheckPoint")
        {
            //oh no i checkpointed
            checkpoint = other.transform.position;
        } else if (other.tag=="Death")
        {
            if (!_isRespawning)
            {
                _isRespawning = true;
                StartCoroutine(RespawnTriggered());
            } 
        }
    }

    public void CheckPoint()
    {
       // GetComponent<PlayerController>().parentTransform = null;
        transform.position = checkpoint;
    }

    private IEnumerator RespawnTriggered()
    {
        GameObject particle = Instantiate(deathParticle, transform);
        particle.transform.parent = null;
        game.playerModelVisible = false;
        Destroy(particle, 3);

        audioManager.Play("DeathSound");
        
        game.IsMovementEnabled = false;


        yield return new WaitForSeconds(respawnDelay);
        audioManager.Play("RespawnSound");


        Color fixedColor = screenOverlay.color;
        fixedColor.a = 1;
        screenOverlay.color = fixedColor;
        screenOverlay.CrossFadeAlpha(0f, 0f, true);

        screenOverlay.CrossFadeAlpha(1, fadeDuration, true);
        yield return new WaitForSeconds(fadeDuration);

        game.player.SetVelocity(Vector3.zero); //reset the player's speed
        transform.position = checkpoint;
        game.playerModelVisible = true;
        game.IsMovementEnabled = true;
        _isRespawning = false;

        particle = Instantiate(respawnParticle, transform);
        particle.transform.parent = null;

        screenOverlay.CrossFadeAlpha(0, fadeDuration, true);
        Destroy(particle, 3);
        //yield return new WaitForSeconds(fadeDuration);


    }
}
