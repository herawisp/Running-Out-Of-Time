using TMPro;
using UnityEngine;

public class WesternArt : MonoBehaviour
{
    [Header("Minigame Stats")]
    [SerializeField] private int questionNumber = 0;
    [SerializeField] private int correctNumber = 0;

    [Header("UI")]
    public TextMeshProUGUI quizFinishedText;
    public TextMeshProUGUI informationText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI titleText;
    public GameObject informationPanel;
    
    public TextMeshProUGUI artTitleText;
    public TextMeshProUGUI artDescriptionText;
    public GameObject artImage;
}
