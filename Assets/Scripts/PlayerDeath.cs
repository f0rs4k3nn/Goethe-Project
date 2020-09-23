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

    private bool _isRespawning = false;
    
    void Awake()
    {
        game = GameManager.Instance;

        Transform spawnpoint = GameObject.Find("SpawnPoint").transform;

        if (spawnpoint != null)
        {
            transform.position = spawnpoint.position;
        } else
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
        game.IsMovementEnabled = false;


        yield return new WaitForSeconds(respawnDelay);

        Color fixedColor = screenOverlay.color;
        fixedColor.a = 1;
        screenOverlay.color = fixedColor;
        screenOverlay.CrossFadeAlpha(0f, 0f, true);

        screenOverlay.CrossFadeAlpha(1, fadeDuration, true);
        yield return new WaitForSeconds(fadeDuration);

        transform.position = checkpoint;
        game.IsMovementEnabled = true;
        _isRespawning = false;

        screenOverlay.CrossFadeAlpha(0, fadeDuration, true);
        yield return new WaitForSeconds(fadeDuration);


    }
}
