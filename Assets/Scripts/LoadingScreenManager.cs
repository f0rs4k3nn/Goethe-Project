// LoadingScreenManager
// --------------------------------
// built by Martin Nerurkar (http://www.martin.nerurkar.de)
// for Nowhere Prophet (http://www.noprophet.com)
//
// Licensed under GNU General Public License v3.0
// http://www.gnu.org/licenses/gpl-3.0.txt

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class LoadingScreenManager : MonoBehaviour {

    [Header("Loading Visuals")]
    //public Image loadingIcon;
	//public Image progressBar;
	public Image fadeOverlay;

	[Header("Timing Settings")]
	public float waitOnLoadEnd = 0.25f;
	public float fadeDuration = 0.25f;

	[Header("Loading Settings")]
	public LoadSceneMode loadSceneMode = LoadSceneMode.Single;
	public ThreadPriority loadThreadPriority;

	[Header("Other")]
	// If loading additive, link to the cameras audio listener, to avoid multiple active audio listeners
	public AudioListener audioListener;

	AsyncOperation operation;
	Scene currentScene;

	public static int sceneToLoad = -1;
	// IMPORTANT! This is the build index of your loading scene. You need to change this to match your actual scene index
	static int loadingSceneIndex = 1;
    static int mainSceneIndex = 2;
    public static bool currentlyLoading = false;
    
	public static void LoadScene(int levelNum) {				
		Application.backgroundLoadingPriority = ThreadPriority.High;
		sceneToLoad = levelNum;
		SceneManager.LoadScene(loadingSceneIndex);
	}

	void Start() {
		if (sceneToLoad < 0)
			return;

        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;


        fadeOverlay.gameObject.SetActive(true); // Making sure it's on so that we can crossfade Alpha
		currentScene = SceneManager.GetActiveScene();
        // Debug.Log("GOT HERE");
        StartCoroutine(LoadAsync(sceneToLoad));
	}

	private IEnumerator LoadAsync(int levelNum) {

        currentlyLoading = true;

        ShowLoadingVisuals();

        FadeOut();
        yield return new WaitForSeconds(fadeDuration);

        StartOperation(levelNum);

		float lastProgress = 0f;

        GameManager.hasToInitialize = true;

        // operation does not auto-activate scene, so it's stuck at 0.9
        while (DoneLoading() == false) {
			yield return null;

			if (Mathf.Approximately(operation.progress, lastProgress) == false) {
				//progressBar.fillAmount = operation.progress;
				lastProgress = operation.progress;
			}
		}

        FadeIn();
        yield return new WaitForSeconds(fadeDuration);

        //loads the camera, canvas and player
        SceneManager.LoadSceneAsync(mainSceneIndex, LoadSceneMode.Additive);

        if (loadSceneMode == LoadSceneMode.Additive)
			audioListener.enabled = false;

		ShowCompletionVisuals();

        float elapsedSeconds = 0;

        while(GameManager.hasToInitialize && elapsedSeconds < 10)
        {
            GameManager.Instance.Initialize();
            yield return new WaitForSeconds(0.05f);
            elapsedSeconds += 0.05f;
        }

        if(elapsedSeconds == 10)
        {
            Debug.LogError("FAILED TO LOAD");
        }

        Image gameFade = null;

        try
        {
            gameFade = GameObject.Find("LoadFade").GetComponent<Image>();
        }
        catch (System.Exception e)
        {
            Debug.Log(e + " Activate the fucking loadFade");
        }


        if(gameFade != null)
        {
            gameFade.CrossFadeAlpha(1, 0, true);
            gameFade.CrossFadeAlpha(0, fadeDuration, true);
            Destroy(gameFade.gameObject, fadeDuration);
        }

        currentlyLoading = false;

        if (loadSceneMode == LoadSceneMode.Additive)
        {
            SceneManager.UnloadSceneAsync(currentScene.name);
        }
        else
			operation.allowSceneActivation = true;
	}




	private void StartOperation(int levelNum) {
		Application.backgroundLoadingPriority = loadThreadPriority;
		operation = SceneManager.LoadSceneAsync(levelNum, loadSceneMode);


		if (loadSceneMode == LoadSceneMode.Single)
			operation.allowSceneActivation = false;
	}

	private bool DoneLoading() {
		return (loadSceneMode == LoadSceneMode.Additive && operation.isDone) || (loadSceneMode == LoadSceneMode.Single && operation.progress >= 0.9f); 
	}

	void FadeIn() {
		fadeOverlay.CrossFadeAlpha(1, fadeDuration, true);
	}

	void FadeOut() {
		fadeOverlay.CrossFadeAlpha(0, fadeDuration, true);
	}

	void ShowLoadingVisuals() {
		//loadingIcon.gameObject.SetActive(true);
		//loadingDoneIcon.gameObject.SetActive(false);

		//progressBar.fillAmount = 0f;
	}

	void ShowCompletionVisuals() {
		//loadingIcon.gameObject.SetActive(false);
		//loadingDoneIcon.gameObject.SetActive(true);

		//progressBar.fillAmount = 1f;
	}

}