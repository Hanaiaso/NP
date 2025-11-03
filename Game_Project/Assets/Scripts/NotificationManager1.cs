using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationManager1 : MonoBehaviour
{
    [Header("Notification Settings")]
    [SerializeField] private Transform notificationParent; // Nơi chứa các thông báo (UI vertical layout)
    [SerializeField] private GameObject notificationPrefab; // Prefab TMP
    [SerializeField] private float displayDuration = 3f; // Thời gian hiển thị
    [SerializeField] private int maxNotifications = 5; // Tối đa 5 thông báo

    private Queue<GameObject> activeNotifications = new Queue<GameObject>();

    /// <summary>
    /// Hiển thị một thông báo mới
    /// </summary>
    public void ShowNotification(string message)
    {
        if (notificationPrefab == null || notificationParent == null)
        {
            Debug.LogWarning("⚠ NotificationManager: Prefab hoặc Parent chưa được gán trong Inspector!");
            return;
        }

        // Nếu đã đủ 5 thông báo → xóa cái cũ nhất
        if (activeNotifications.Count >= maxNotifications)
        {
            GameObject old = activeNotifications.Dequeue();
            Destroy(old);
        }

        // Tạo thông báo mới
        GameObject newNotification = Instantiate(notificationPrefab, notificationParent);
        TextMeshProUGUI text = newNotification.GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = message;
        }

        activeNotifications.Enqueue(newNotification);

        // Hiệu ứng mờ dần
        StartCoroutine(FadeOutAndDestroy(newNotification));
    }

    private IEnumerator FadeOutAndDestroy(GameObject notification)
    {
        CanvasGroup canvasGroup = notification.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = notification.AddComponent<CanvasGroup>();
        }

        // Fade in
        canvasGroup.alpha = 0;
        float fadeInTime = 0.3f;
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeInTime);
            yield return null;
        }
        canvasGroup.alpha = 1;

        // Giữ trong vài giây
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        float fadeOutTime = 0.5f;
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeOutTime);
            yield return null;
        }

        // Xóa thông báo
        if (activeNotifications.Count > 0)
            activeNotifications.Dequeue();

        Destroy(notification);
    }
}
