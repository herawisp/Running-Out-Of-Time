using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private DialogueManager dialogueManager;

    public void TriggerDialogue()
    {
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogue);
        }
    }

    private void Update()
    {
        // Contoh: Tekan Space / Left Click untuk lanjut dialog jika sedang aktif
        if (dialogueManager != null && dialogueManager.IsDialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                dialogueManager.DisplayNextSentence();
            }
        }
    }

    void Start()
    {
        TriggerDialogue();
    }
}