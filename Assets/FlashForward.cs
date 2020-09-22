using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlashForward : MonoBehaviour
{
    Color colorNow;

    public GameObject dirLight;
    public GameObject explosion1;
    public GameObject explosion2;
    public GameObject explosion3;
    public GameObject explosion4;

    private void Start()
    {
        colorNow = dirLight.GetComponent<Light>().color;
    }

    private void OnTriggerEnter(Collider other)
    {
        dirLight.GetComponent<Light>().intensity = 0.3f;
        dirLight.GetComponent<Light>().color = Color.red;
        explosion1.SetActive(true);
        explosion2.SetActive(true);
        explosion3.SetActive(true);
        explosion4.SetActive(true);
        StartCoroutine(OmgWhatsThat());
    }

    IEnumerator OmgWhatsThat()
    {
        yield return new WaitForSeconds(1);
        dirLight.GetComponent<Light>().intensity = 1;
        dirLight.GetComponent<Light>().color = colorNow;
        explosion1.SetActive(false);
        explosion2.SetActive(false);
        explosion3.SetActive(false);
        explosion4.SetActive(false);
        Destroy(gameObject);
    }
}
