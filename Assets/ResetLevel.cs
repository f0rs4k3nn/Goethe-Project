using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
            SceneManager.LoadScene("LevelSERBAN");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
