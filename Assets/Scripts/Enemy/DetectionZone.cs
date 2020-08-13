using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public MagnetEnemyBehaviour enemyBehaviour;

    private void OnTriggerEnter(Collider other)
    {
        enemyBehaviour.FollowPlayer();
    }

    private void OnTriggerExit(Collider other)
    {
        enemyBehaviour.PlayerEscaped();
    }
}
