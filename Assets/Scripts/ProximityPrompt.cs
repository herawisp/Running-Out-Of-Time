using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class ProximityPrompt : MonoBehaviour
{
    [Header("Target Scene")]
    [SerializeField] private string targetSceneName = "GameScene";

    [Header("UI Prompt Reference")]
    [Tooltip("Drag your 'E Key' UI Image or World-Space Canvas element here.")]
    [SerializeField] private GameObject promptUI;

    [Header("Player Settings")]
    [Tooltip("Tag assigned to your Player object.")]
    [SerializeField] private string playerTag = "Player";

    private bool isPlayerInRange = false;

    private void Awake()
    {
        // Ensure the collider is set to trigger
        GetComponent<Collider>().isTrigger = true;

        // Hide the prompt UI by default
        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isPlayerInRange) return;

        // Detect 'E' key press using the New Input System
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    private void Interact()
    {
        Debug.Log($"Interacted with {gameObject.name}. Loading scene: {targetSceneName}");
        
        // Hide UI immediately on interact
        if (promptUI != null)
        {
            promptUI.SetActive(false);
        }

        // Load the specified scene
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning("Target scene name is empty on " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = true;
            if (promptUI != null)
            {
                promptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = false;
            if (promptUI != null)
            {
                promptUI.SetActive(false);
            }
        }
    }

    // Optional: 2D Physics Support (automatically works if your project uses 2D colliders)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = true;
            if (promptUI != null)
            {
                promptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = false;
            if (promptUI != null)
            {
                promptUI.SetActive(false);
            }
        }
    }
}