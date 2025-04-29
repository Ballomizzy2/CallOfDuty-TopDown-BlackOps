using System.Collections;
using UnityEngine;

public class LightFade : MonoBehaviour
{
    private Light lightComp;

    void Start()
    {
        lightComp = GetComponent<Light>();
        StartCoroutine(FadeLight());
    }

    IEnumerator FadeLight()
    {
        float t = 0f;
        float startIntensity = lightComp.intensity;

        while (t < 0.2f)
        {
            t += Time.deltaTime;
            lightComp.intensity = Mathf.Lerp(startIntensity, 0, t / 0.2f);
            yield return null;
        }

        Destroy(gameObject);
    }
}
