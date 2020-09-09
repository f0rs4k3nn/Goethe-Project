using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTriggerTeleport : MonoBehaviour
{
    public Transform spawnPoint;
    private float duration = 0.6f;
    private CanvasGroup canvas;
    private Image fadeScreen;
    
    private GameManager game;
    
    private void Awake()
    {
        game = GameManager.Instance;
    }

    private void Start()
    {
        GameObject obj = game.TimeTravelMechanic.gameObject;
        canvas = obj.GetComponent<CanvasGroup>();
        fadeScreen = obj.GetComponent<Image>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        
        game.IsMovementEnabled = false;
        fadeScreen.color = new Color(0.1f, 0.1f, 0.1f);

        StartCoroutine(Fade(0, 1, duration, true));
    }

    /// <summary>
    /// Initiates the fade of the screen for the time travel sequence.
    /// </summary>
    /// <param name="start">The start value</param>
    /// <param name="end">The end value</param>
    /// <param name="duration">The duration of the fade</param>
    /// <param name="isFadeIn">This bool serves to tell the function if we just started the 
    ///                       time travel sequence or if we are ending it.If it's true, the entire scene will be changed, otherwise
    ///                    it will simply do a fadeoutt</param>
    private IEnumerator Fade(float start, float end, float duration, bool isFadeIn)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            canvas.alpha = Mathf.Lerp(start, end, normalizedTime);
            yield return null;
        }

        canvas.alpha = end;

        if (isFadeIn)
        {
            game.Player.transform.position = spawnPoint.position;
            StartCoroutine(Fade(1, 0, duration, false));
        }
        else
        {
            game.IsMovementEnabled = true;
        }
    }
}
