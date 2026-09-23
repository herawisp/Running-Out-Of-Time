using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using UnityEngine.UI;

public class VideoCutsceneController : MonoBehaviour
{
    public GameObject background;

    [Header("References")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private ScreenFader screenFader;

    [Header("Fade Settings")]
    [SerializeField] private float fadeOutDuration = 1.5f;
    [SerializeField] private float holdBlackDuration = 0.5f; // Pause at pitch black
    [SerializeField] private float fadeInDuration = 1.5f;
    
    [Header("Events")]
    [Tooltip("Fires while the screen is completely black (great for turning on UI/Dialogue).")]
    [SerializeField] private UnityEvent onPeakBlackout;

    [Tooltip("Fires after the screen has completely faded back in.")]
    [SerializeField] private UnityEvent onVideoFinishedAndFadedIn;

    private void OnEnable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(CutsceneSequenceRoutine());
    }

    private IEnumerator CutsceneSequenceRoutine()
    {
        // 1. Fade screen out to black
        yield return screenFader.FadeOut(fadeOutDuration);

        // 2. Stop video & disable the RawImage component instead of the whole GameObject
        videoPlayer.Stop();
        
        RawImage rawImg = videoPlayer.GetComponent<RawImage>();
        if (rawImg != null)
        {
            background.SetActive(false);
            rawImg.enabled = false;
        }

        // Trigger events while pitch black
        onPeakBlackout?.Invoke();

        if (holdBlackDuration > 0f)
        {
            yield return new WaitForSeconds(holdBlackDuration);
        }

        // 3. Fade screen back in
        yield return screenFader.FadeIn(fadeInDuration);

        // 4. Trigger post-fade events
        onVideoFinishedAndFadedIn?.Invoke();
    }
}