using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : MonoBehaviour
{
    public static GameObject checkpoint;

    private void OnTriggerStay(Collider other)
    {
        if(other.name == "Player")
        {
            other.gameObject.transform.position = checkpoint.transform.position;
        }
    }
}
