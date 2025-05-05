using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuFader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1.5f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        // enables interaction after fade
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
    }
}
