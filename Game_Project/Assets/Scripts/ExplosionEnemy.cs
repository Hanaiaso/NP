using UnityEngine;

public class ExplosionEnemy : Enemy
{
    [SerializeField] private GameObject explosionPrefab; // Hiệu ứng vụ nổ

    private void CreateExplosion()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamege(enterDamage);
            }
            CreateExplosion(); // Nổ khi chạm vào Player
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamege(stayDamage);
            }
        }
    }

    protected override void Die()
    {
        CreateExplosion(); // Nổ khi enemy chết
        base.Die();
    }
}
