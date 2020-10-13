using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public GameObject player;
    public GameObject explosion;

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(explosion, player.transform);
        player.GetComponent<AudioSource>().enabled = true;
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(player.gameObject);
        yield return new WaitForSeconds(2);
        Application.Quit();
    }
}
