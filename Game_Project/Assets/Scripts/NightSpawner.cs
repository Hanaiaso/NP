using UnityEngine;
using System.Collections;

public class NightSpawner : MonoBehaviour
{
    [Header("Cấu hình quái ban đêm")]
    [SerializeField] private GameObject[] nightEnemies;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnRadius = 10f;     // 🔥 Khoảng cách spawn quanh người chơi
    [SerializeField] private float minDistance = 5f;     // 🔥 Không spawn quá gần player

    private bool isSpawning = false;
    private Coroutine spawnRoutine;
    private Transform player; // Tham chiếu tới người chơi

    private void Start()
    {
        // Tìm Player trong scene (gắn tag "Player")
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogWarning("⚠️ [NightSpawner] Không tìm thấy Player!");
    }

    public void StartNightSpawn()
    {
        if (isSpawning) return;
        if (player == null) return;

        Debug.Log("🌑 [NightSpawner] Bắt đầu spawn quanh người chơi!");
        isSpawning = true;
        spawnRoutine = StartCoroutine(SpawnEnemies());
    }

    public void StopNightSpawn()
    {
        if (!isSpawning) return;

        Debug.Log("🌅 [NightSpawner] Dừng spawn quanh người chơi!");
        isSpawning = false;
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
    }

    private IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (player == null) yield break;

            // ✅ Tạo vị trí spawn ngẫu nhiên quanh player
            Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(minDistance, spawnRadius);
            Vector2 spawnPos = (Vector2)player.position + offset;

            // Chọn enemy ngẫu nhiên
            GameObject enemyPrefab = nightEnemies[Random.Range(0, nightEnemies.Length)];
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}
