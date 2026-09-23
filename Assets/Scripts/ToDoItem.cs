using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ToDoItemTweener : MonoBehaviour, IPointerClickHandler
{
    [Header("Target Anchored Position")]
    [Tooltip("The X and Y coordinates to move to when clicked.")]
    [SerializeField] private Vector2 targetAnchoredPosition = new Vector2(-500f, 0f);

    [Header("Animation Settings")]
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Coroutine tweenCoroutine;
    public bool isAtTarget = false;

    private ToDoListManager manager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        manager = GetComponentInParent<ToDoListManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Cegah klik berulang pada item yang sama
        if (isAtTarget) return;

        isAtTarget = true;
        Vector2 destination = targetAnchoredPosition;
        MoveToPosition(destination);

        // Beritahu manager bahwa item ini selesai
        if (manager != null)
        {
            manager.OnItemCompleted();
        }
    }

    public void MoveToPosition(Vector2 destination)
    {
        if (tweenCoroutine != null)
        {
            StopCoroutine(tweenCoroutine);
        }

        tweenCoroutine = StartCoroutine(TweenRoutine(destination));
    }

    private IEnumerator TweenRoutine(Vector2 destination)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float curveT = easeCurve.Evaluate(t);

            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, destination, curveT);
            yield return null;
        }

        rectTransform.anchoredPosition = destination;
        tweenCoroutine = null;
    }
}