using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage = 40f;

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
        Destroy(gameObject);
    }
}
