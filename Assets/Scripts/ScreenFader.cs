using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float defaultDuration = 1f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    public Coroutine FadeTo(float targetAlpha, float duration = -1f)
    {
        if (duration < 0f) duration = defaultDuration;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha, duration));
        return fadeCoroutine;
    }

    public Coroutine FadeIn(float duration = -1f) => FadeTo(0f, duration);
    public Coroutine FadeOut(float duration = -1f) => FadeTo(1f, duration);

    private IEnumerator FadeRoutine(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        canvasGroup.blocksRaycasts = targetAlpha > 0.5f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        fadeCoroutine = null;
    }
}