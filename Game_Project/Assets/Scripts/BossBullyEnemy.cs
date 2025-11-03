using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullyEnemy : Enemy
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioManager audioManager;

    [Header("Bắn đạn")]
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedDanThuong = 10f;
    [SerializeField] private float shootCooldown = 1.5f;
    private float lastShootTime = 0f;

    [Header("Hồi máu")]
    [SerializeField] private float hpValue = 100f;

    [Header("Đánh thường")]
    [SerializeField] private Transform meleePoint;
    [SerializeField] private float meleeRange = 40f;
    [SerializeField] private float meleeAttackRange = 0.5f;
    [SerializeField] private float meleeDamage = 2f;
    [SerializeField] private float meleeAttackAnimationTime = 0.5f;
    [SerializeField] private float meleeAttackDamageDelay = 0.1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private bool isMeleeAttacked = false;
    private float lastAttackTime = 0f;

    [SerializeField] private Animator meleeAnimator;

    [Header("Rơi đồ khi chết")]
    public GameObject[] ItemPrefabs;   // Danh sách vật phẩm rơi
    public Transform dropPoint;        // Nơi spawn đồ (nếu null thì dùng transform boss)

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Nếu boss ở quá xa player → teleport lại gần
        if (distanceToPlayer >= meleeRange * 10f)
        {
            DichChuyen();
            return;
        }

        if (Time.time < lastAttackTime + attackCooldown)
        {
            GaySatThuongDanhThuong();
            return;
        }

        isMeleeAttacked = false;
        if (distanceToPlayer <= meleeRange)
        {
            DanhThuong();
        }
        else
        {
            BanDanThuong();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            player.TakeDamege(enterDamage);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            player.TakeDamege(stayDamage);
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


    private void DanhThuong()
    {
        lastAttackTime = Time.time;

        StopCoroutine("ShootLoop");

        if (meleeAnimator != null)
            meleeAnimator.SetTrigger("Slash");

        Debug.Log("Boss bắt đầu chém!");
    }

    private void GaySatThuongDanhThuong()
    {
        if (isMeleeAttacked) return;
        if (Time.time >= lastAttackTime + meleeAttackAnimationTime) return;
        if (Time.time < lastAttackTime + meleeAttackDamageDelay) return;

        float distanceToPlayer = Vector2.Distance(meleePoint.position, player.transform.position);
        if (distanceToPlayer > meleeAttackRange) return;

        isMeleeAttacked = true;
        player.TakeDamege(meleeDamage);
    }

    private void BanDanThuong()
    {
        if (player == null) return;
        if (Time.time < lastShootTime + shootCooldown) return;

        lastShootTime = Time.time;

        Vector3 directionToPlayer = player.transform.position - firePoint.position;
        directionToPlayer.Normalize();

        GameObject bullet = Instantiate(bulletPrefabs, firePoint.position, Quaternion.identity);
        BossBullyBullet enemyBullet = bullet.AddComponent<BossBullyBullet>();
        enemyBullet.SetMovementDirection(directionToPlayer * speedDanThuong);
    }

    private void HoiMau(float hpAmount)
    {
        currentHp = Mathf.Min(currentHp + hpAmount, maxHp);
        UpdateHpBar();
    }

    private void DichChuyen()
    {
        if (player != null)
        {
            Vector2 direction = transform.position - player.transform.position;
            Vector3 direction3 = direction.normalized;
            transform.position = player.transform.position + direction3 * 3 * meleeAttackRange;
        }
    }
}
