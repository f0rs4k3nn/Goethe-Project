using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTriggerTeleport : MonoBehaviour
{
    public Transform spwanPoint;
    private float duration = 0.6f;
    private PlayerMisc player;
    private CanvasGroup canvas;
    private Image fadeScreen;

    private void Start()
    {
        GameObject obj = GameObject.Find("TimeTravelManager");
        canvas = obj.GetComponent<CanvasGroup>();
        fadeScreen = obj.GetComponent<Image>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        
        player = other.gameObject.GetComponent<PlayerMisc>();
        player.ActivateMovement(false);
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
            player.gameObject.transform.position = spwanPoint.position;

            StartCoroutine(Fade(1, 0, duration, false));
        }
        else
        {
            
            player.ActivateMovement(true);
        }
    }
}
