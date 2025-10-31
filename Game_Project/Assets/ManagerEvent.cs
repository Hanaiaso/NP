using UnityEngine;
using System.Collections;

public class ManagerEvent : MonoBehaviour
{
    public NightfallEvent nightEventPrefab;
    public HeatZoneEvent heatZoneEventPrefab;
    public SandstormEvent sandstormEventPrefab;

    public float eventInterval = 180f; // mỗi 3 phút

    private bool isRunning = false;

    void Start()
    {
        Debug.Log("🚀 Bắt đầu hệ thống sự kiện ngẫu nhiên...");
        StartCoroutine(RandomEventLoop());
    }

    private IEnumerator RandomEventLoop()
    {
        isRunning = true;

        while (isRunning)
        {
            // ⏳ Chờ giữa các lần (random nhẹ)
            yield return new WaitForSeconds(Random.Range(eventInterval - 30f, eventInterval + 30f));

            int randomEvent = Random.Range(0, 3);

            switch (randomEvent)
            {
                case 0: // 🌑 Nightfall
                    if (nightEventPrefab != null)
                    {
                        Debug.Log("🌑 Nightfall xuất hiện!");
                        NightfallEvent newNight = Instantiate(nightEventPrefab);
                        newNight.StartEvent();
                    }
                    else
                    {
                        Debug.LogWarning("❌ NightfallEvent Prefab chưa gán!");
                    }
                    break;

                case 1: // 🔥 Heat Zone
                    if (heatZoneEventPrefab != null)
                    {
                        Debug.Log("🔥 Heat Zone xuất hiện!");
                        HeatZoneEvent newZone = Instantiate(heatZoneEventPrefab);
                        StartCoroutine(newZone.StartHeatZone());
                    }
                    else
                    {
                        Debug.LogWarning("❌ HeatZoneEvent Prefab chưa gán!");
                    }
                    break;

                case 2: // 🏜 Sandstorm
                    if (sandstormEventPrefab != null)
                    {
                        Debug.Log("🏜 Bão cát xuất hiện!");
                        SandstormEvent newStorm = Instantiate(sandstormEventPrefab);
                        StartCoroutine(newStorm.StartSandstorm());
                    }
                    else
                    {
                        Debug.LogWarning("❌ SandstormEvent Prefab chưa gán!");
                    }
                    break;
            }
        }
    }
}
