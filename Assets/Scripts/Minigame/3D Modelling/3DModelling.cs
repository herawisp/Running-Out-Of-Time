using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//=============================================================================================================================//
//=============================================================================================================================//

[RequireComponent(typeof(Timer))]
public class ModellingGame : MonoBehaviour, IPointerClickHandler
{
    [Header("Minigame Stats")]
    [SerializeField] private int score = 0;
    [SerializeField] private int level = 0;
    [SerializeField] private int objectFinished = 0;

    [Header("Settings")]
    [SerializeField] private int objectNeeded = 3;
    [SerializeField] private int scoreThreshold = 5;
    [SerializeField] private SpriteGrid spriteGrid = new();
    
    [SerializeField] private float shakeDuration = 0.15f;
    [SerializeField] private float shakeMagnitude = 10f;

    [SerializeField] private float flashDuration = 0.35f;
    [SerializeField] private Color greenFlashColor = new(0.2f, 1f, 0.2f, 0.45f);
    [SerializeField] private Color redFlashColor = new(1f, 0.2f, 0.2f, 0.45f);

    [Header("UI")]
    public GameObject thisGame;
    public GameObject nextGame;

    public GameObject objectImage;
    public Timer timer;
    public TextMeshProUGUI objectFinishedText;
    public TextMeshProUGUI informationText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI titleText;
    public GameObject informationPanel;
    public GameObject darkenPanel;
    public GameObject flashPanel;

    private List<Sprite> objectSprites;
    private RectTransform objectRectTransform;
    private Vector3 originalImagePosition;
    private Coroutine shakeCoroutine;
    private Coroutine flashCoroutine;
    private Image flashImage;

    //=========================================================================================================================//
    
    private void Awake()
    {
        if (timer == null)
            timer = GetComponent<Timer>();

        if (objectImage != null)
        {
            objectRectTransform = objectImage.GetComponent<RectTransform>();
            if (objectRectTransform != null)
            {
                originalImagePosition = objectRectTransform.anchoredPosition;
            }
        }

        if (flashPanel != null)
        {
            flashImage = flashPanel.GetComponent<Image>();
            flashPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (timerText != null && timer != null)
        {
            timerText.text = timer.TimeLeft.ToString("F0");
        }
    }

    //=========================================================================================================================//
    
    public void StartGame()
    {
        score = 0;
        level = 0;
        objectFinished = 0;

        if (timer != null)
        {
            timer.OnTimeout -= HandleTimeout;
            timer.OnTimeout += HandleTimeout;
            timer.StartTimer();
        }   
        if (informationPanel != null)
        {
            informationPanel.SetActive(false);
        }
        if (darkenPanel != null)
        {
            darkenPanel.SetActive(false);
        }
        if (flashPanel != null)
        {
            flashPanel.SetActive(false);
        }

        LoadNextObject();
    }

    public void IncreaseClick()
    {
        score++;

        TriggerImageShake();

        if (score >= scoreThreshold)
        {
            score = 0;
            level++;
            UpdateObjectImage();
        }
    }

    //=========================================================================================================================//
    
    private void TriggerImageShake()
    {
        if (objectRectTransform == null) return;

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            objectRectTransform.anchoredPosition = originalImagePosition;
        }

        shakeCoroutine = StartCoroutine(ShakeImageRoutine());
    }

    private IEnumerator ShakeImageRoutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * shakeMagnitude;
            objectRectTransform.anchoredPosition = (Vector2)originalImagePosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        objectRectTransform.anchoredPosition = originalImagePosition;
        shakeCoroutine = null;
    }

    private void TriggerFlash(Color targetColor)
    {
        if (flashPanel == null || flashImage == null) return;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRoutine(targetColor));
    }

    private IEnumerator FlashRoutine(Color targetColor)
    {
        flashPanel.SetActive(true);
        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alphaProgress = 1f - Mathf.Clamp01(elapsed / flashDuration);
            flashImage.color = new Color(targetColor.r, targetColor.g, targetColor.b, targetColor.a * alphaProgress);
            yield return null;
        }

        flashPanel.SetActive(false);
        flashCoroutine = null;
    }

    //=========================================================================================================================//
    
    private void UpdateObjectImage()
    {
        if (objectSprites == null || objectImage == null) return;

        if (level < objectSprites.Count)
        {
            objectImage.GetComponent<Image>().sprite = objectSprites[level];
        }
        else
        {
            OnObjectCompleted();
        }
    }

    private void OnObjectCompleted()
    {
        objectFinished++;
        if (objectFinishedText != null)
        {
            objectFinishedText.text = $"{objectFinished}/{objectNeeded}";
        }
        Debug.Log($"Object Finished! ({objectFinished}/{objectNeeded})");

        // Flash green on completing an object
        TriggerFlash(greenFlashColor);

        if (objectFinished >= objectNeeded)
        {
            OnGameWon();
        }
        else
        {
            LoadNextObject();
        }
    }

    private void LoadNextObject()
    {
        level = 0;
        score = 0;
        objectSprites = spriteGrid.GetRandomRow();
        if (titleText != null)
        {
            titleText.text = "Make the 3D Model!";
        }

        if (objectSprites != null && objectSprites.Count > 0 && objectImage != null)
        {
            objectImage.GetComponent<Image>().sprite = objectSprites[level];
        }
        else
        {
            Debug.LogWarning("Failed to load sprites for the next object.");
        }
    }

    private void OnGameWon()
    {
        informationText.text = "Good Job!";
        if (informationPanel != null)
        {
            informationPanel.SetActive(true);
        }

        if (darkenPanel != null)
        {
            darkenPanel.SetActive(true);
        }

        if (timer != null)
        {
            timer.StopTimer();
        }
    }
    
    private void NextGame()
    {
        thisGame.SetActive(false);
        nextGame.SetActive(true);
    }

    private void HandleTimeout()
    {
        Debug.Log("Game Over! Time ran out.");

        // Flash red on timeout
        TriggerFlash(redFlashColor);

        if (informationText != null)
        {
            informationText.text = "Time's Up!";
        }

        if (informationPanel != null)
        {
            informationPanel.SetActive(true);
        }

        if (darkenPanel != null)
        {
            darkenPanel.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (timer != null)
        {
            timer.OnTimeout -= HandleTimeout;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            IncreaseClick();
        }
    }
}