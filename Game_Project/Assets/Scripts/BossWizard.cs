using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FirePattern
{
    Single,
    MultiAim,
    FanSpread,
    Circle
}

public class BossWizard : Enemy
{
    [SerializeField] private AudioManager audioManager;
    [Header("Animator & States")]
    private Animator anim;
    private bool isDeadLocal = false;

    [Header("Movement & Attack")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireCooldown = 2f;

    [Header("Bullet Pattern")]
    [SerializeField] private int bulletCount = 5;
    [SerializeField] private float spreadAngle = 45f;
    [SerializeField] private float[] bulletSpeeds;
    [Header("Teleport Skill")]
    [SerializeField] private float teleportDistance = 10f; // khoảng cách player quá xa thì boss teleport
    [SerializeField] private float teleportCooldown = 5f;  // thời gian chờ giữa các lần teleport
    [SerializeField] private float teleportOffset = 2f;    // dịch chuyển gần player, tránh dịch ngay lên player
    [Header("Rơi đồ khi chết")]
    public GameObject[] ItemPrefabs;   // Danh sách vật phẩm rơi
    public Transform dropPoint;        // Nơi spawn đồ (nếu null thì dùng transform boss)

    private float teleportTimer = 0f;
    private float fireTimer = 0f;

    protected override void Start()
    {
        base.Start();
        if (audioManager == null)
            audioManager = FindObjectOfType<AudioManager>();
    }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        if (bulletSpeeds == null || bulletSpeeds.Length < bulletCount)
        {
            bulletSpeeds = new float[bulletCount];
            for (int i = 0; i < bulletCount; i++) bulletSpeeds[i] = 9f;
        }
    }

    private void Update()
    {
        if (player == null || anim == null) return;

        if (isDeadLocal)
        {
            anim.SetFloat("speed", 0);
            anim.SetBool("isAttack", false);
            rb.velocity = Vector2.zero;
            return;
        }

        // --- TELEPORT CHECK ---
        teleportTimer += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance > teleportDistance && teleportTimer >= teleportCooldown)
        {
            TeleportNearPlayer();
            teleportTimer = 0f;
            return; // sau khi teleport không di chuyển tiếp frame này
        }

        // --- WALK / ATTACK ---
        if (distance > attackRange) // walk
        {
            MoveTowardsPlayer(walkSpeed);
            anim.SetFloat("speed", walkSpeed);
            anim.SetBool("isAttack", false);
        }
        else // attack
        {
            rb.velocity = Vector2.zero;
            anim.SetFloat("speed", 0);
            TryAttack();
        }
    }
    private void TeleportNearPlayer()
    {
        Vector3 playerPos = player.transform.position;

        // Chọn vị trí gần player nhưng cách 1 khoảng offset để không trùng player
        Vector3 teleportPos = playerPos + (Vector3)(Random.insideUnitCircle.normalized * teleportOffset);

        // Dịch chuyển boss ngay lập tức
        transform.position = teleportPos;

        // Flip hướng boss về player
        Vector2 direction = (playerPos - transform.position).normalized;
        if (direction.x != 0)
            transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);

        // optional: play effect teleport
        Debug.Log("Boss teleported!");
    }
    private void MoveTowardsPlayer(float speed)
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;

        if (direction.x != 0)
            transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
    }

    private void TryAttack()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireCooldown)
        {
            fireTimer = 0f;
            anim.SetBool("isAttack", true); // trigger animation
        }
        else
        {
            anim.SetBool("isAttack", false);
        }
    }

    // --- AnimationEvent gọi ---
    public void FireBulletsFromAnimation()
    {
        FireRandomPattern(); // bắn pattern ngẫu nhiên ngay frame này
    }

    // --- Random Pattern ---
    private void FireRandomPattern()
    {
        FirePattern[] patterns = new FirePattern[] { FirePattern.Single, FirePattern.MultiAim, FirePattern.FanSpread, FirePattern.Circle };
        FirePattern chosenPattern = patterns[Random.Range(0, patterns.Length)];

        switch (chosenPattern)
        {
            case FirePattern.Single: FireSingle(); break;
            case FirePattern.MultiAim: FireMultiAim(); break;
            case FirePattern.FanSpread: FireFanSpread(); break;
            case FirePattern.Circle: FireCircle(); break;
        }

        Debug.Log("Boss used pattern: " + chosenPattern);
    }

    // --- Pattern Methods ---
    private void FireSingle()
    {
        Vector3 dir = (player.transform.position - firePoint.position).normalized;
        FireBullet(dir, bulletSpeeds.Length > 0 ? bulletSpeeds[0] : 5f);
    }

    private void FireMultiAim()
    {
        Vector3 playerPos = player.transform.position;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0);
            Vector3 dir = ((playerPos + offset) - firePoint.position).normalized;
            float speed = bulletSpeeds.Length > i ? bulletSpeeds[i] : bulletSpeeds[0];
            FireBullet(dir, speed);
        }
    }

    private void FireFanSpread()
    {
        Vector3 baseDir = (player.transform.position - firePoint.position).normalized;
        float startAngle = -spreadAngle / 2f;
        float angleStep = bulletCount > 1 ? spreadAngle / (bulletCount - 1) : 0;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector3 dir = Quaternion.Euler(0, 0, angle) * baseDir;
            float speed = bulletSpeeds.Length > i ? bulletSpeeds[i] : bulletSpeeds[0];
            FireBullet(dir, speed);
        }
    }

    private void FireCircle()
    {
        float angleStepCircle = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStepCircle;
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
            float speed = bulletSpeeds.Length > i ? bulletSpeeds[i] : bulletSpeeds[0];
            FireBullet(dir, speed);
        }
    }

    private void FireBullet(Vector3 dir, float speed)
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        WizadBullet wizadBullet = bullet.GetComponent<WizadBullet>();
        wizadBullet.SetMovementDirection(dir);
        wizadBullet.SetSpeed(speed);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // --- Damage / Death ---
    public override void TakeDamege(float damage)
    {
        base.TakeDamege(damage);
        if (!isDeadLocal && currentHp <= 0)
        {
            isDeadLocal = true;
            rb.velocity = Vector2.zero;
            anim.SetFloat("speed", 0);
            anim.SetBool("isAttack", false);
        }
    }

    public void OnDieAnimationEnd()
    {
        Destroy(gameObject);
    }

    protected override void Die()
    {
        base.Die();

        // 💥 Rơi tất cả vật phẩm
        DropAllItems();

        // 🔥 Có thể thêm hiệu ứng nổ hoặc animation chết
        Destroy(gameObject, 1.5f); // Xóa boss sau 1.5s
        audioManager.PlayDefaultAudio();
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
