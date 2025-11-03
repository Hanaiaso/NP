using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy_Khanh : Enemy
{
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedDanThuong = 20f;
    [SerializeField] private float speedDanVongTron = 10f;
    [SerializeField] private float hpValue = 100f;
    [SerializeField] private float skillCooldown = 2f;
    private float nextSkillTime = 0f;

    [Header("Rơi đồ khi chết")]
    public GameObject[] ItemPrefabs;   // Danh sách vật phẩm rơi
    public Transform dropPoint;        // Nơi spawn đồ (nếu null thì dùng transform boss)


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Time.time >= nextSkillTime)
        {
            SuDungSkill();
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
    private void BanDanThuong()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - firePoint.position;
            directionToPlayer.Normalize();
            GameObject bullet = Instantiate(bulletPrefabs, firePoint.position, Quaternion.identity);
            EnemyBullet_Khanh enemyBullet = bullet.GetComponent<EnemyBullet_Khanh>();
            enemyBullet.SetMovementDirection(directionToPlayer * speedDanThuong);
        }
    }
    private void BanDanVongTron()
    {
        const int bulletCount = 12;
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector3 bulletDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            GameObject bullet = Instantiate(bulletPrefabs, transform.position, Quaternion.identity);
            EnemyBullet_Khanh enemyBullet = bullet.AddComponent<EnemyBullet_Khanh>();
            enemyBullet.SetMovementDirection(bulletDirection * speedDanVongTron);
        }
    }
    private void HoiMau(float hpAmount)
    {
        currentHp = Mathf.Min(currentHp + hpAmount, maxHp);
        UpdateHpBar();
    }
    private void ChonSkillNgauNhien()
    {
        int randomSkill = Random.Range(0, 3);
        switch (randomSkill)
        {
            case 0:
                BanDanThuong();
                break;
            case 1:
                BanDanVongTron();
                break;
            case 2:
                HoiMau(hpValue);
                break;
        }
    }
    private void SuDungSkill()
    {
        nextSkillTime = Time.time + skillCooldown;
        ChonSkillNgauNhien();
    }
    protected override void Die()
    {
        base.Die();

        // 💥 Rơi tất cả vật phẩm
        DropAllItems();

        // 🔥 Có thể thêm hiệu ứng nổ hoặc animation chết
        Destroy(gameObject, 1.5f); // Xóa boss sau 1.5s
    }

    private void DropAllItems()
    {
        if (ItemPrefabs == null || ItemPrefabs.Length == 0) return;

        Transform spawnRoot = dropPoint != null ? dropPoint : transform;

        foreach (GameObject prefab in ItemPrefabs)
        {
            if (prefab == null) continue;

            // Tạo vị trí ngẫu nhiên quanh boss
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.5f, 1f), 0f);
            Vector3 spawnPos = spawnRoot.position + randomOffset;

            // Spawn từng vật phẩm
            GameObject item = Instantiate(prefab, spawnPos, Quaternion.identity);

            // Cho lực ngẫu nhiên bay ra để tản đều
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 randomForce = new Vector2(Random.Range(-2f, 2f), Random.Range(3f, 5f));
                rb.AddForce(randomForce, ForceMode2D.Impulse);
            }
        }
    }
}
