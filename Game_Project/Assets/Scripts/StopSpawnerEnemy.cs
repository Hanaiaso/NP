using UnityEngine;

public class StopSpawnerEnemy : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner; // Kéo object chứa EnemySpawner vào đây

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (enemySpawner != null)
            {
                enemySpawner.StopSpawning();
                Debug.Log("Player reached checkpoint → Stop spawning enemies");
            }
            else
            {
                Debug.LogWarning("EnemySpawner not assigned in StopSpawnerEnemy!");
            }
        }
    }
}
