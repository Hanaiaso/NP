using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;
    [SerializeField] private TextMeshProUGUI notificationText;

    private void Awake()
    {
        Instance = this;
        notificationText.gameObject.SetActive(false);
    }

    public void ShowNotification(string message, float duration = 2f)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageRoutine(message, duration));
    }

    private IEnumerator ShowMessageRoutine(string message, float duration)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        notificationText.gameObject.SetActive(false);
    }
}
