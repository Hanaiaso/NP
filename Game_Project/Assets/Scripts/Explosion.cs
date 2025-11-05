using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 40f;
    private AudioManager audioManager; 

    void Awake()
    {
        
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        Enemy enemy = collision.GetComponent<Enemy>();

        if (collision.CompareTag("Player"))
        {
            player.TakeDamege(damage);
        }
        if (collision.CompareTag("Enemy"))
        {
            enemy.TakeDamege(damage);
        }
    }
    public void DestroyExplosion()
    {
        if (audioManager != null)
        {
            audioManager.EnemyExplosion(); 
        }
        else
        {
            Debug.LogWarning("Không tìm thấy AudioManager trong Scene!");
        }
        Destroy(gameObject);
    }
}
