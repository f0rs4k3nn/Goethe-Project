using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlashForward : MonoBehaviour
{
    public GameObject path1;
    Color colorNow;
    public GameObject dirLight;
    public GameObject explosion1;
    public GameObject explosion2;
    public GameObject explosion3;
    public GameObject explosion4;

    private void Start()
    {
        colorNow = dirLight.GetComponent<Light>().color;
        path1.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            dirLight.GetComponent<Light>().color = Color.red;
            explosion1.SetActive(true);
            explosion2.SetActive(true);
            explosion3.SetActive(true);
            explosion4.SetActive(true);
            StartCoroutine(OmgWhatsThat());
        }
    }

    IEnumerator OmgWhatsThat()
    {
        yield return new WaitForSeconds(1);
        dirLight.GetComponent<Light>().color = colorNow;
        explosion1.SetActive(false);
        explosion2.SetActive(false);
        explosion3.SetActive(false);
        explosion4.SetActive(false);
        path1.GetComponent<Animator>().Play("spawnPath1");
        Destroy(gameObject);
    }
}
