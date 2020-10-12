using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBossTerminal : MonoBehaviour
{
    public static int shieldCount;
    public static bool shakeCam = false;
    private void Awake()
    {
        shieldCount = GameObject.FindGameObjectsWithTag("SecurityConsole").Length;
        Debug.Log("Consoles : " + shieldCount);
    } 

    private void Update()
    {
        if (shakeCam)
        {
            ThirdPersonCamera.shakeMagnitude = 0.1f;
        }
    }
    private void OnDestroy()
    {
        shakeCam = false;
        ThirdPersonCamera.shakeMagnitude = 0f;
    }
}
