using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image avatarImage;
    [SerializeField] private GameObject dialogueBox;

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private Queue<DialogueSentence> sentencesQueue = new Queue<DialogueSentence>();
    private bool isTyping = false;
    private string currentSentence = "";
    private Coroutine typingCoroutine;

    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
        if (dialogueBox != null)
            dialogueBox.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        IsDialogueActive = true;
        if (dialogueBox != null)
            dialogueBox.SetActive(true);

        sentencesQueue.Clear();

        foreach (DialogueSentence sentence in dialogue.sentences)
        {
            sentencesQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentSentence;
            isTyping = false;
            return;
        }

        if (sentencesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueSentence current = sentencesQueue.Dequeue();


        if (avatarImage != null)
        {
            if (current.speakerAvatar != null)
            {
                avatarImage.sprite = current.speakerAvatar;
                avatarImage.gameObject.SetActive(true);
            }
            else
            {
                avatarImage.gameObject.SetActive(false);
            }
        }

        // Jalankan efek mengetik
        currentSentence = current.sentence;
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isTyping = true;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void EndDialogue()
    {
        IsDialogueActive = false;
        if (dialogueBox != null)
            dialogueBox.SetActive(false);

        Debug.Log("Dialogue Ended");
    }
}