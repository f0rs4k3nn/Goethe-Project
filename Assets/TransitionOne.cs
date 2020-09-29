using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionOne : MonoBehaviour
{
    public GameObject canvas;
    public GameObject cameraAnim;
    public GameObject player;
    public GameObject mainCamera;
    public GameObject fire;
    GameObject instantiatedFire;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            mainCamera.GetComponent<ThirdPersonCamera>().enabled = false;
            mainCamera.GetComponent<AudioSource>().Play();
            instantiatedFire = Instantiate(fire, gameObject.transform);
            Destroy(player.gameObject);
            StartCoroutine(OmgWhatsThat());
        }
    }

    IEnumerator OmgWhatsThat()
    {
        yield return new WaitForSeconds(1);
        mainCamera.transform.SetParent(cameraAnim.transform);
        cameraAnim.GetComponent<Animation>().Play();
        canvas.GetComponent<AudioSource>().Play();
        Destroy(instantiatedFire);
        yield return new WaitForSeconds(1f);
        mainCamera.GetComponent<StressReceiver>().InduceStress(10);
        yield return new WaitForSeconds(1f);
        mainCamera.GetComponent<StressReceiver>().InduceStress(25);
        yield return new WaitForSeconds(24);
        SceneManager.LoadScene(1);
    }
}
