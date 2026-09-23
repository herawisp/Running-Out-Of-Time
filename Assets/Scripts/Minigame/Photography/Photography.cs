using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//=============================================================================================================================//
//=============================================================================================================================//

public class PhotoMatchGame : MonoBehaviour
{
    [Header("UI References")]
    
    public GameObject thisGame;
    public GameObject nextGame;
    
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject flashPanel;
    [SerializeField] private GameObject darkenPanel;
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private Image centerTargetImage;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI informationText;
    [SerializeField] private TextMeshProUGUI titleText;

    [Header("Flash Feedback Settings")]
    [SerializeField] private float flashDuration = 0.35f;
    [SerializeField] private Color greenFlashColor = new(0.2f, 1f, 0.2f, 0.45f);
    [SerializeField] private Color redFlashColor = new(1f, 0.2f, 0.2f, 0.45f);

    [Header("Timer")]
    [SerializeField] private Timer timer;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Phase Settings")]
    [Range(1, 2)]
    [SerializeField] private int currentPhase = 1; // 1 = Q1-5, 2 = Q6-10
    [SerializeField] private int questionsPerPhase = 5;

    [Header("Game Levels (10 Total)")]
    [SerializeField] private List<PhotoMatchSet> roundList = new List<PhotoMatchSet>();

    private int currentRoundIndex = 0;
    private int phaseEndIndex = 0;
    private int score = 0;
    private int correctSlotIndex = 0;
    private bool isGameActive = false;

    private Coroutine flashCoroutine;
    private Image flashImage;

    public int CurrentPhase
    {
        get => currentPhase;
        set => currentPhase = Mathf.Clamp(value, 1, 2);
    }

    //=========================================================================================================================//

    private void Awake()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnOptionClicked(index));
        }

        if (flashPanel != null)
        {
            flashImage = flashPanel.GetComponent<Image>();
            flashPanel.SetActive(false);
        }
    }

    private void Start()
    {
        if (gamePanel != null)
        {
            gamePanel.SetActive(false);
        }
        informationPanel.SetActive(true);
        darkenPanel.SetActive(true);
    }

    private void Update()
    {
        if (isGameActive && timerText != null && timer != null)
        {
            timerText.text = timer.TimeLeft.ToString("F0");
        }
    }

    //=========================================================================================================================//
    
    public void StartGame(int phase)
    {
        informationPanel.SetActive(false);
        darkenPanel.SetActive(false);

        currentPhase = Mathf.Clamp(phase, 1, 2);
        score = 0;
        isGameActive = true;

        currentRoundIndex = (currentPhase - 1) * questionsPerPhase;
        phaseEndIndex = currentRoundIndex + questionsPerPhase;

        if (gamePanel != null)
        {
            gamePanel.SetActive(true);
        }

        if (flashPanel != null)
        {
            flashPanel.SetActive(false);
        }

        if (timer != null)
        {
            timer.OnTimeout -= HandleTimeout;
            timer.OnTimeout += HandleTimeout;
            timer.StartTimer();
        }

        LoadRound(currentRoundIndex);
    }

    private void LoadRound(int index)
    {
        if (roundList == null || index >= phaseEndIndex || index >= roundList.Count)
        {
            OnGameFinished();
            return;
        }

        PhotoMatchSet currentSet = roundList[index];

        if (centerTargetImage != null)
        {
            centerTargetImage.sprite = currentSet.correctPhoto;
        }

        if (progressText != null)
        {
            int relativeQuestionNumber = (index - (currentPhase - 1) * questionsPerPhase) + 1;
            progressText.text = $"{relativeQuestionNumber}/{questionsPerPhase}";
        }

        if (titleText != null)
        {
            titleText.text = "Match the Photo!";
        }

        List<Sprite> choices = new List<Sprite> { currentSet.correctPhoto };
        if (currentSet.decoyPhotos != null)
        {
            choices.AddRange(currentSet.decoyPhotos);
        }

        ShuffleList(choices);

        correctSlotIndex = choices.IndexOf(currentSet.correctPhoto);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < choices.Count && choices[i] != null)
            {
                Image btnImage = optionButtons[i].GetComponent<Image>();
                if (btnImage != null)
                {
                    btnImage.sprite = choices[i];
                }
                optionButtons[i].gameObject.SetActive(true);
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnOptionClicked(int selectedIndex)
    {
        if (!isGameActive) return;

        if (selectedIndex == correctSlotIndex)
        {
            Debug.Log("<color=green>Pilihan Tepat!</color>");
            score++;
            TriggerFlash(greenFlashColor);
        }
        else
        {
            Debug.Log("<color=red>Pilihan Salah!</color>");
            TriggerFlash(redFlashColor);
        }

        currentRoundIndex++;
        LoadRound(currentRoundIndex);
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

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void OnGameFinished()
    {
        isGameActive = false;

        gamePanel.SetActive(false);
        darkenPanel.SetActive(true);
        informationPanel.SetActive(true);
        informationText.text = $"Skor: {score}/{questionsPerPhase}";

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
        isGameActive = false;
        Debug.Log("Waktu habis!");
        TriggerFlash(redFlashColor);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (optionButtons[i] != null)
            {
                optionButtons[i].onClick.RemoveAllListeners();
            }
        }

        if (timer != null)
        {
            timer.OnTimeout -= HandleTimeout;
        }
    }
}