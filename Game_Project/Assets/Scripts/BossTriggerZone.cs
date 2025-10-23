﻿using UnityEngine;

public class BossTriggerZone : MonoBehaviour
{
    public GameObject bossPrefab;   // Prefab của boss
    public Transform spawnPoint;    // Vị trí sinh boss
    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasSpawned && collision.CompareTag("Player"))
        {
            Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
            hasSpawned = true;
        }
    }
}
