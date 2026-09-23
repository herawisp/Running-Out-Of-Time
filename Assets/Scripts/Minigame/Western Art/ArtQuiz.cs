using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtQuizManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private GameObject quizPanel;

    [Header("Quiz Questions")]
    [SerializeField] private List<QuizQuestion> questionList = new();
    [SerializeField] private List<Sprite> allArtPool = new();

    [Header("Timer")]
    [SerializeField] private Timer timer;
    [SerializeField] private TextMeshProUGUI timerText;

    private int currentQuestionIndex = 0;
    private int score = 0;
    private int currentCorrectSlotIndex = 0;
    private bool isGameActive = false;

    private void Awake()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    private void Start()
    {
        // Jangan langsung mulai di Start(). Biarkan menunggu StartGame() dipanggil.
        if (quizPanel != null)
        {
            quizPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (timerText != null && timer != null)
        {
            timerText.text = timer.TimeLeft.ToString("F0");
        }
    }

    public void StartGame()
    {
        score = 0;
        currentQuestionIndex = 0;
        isGameActive = true;

        if (quizPanel != null)
        {
            quizPanel.SetActive(true);
        }

        if (timer != null)
        {
            timer.StartTimer();
        }

        LoadQuestion(currentQuestionIndex);
    }

    private void LoadQuestion(int index)
    {
        if (questionList == null || index >= questionList.Count)
        {
            OnQuizFinished();
            return;
        }

        QuizQuestion currentQ = questionList[index];

        if (questionText != null)
        {
            questionText.text = currentQ.question;
        }

        // 1. Ambil 2 pengecoh acak yang tidak sama dengan jawaban benar
        List<Sprite> choices = GetChoices(currentQ.correctSprite);

        // 2. Acak posisi 3 opsi (shuffle)
        ShuffleList(choices);

        // 3. Catat di slot mana jawaban benar berada
        currentCorrectSlotIndex = choices.IndexOf(currentQ.correctSprite);

        // 4. Pasang sprite ke button
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < choices.Count)
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

    private List<Sprite> GetChoices(Sprite correct)
    {
        List<Sprite> choices = new List<Sprite> { correct };

        // Buat list kandidat pengecoh (semua pool kecuali jawaban benar)
        List<Sprite> availableDecoys = new List<Sprite>(allArtPool);
        availableDecoys.Remove(correct);

        // Ambil 2 decoy secara acak
        int decoysNeeded = optionButtons.Length - 1; // 3 - 1 = 2
        for (int i = 0; i < decoysNeeded && availableDecoys.Count > 0; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableDecoys.Count);
            choices.Add(availableDecoys[randomIndex]);
            availableDecoys.RemoveAt(randomIndex); // Cegah opsi kembar
        }

        return choices;
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

    private void OnOptionSelected(int selectedIndex)
    {
        if (selectedIndex == currentCorrectSlotIndex)
        {
            Debug.Log("<color=green>Jawaban Benar!</color>");
            score++;
        }
        else
        {
            Debug.Log("<color=red>Jawaban Salah!</color>");
        }

        currentQuestionIndex++;
        LoadQuestion(currentQuestionIndex);
    }

    private void OnQuizFinished()
    {
        isGameActive = false;
        Debug.Log($"Quiz Selesai! Skor: {score}/{questionList.Count}");

        if (timer != null)
        {
            timer.StopTimer();
        }
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
    }
}