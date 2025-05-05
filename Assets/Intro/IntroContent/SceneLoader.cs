using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public float delayBeforeLoad = 2f;

    public void StartGame()
    {
        StartCoroutine(LoadSceneWithDelay());
    }

    IEnumerator LoadSceneWithDelay()
    {
        loadingScreen.SetActive(true);

        yield return new WaitForSeconds(delayBeforeLoad);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main Scene");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
