using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NightfallEvent : MonoBehaviour
{
    public Image nightOverlay;
    public NightSpawner nightSpawner; // 🌑 Spawner riêng cho ban đêm

    public float fadeDuration = 3f;
    public float nightDuration = 20f;
    private bool isNight = false;

    public void StartEvent()
    {
        if (!isNight)
        {
            Debug.Log("🌑 [NightfallEvent] Bắt đầu sự kiện Nightfall...");
            StartCoroutine(NightRoutine());
        }
        else
        {
            Debug.Log("⚠️ [NightfallEvent] Sự kiện Nightfall đang diễn ra!");
        }
    }

    IEnumerator NightRoutine()
    {
        isNight = true;

        // Làm tối
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float a = Mathf.Lerp(0, 0.7f, t / fadeDuration);
            nightOverlay.color = new Color(0, 0, 0, a);
            yield return null;
        }

        Debug.Log("🌘 Màn đêm đã bao phủ!");
        nightSpawner.StartNightSpawn(); // 🔥 Bắt đầu spawn quái đêm

        yield return new WaitForSeconds(nightDuration);

        // Sáng lại
        Debug.Log("🌅 Bắt đầu bình minh...");
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float a = Mathf.Lerp(0.7f, 0, t / fadeDuration);
            nightOverlay.color = new Color(0, 0, 0, a);
            yield return null;
        }

        // Dừng quái ban đêm
        nightSpawner.StopNightSpawn();

        isNight = false;
        Debug.Log("☀️ Trời sáng trở lại!");
    }
}
