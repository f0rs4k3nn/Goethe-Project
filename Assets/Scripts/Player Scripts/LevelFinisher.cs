using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinisher : MonoBehaviour
{
    public GameObject finishParticle;

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(finishParticle, transform);
        GameManager.Instance.FinishedLevel();
    }
}
