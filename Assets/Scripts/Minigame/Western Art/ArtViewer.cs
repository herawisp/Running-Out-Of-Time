using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//=============================================================================================================================//
//=============================================================================================================================//

public class ArtViewer : MonoBehaviour
{
    [Header("UI References")]
    

    [SerializeField] private Image artImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Navigation Buttons")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button continueButton;

    [Header("Art Database")]
    [SerializeField] private List<ArtData> artList = new();

    private int currentIndex = 0;

    //=========================================================================================================================//
    
    private void Awake()
    {
        if (prevButton != null) prevButton.onClick.AddListener(ShowPrevious);
        if (nextButton != null) nextButton.onClick.AddListener(ShowNext);
        if (continueButton != null) continueButton.onClick.AddListener(OnContinue);
    }

    private void Start()
    {
        UpdateUI();
    }

    //=========================================================================================================================//

    private void UpdateUI()
    {
        if (artList.Count == 0 || currentIndex < 0 || currentIndex >= artList.Count) return;

        ArtData currentArt = artList[currentIndex];

        if (titleText != null) 
            titleText.text = currentArt.title;

        if (descriptionText != null) 
            descriptionText.text = currentArt.description;

        if (artImage != null && currentArt.artSprite != null) 
            artImage.sprite = currentArt.artSprite;
    }

    public void ShowNext()
    {
        if (artList.Count == 0) return;

        currentIndex = (currentIndex + 1) % artList.Count;
        UpdateUI();
    }

    public void ShowPrevious()
    {
        if (artList.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = artList.Count - 1;
        }
        UpdateUI();
    }

    private void OnContinue()
    {
        
    }

    private void OnDestroy()
    {
        if (prevButton != null) prevButton.onClick.RemoveListener(ShowPrevious);
        if (nextButton != null) nextButton.onClick.RemoveListener(ShowNext);
        if (continueButton != null) continueButton.onClick.RemoveListener(OnContinue);
    }
    
    private void NextGame()
    {
    }

    //=========================================================================================================================//
}

//=============================================================================================================================//
//=============================================================================================================================//