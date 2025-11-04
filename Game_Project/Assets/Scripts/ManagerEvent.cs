using UnityEngine;
using System.Collections;

public class ManagerEvent : MonoBehaviour
{
    public NightfallEvent nightEventPrefab;
    public HeatZoneEvent heatZoneEventPrefab;
    public SandstormEvent sandstormEventPrefab;

    public float eventInterval = 180f; // mỗi 3 phút
    private bool isRunning = false;

    private Coroutine eventLoopCoroutine;

    void Start()
    {
        Debug.Log("Bắt đầu hệ thống sự kiện ngẫu nhiên...");
        eventLoopCoroutine = StartCoroutine(RandomEventLoop());
    }

    private IEnumerator RandomEventLoop()
    {
        isRunning = true;

        while (isRunning)
        {
            // ⏳ Chờ giữa các lần (random nhẹ)
            yield return new WaitForSeconds(Random.Range(eventInterval - 30f, eventInterval + 30f));

            if (!isRunning) yield break; // Nếu đã dừng, thoát ngay

            int randomEvent = Random.Range(0, 3);

            switch (randomEvent)
            {
                case 0: // 🌑 Nightfall
                    if (nightEventPrefab != null)
                    {
                        AnnouncementManager.Instance?.ShowMessage(" Warning: Deadly darkness covers the land!");
                        NightfallEvent newNight = Instantiate(nightEventPrefab);
                        newNight.StartEvent();
                    }
                    else
                    {
                        Debug.LogWarning("NightfallEvent Prefab chưa gán!");
                    }
                    break;

                case 1: // 🔥 Heat Zone
                    if (heatZoneEventPrefab != null)
                    {
                        AnnouncementManager.Instance?.ShowMessage(" Warning: Unusual heat detected in the area!");
                        HeatZoneEvent newZone = Instantiate(heatZoneEventPrefab);
                        StartCoroutine(newZone.StartHeatZone());
                    }
                    else
                    {
                        Debug.LogWarning(" HeatZoneEvent Prefab chưa gán!");
                    }
                    break;

                case 2: // 🏜 Sandstorm
                    if (sandstormEventPrefab != null)
                    {
                        AnnouncementManager.Instance?.ShowMessage(" Warning: A massive sandstorm is approaching!");
                        SandstormEvent newStorm = Instantiate(sandstormEventPrefab);
                        StartCoroutine(newStorm.StartSandstorm());
                    }
                    else
                    {
                        Debug.LogWarning(" SandstormEvent Prefab chưa gán!");
                    }
                    break;
            }
        }
    }

    // 🛑 Hàm dừng toàn bộ sự kiện khi người chơi chết
    public void StopAllEvents()
    {
        Debug.Log(" Dừng hệ thống sự kiện vì người chơi đã chết!");
        isRunning = false;

        if (eventLoopCoroutine != null)
            StopCoroutine(eventLoopCoroutine);

        // Dừng tất cả coroutine đang chạy trong các event (nếu có)
        StopAllCoroutines();

        // (Tùy chọn) Hủy các event hiện có trên scene
        foreach (NightfallEvent night in FindObjectsOfType<NightfallEvent>())
            Destroy(night.gameObject);
        foreach (HeatZoneEvent heat in FindObjectsOfType<HeatZoneEvent>())
            Destroy(heat.gameObject);
        foreach (SandstormEvent storm in FindObjectsOfType<SandstormEvent>())
            Destroy(storm.gameObject);
    }
}
