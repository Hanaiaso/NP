using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemy : Enemy
{
    [SerializeField] private GameObject explosionPrefabs; // tạo game object

    private void CreateExplosion()
    {
        if (explosionPrefabs != null) 
        {
        Instantiate(explosionPrefabs, transform.position, Quaternion.identity);  // instant một vụ nổ ở enemy
        }
    }

    protected override void Die()
    {
        CreateExplosion();
        base.Die();
    }
    private void OnTriggerEnter2D(Collider2D collision) // cha chạm với player để tạo vụ nổ
    {
        if (collision.CompareTag("Player")) 
        {
            CreateExplosion();
        }
    }

}
