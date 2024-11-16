using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f;
    public AnimationCurve fadeCurve;

    private void Awake()
    {
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 1);
        }
    }

    public void FadeIn()
    {
        if (fadeImage != null)
        {
            StartCoroutine(Fade(1, 0));
        }
    }

    public void FadeOut()
    {
        if (fadeImage != null)
        {
            StartCoroutine(Fade(0, 1));
        }
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float curveValue = fadeCurve.Evaluate(t);
            color.a = Mathf.Lerp(startAlpha, targetAlpha, curveValue);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }
}
