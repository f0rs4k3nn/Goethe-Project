using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionOne : MonoBehaviour
{
    public GameObject cameraAnim;
    public GameObject fire;
    GameObject instantiatedFire;
    //GameObject cumvreau;

    private void Start()
    {
        //cumvreau = GameObject.Find("LevelFinish");
        //cumvreau.transform.GetChild(0).gameObject.SetActive(false);
        //cumvreau.transform.GetChild(1).gameObject.SetActive(false);
       // StartCoroutine(InitializeArrow());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.camera.enabled = false;
            instantiatedFire = Instantiate(fire, gameObject.transform);
            StartCoroutine(OmgWhatsThat());
        }
    }

  //  IEnumerator InitializeArrow()
  //  {
  //      while (cumvreau.transform.parent == null)
  //      {
  //          Debug.Log("checking for player...");
  //          cumvreau.transform.SetParent(GameManager.Instance.playerTransform);
  //          cumvreau.transform.localPosition = new Vector3(0, 2, 0.5f);
  //          cumvreau.transform.localRotation = new Quaternion(0, 0, 0, 0);
  //          yield return null;
  //      }
  //  }
  
    IEnumerator OmgWhatsThat()
    {
        GameManager.Instance.camera.transform.SetParent(cameraAnim.transform);
        cameraAnim.GetComponent<Animation>().Play();
        Destroy(instantiatedFire);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.camera.gameObject.AddComponent<StressReceiver>();
        GameManager.Instance.camera.GetComponent<StressReceiver>().InduceStress(10);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.camera.GetComponent<StressReceiver>().InduceStress(25);
        yield return new WaitForSeconds(5f);
        // cumvreau.transform.GetChild(0).gameObject.SetActive(true);
        // cumvreau.transform.GetChild(1).gameObject.SetActive(true);
        LoadingScreenManager.LoadScene(17);
    }
}
