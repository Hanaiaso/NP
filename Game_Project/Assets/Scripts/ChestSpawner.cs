using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] chests;          // Các prefab rương
    [SerializeField] private Transform[] spawnPoints;      // Các vị trí spawn
    [SerializeField] private float timeBetweenSpawns = 2f; // Thời gian giữa mỗi lần spawn
    [SerializeField] private int maxChestCount = 20;       // Giới hạn tối đa số rương

    private int currentChestCount = 0;                     // Đếm số rương đã spawn
    private bool canSpawn = true;

    void Start()
    {
        StartCoroutine(SpawnChestCoroutine());
    }

    private IEnumerator SpawnChestCoroutine()
    {
        while (canSpawn)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);

            if (currentChestCount >= maxChestCount)
            {
                canSpawn = false;
                Debug.Log("Đã đạt giới hạn 20 rương, dừng spawn!");
                yield break;
            }

            GameObject chest = chests[Random.Range(0, chests.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(chest, spawnPoint.position, Quaternion.identity);
            currentChestCount++;

            Debug.Log($"Spawn rương thứ {currentChestCount}/{maxChestCount}");
        }
    }

    public void StopSpawning()
    {
        canSpawn = false;
        Debug.Log("Chest spawner đã dừng thủ công!");
    }
}
