using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsSceneController : MonoBehaviour
{
    [SerializeField]private Animator textAnimator;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            textAnimator.speed = 3;
        else
            textAnimator.speed = 1;
    }

    public void OnAnimationEnd()
    {
        SceneManager.LoadScene(0);
    }
}
