using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeTravelMechanic : MonoBehaviour
{

    public float fadeDuration = 1.0f;
    private bool isInFuture;
    private GameObject[] pastObjects;
    private GameObject[] futureObjects;
    private Animator[] animatedObjects;
    private Image fadeScreen;
    private bool canTimeTravel = false;
    private bool keyWasReleased = true;
    private PlayerMisc player;


    private Light mainLight;

    public Color pastSkyBoxColour;
    public Color pastLightColour;
    public Color pastFogColour;
    public float pastLightIntensity;
    public float pastFogEndDist;

    public Color futureSkyBoxColour;
    public Color futureLightColour;
    public Color futureFogColour;
    public float futureLightIntensity;
    public float futureFogEndDist;

    public Material skyBox;





    // Start is called before the first frame update
    void Start()
    {
        
        //Skybox.
        player = FindObjectOfType<PlayerMisc>();
        fadeScreen = GetComponent<Image>();
        mainLight = GameObject.Find("Directional Light").GetComponent<Light>();
        pastObjects = GameObject.FindGameObjectsWithTag("Past Object");
        futureObjects = GameObject.FindGameObjectsWithTag("Future Object");
        animatedObjects = GameObject.FindObjectsOfType<Animator>();

        ChangeSceneEnvironment(true);
        canTimeTravel = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetAxisRaw("TimeTravel") > 0.3 && keyWasReleased && player.IsActive())
        {
            keyWasReleased = false;
            TriggerTimeTravel();
        } else if(Input.GetAxisRaw("TimeTravel") == 0)
        {
            keyWasReleased = true;
        }
    }

    /**
     * Trigger a time travel change.
     */
    public void TriggerTimeTravel()
    {
        if(canTimeTravel)
        {
            //pause every animation
            foreach(Animator anim in animatedObjects)
            {
                anim.speed = 0;
            }

            player.ActivateMovement(false);
            canTimeTravel = false;
            isInFuture = !isInFuture;

            //Select a random colour for the fade screen.
            fadeScreen.color = new Color(UnityEngine.Random.Range(0.3f, 0.8f), UnityEngine.Random.Range(0.3f, 0.8f), UnityEngine.Random.Range(0.3f, 0.8f));

            //Start the fade in of the time travel screen.
            StartCoroutine(Fade(0, 1, fadeDuration, true));
        }
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
            GetComponent<CanvasGroup>().alpha = Mathf.Lerp(start, end, normalizedTime);
            yield return null;
        }

        GetComponent<CanvasGroup>().alpha = end;

        if(isFadeIn)
        {
            ChangeSceneEnvironment(false);
        } else
        {
            canTimeTravel = true;

            //pause every animation
            foreach (Animator anim in animatedObjects)
            {
                anim.speed = 1;
            }

            player.ActivateMovement(true);

        }
    }

    /**
     * Changes the scene to either the past/future, depending on the isInFuture bool.
     */
    private void ChangeSceneEnvironment(bool isStartOfLevel)
    {
        mainLight.color = isInFuture ? futureLightColour : pastLightColour;
        mainLight.intensity = isInFuture ? futureLightIntensity : pastLightIntensity;

        RenderSettings.fogColor = isInFuture ? futureFogColour : pastFogColour;
        RenderSettings.fogEndDistance = isInFuture ? futureFogEndDist : pastFogEndDist;

        RenderSettings.ambientSkyColor = isInFuture ? futureSkyBoxColour : pastSkyBoxColour;

        skyBox.SetColor("_Color", isInFuture ? futureSkyBoxColour : pastSkyBoxColour);


        SetCollisions();
        /**
         * Change material jajajajajaja not yet.
         */

        if(!isStartOfLevel)
        {
            //End the time travel sequence by fading out the fade screen.
            StartCoroutine(Fade(1, 0, fadeDuration, false));
        }
    }

    private void SetCollisions()
    {
        foreach (GameObject obj in pastObjects)
        {
            if (obj == null)
                continue;

            obj.GetComponent<Collider>().enabled = !isInFuture;

            try
            {
                Material mat = obj.GetComponent<Renderer>().material;
                mat.SetFloat("Vector1_A397D302", (isInFuture ? 0.73f : 0.0f));
                mat.SetFloat("Vector1_B97EA0A9", (isInFuture ? 1 : 0));
            } catch(Exception e)
            {
                //literally nothing to do
                //we do this because there are some objects which simply don't have a material
            }
        }

        foreach (GameObject obj in futureObjects)
        {
            if (obj == null)
                continue;

            obj.GetComponent<Collider>().enabled = isInFuture;

            try
            {
                Material mat = obj.GetComponent<Renderer>().material;
                mat.SetFloat("Vector1_A397D302", (isInFuture ? 0.0f : 0.73f));
                mat.SetFloat("Vector1_B97EA0A9", (isInFuture ? 0 : 1));
            }
            catch (Exception e)
            {
                //literally nothing to do
                //we do this because there are some objects which simply don't have a material
            }


        }
    }

}
