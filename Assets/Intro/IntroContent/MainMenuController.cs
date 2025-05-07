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
        loadingScreen.SetActive(true);
        loadingBar.value = 0f;

        // Fade out menu
        yield return StartCoroutine(FadeCanvasGroup(menuCanvasGroup, 1, 0, fadeDuration));
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;
        menuCanvasGroup.gameObject.SetActive(false);

        if (scareImageCanvasGroup != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(scareImageCanvasGroup, 1, 0, fadeDuration));
            scareImageCanvasGroup.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        // Show loading screen
        loadingScreen.SetActive(true);
        fadeCanvasGroup.alpha = 0;
        yield return StartCoroutine(FadeCanvasGroup(fadeCanvasGroup, 0, 1, fadeDuration));

        // Begin async scene load (but don't allow activation yet)
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        // Fake loading from 0% to 90%
        float fakeProgress = 0f;
        while (fakeProgress < 0.9f)
        {
            fakeProgress += Time.deltaTime * 0.5f; // adjust speed here
            loadingBar.value = fakeProgress;
            yield return null;
        }

        loadingBar.value = 0.9f;

        // Wait for actual scene load to reach 90%
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Animate from 90% to 100%
        float t = 0f;
        float duration = 1f;
        while (t < duration)
        {
            t += Time.deltaTime;
            loadingBar.value = Mathf.Lerp(0.9f, 1f, t / duration);
            yield return null;
        }

        loadingBar.value = 1f;
        yield return new WaitForSeconds(0.5f);

        // Activate scene
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
