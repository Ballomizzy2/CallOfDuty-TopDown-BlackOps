using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public AudioSource zombieAudio;
    public UIShake uiShake;
    public Slider loadingBar;

    public GameObject loadingScreen;
    public CanvasGroup fadeCanvasGroup;       // Loading screen canvas group
    public CanvasGroup menuCanvasGroup;       // Menu UI canvas group
    public CanvasGroup scareImageCanvasGroup; // ScareImage canvas group
    public float fadeDuration = 2.5f;
    public string sceneToLoad = "Main Scene";

    public void StartGameWithLoading()
    {
        if (zombieAudio != null) zombieAudio.Play();
        if (uiShake != null) StartCoroutine(uiShake.Shake(1f, 10f));
        StartCoroutine(LoadSceneWithDelay());
    }

    IEnumerator LoadSceneWithDelay()
    {
        loadingScreen.SetActive(true); // Shows the loading UI
        loadingBar.value = 0f;         // Reset progress just in case

        // Fade out menu
        yield return StartCoroutine(FadeCanvasGroup(menuCanvasGroup, 1, 0, fadeDuration));
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;
        menuCanvasGroup.gameObject.SetActive(false);

        // Fade out scare image
        if (scareImageCanvasGroup != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(scareImageCanvasGroup, 1, 0, fadeDuration));
            scareImageCanvasGroup.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        // Activate and fade in loading screen
        loadingScreen.SetActive(true);
        fadeCanvasGroup.alpha = 0;
        yield return StartCoroutine(FadeCanvasGroup(fadeCanvasGroup, 0, 1, fadeDuration));

        // Load the scene asynchronously and show progress
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f) // Scene loads to 0.9 before activation
        {
            loadingBar.value = asyncLoad.progress;
            yield return null;
        }

        // Optional: fill to 100%
        loadingBar.value = 1f;
        yield return new WaitForSeconds(1f);

        // Activate the scene
        asyncLoad.allowSceneActivation = true;
    }


    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float t = 0f;
        cg.alpha = start;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, t / duration);
            yield return null;
        }

        cg.alpha = end;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
