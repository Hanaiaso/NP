using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class AnnouncementManager : MonoBehaviour
{
    public static AnnouncementManager Instance;

    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip announcementSound;
    [Range(0f, 1f)] public float soundVolume = 0.8f;

    private Queue<string> messageQueue = new Queue<string>();
    private Coroutine showRoutine;
    private bool isShowing = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (messageText == null)
            Debug.LogWarning("⚠️ AnnouncementManager: messageText chưa được gán trong Inspector.");
        else
            messageText.alpha = 0f; // ẩn text lúc đầu
    }

    public void ShowMessage(string msg)
    {
        if (string.IsNullOrWhiteSpace(msg)) return;

        messageQueue.Enqueue(msg);

        if (!isShowing)
        {
            showRoutine = StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        isShowing = true;

        while (messageQueue.Count > 0)
        {
            string msg = messageQueue.Dequeue();
            yield return StartCoroutine(ShowRoutine(msg));
        }

        isShowing = false;
        showRoutine = null;
    }

    private IEnumerator ShowRoutine(string msg)
    {
        messageText.text = msg;
        messageText.alpha = 0f;
        Vector3 startPos = messageText.rectTransform.anchoredPosition - new Vector2(0, 100f);
        Vector3 targetPos = messageText.rectTransform.anchoredPosition;

        // --- Fade in + trượt lên ---
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = t / fadeDuration;
            messageText.alpha = progress;
            messageText.rectTransform.anchoredPosition = Vector3.Lerp(startPos, targetPos, progress);
            messageText.rectTransform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, progress);
            yield return null;
        }

        messageText.alpha = 1;
        messageText.rectTransform.anchoredPosition = targetPos;

        // --- Giữ hiển thị ---
        yield return new WaitForSeconds(displayDuration);

        // --- Fade out + trượt lên ---
        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float progress = t / fadeDuration;
            messageText.alpha = 1 - progress;
            messageText.rectTransform.anchoredPosition = targetPos + new Vector3(0, 50f * progress, 0);
            yield return null;
        }

        messageText.alpha = 0;
    }
}
