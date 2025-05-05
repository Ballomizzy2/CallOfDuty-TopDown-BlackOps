using UnityEngine;
using System.Collections;

public class UIShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 originalPos = rectTransform.anchoredPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            rectTransform.anchoredPosition = (Vector2)originalPos + new Vector2(offsetX, offsetY);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPos;
    }
}
