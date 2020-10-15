using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenAdditiveScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("INITIALIZE");
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {

        float elapsedSeconds = 0;

        while (GameManager.hasToInitialize && elapsedSeconds < 10)
        {
            GameManager.Instance.Initialize();
            yield return new WaitForSeconds(0.05f);
            elapsedSeconds += 0.05f;
        }
    }
}
