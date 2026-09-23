using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToDoListManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Nama scene tujuan yang ingin dimuat.")]
    [SerializeField] private string targetSceneName;

    [Tooltip("Delay waktu sebelum scene berpindah setelah item terakhir diklik.")]
    [SerializeField] private float transitionDelay = 0.5f;

    [Header("Items")]
    [SerializeField] private List<ToDoItemTweener> items = new List<ToDoItemTweener>();

    private int completedCount = 0;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (items.Count == 0)
        {
            items.AddRange(GetComponentsInChildren<ToDoItemTweener>());
        }
    }

    public void OnItemCompleted()
    {
        if (isTransitioning) return;

        completedCount++;

        if (completedCount >= items.Count)
        {
            isTransitioning = true;
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(transitionDelay);

        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning("Target Scene Name belum diisi pada ToDoListManager!");
        }
    }
}