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
    [SerializeField] private float shootCooldown = 1.5f; // 1.5s mỗi phát
    private float lastShootTime = 0f;
    [Header("Hồi máu")]
    [SerializeField] private float hpValue = 100f;

    [Header("Đánh thường")]
    [SerializeField] private Transform meleePoint;  // nơi chém (có Animator riêng nếu cần)
    [SerializeField] private float meleeRange = 40f; // phạm vi tấn công
    [SerializeField] private float meleeAttackRange = 0.5f; // phạm vi sát thương
    [SerializeField] private float meleeDamage = 2f; // sát thương
    [SerializeField] private float meleeAttackAnimationTime = 0.5f; // thời gian tấn công
    [SerializeField] private float meleeAttackDamageDelay = 0.1f; // thời gian delay debug animation chậm
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private bool isMeleeAttacked = false;

    [SerializeField] private Animator meleeAnimator; // Animator riêng của MeleePoint
    private float lastAttackTime = 0f;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

        // lấy animator từ MeleePoint (nếu có)
        //if (meleePoint != null)
        //    meleeAnimator = meleePoint.GetComponent<Animator>();
    }

    //protected override void FixedUpdate()
    //{
    //    base.FixedUpdate();

    //    if (player == null) return;

    //    float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

    //    // Nếu boss ở quá xa player → teleport lại gần
    //    if (distanceToPlayer >= meleeRange * 10f) // ví dụ: xa gấp 10 lần tầm đánh thì teleport
    //    {
    //        DichChuyen();
    //        return;
    //    }

    //    // Nếu trong tầm đánh cận → chém
    //    if (distanceToPlayer <= meleeRange)
    //    {
    //        DanhThuong();

    //    }
    //    else
    //    {
    //        BanDanThuong();

    //    }
    //}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Nếu boss ở quá xa player → teleport lại gần
        if (distanceToPlayer >= meleeRange * 10f) // ví dụ: xa gấp 10 lần tầm đánh thì teleport
        {
            DichChuyen();
            return; // sau khi teleport thì bỏ qua frame này
        }

        if (Time.time < lastAttackTime + attackCooldown) //Trong khi danh thuong
        {
            GaySatThuongDanhThuong();
            return;
        }
        // Nếu trong tầm đánh cận → chém

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

    private void OnTriggerEnter2D(Collider2D collision) { if (collision.CompareTag("Player")) { player.TakeDamege(enterDamage); } }
    private void OnTriggerStay2D(Collider2D collision) { if (collision.CompareTag("Player")) { player.TakeDamege(stayDamage); } }


    private void DanhThuong()
    {
        lastAttackTime = Time.time;

        // Ngừng bắn khi đang chém
        StopCoroutine("ShootLoop");

        // Gọi animation chém
        if (meleeAnimator != null)
            meleeAnimator.SetTrigger("Slash");

        Debug.Log("Boss bắt đầu chém!");
    }

    private void GaySatThuongDanhThuong()
    {
        if(isMeleeAttacked) return;
        if (Time.time >= lastAttackTime + meleeAttackAnimationTime) return;
        if (Time.time < lastAttackTime + meleeAttackDamageDelay) return;
        float distanceToPlayer = Vector2.Distance(meleePoint.position, player.transform.position);
        if (distanceToPlayer > meleeAttackRange) return;
        isMeleeAttacked = true;
        player.TakeDamege(meleeDamage);
        Debug.Log("Attack at " + Time.time);
        Debug.Log("Start attack at " + lastAttackTime);
    }

    //private void DanhThuong()
    //{
    //    if (player == null) return;

    //    float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

    //    // Nếu trong phạm vi đánh
    //    if (distanceToPlayer <= meleeRange)
    //    {
    //        // Nếu đã cooldown xong và chưa đang chém
    //        if (Time.time >= lastAttackTime + attackCooldown)
    //        {
    //            lastAttackTime = Time.time;

    //            // Ngừng bắn khi đang chém
    //            StopCoroutine("ShootLoop");

    //            // Gọi animation chém
    //            if (meleeAnimator != null)
    //                meleeAnimator.SetTrigger("Slash");

    //            Debug.Log("Boss bắt đầu chém!");
    //        }
    //    }
    //}

    // Gọi từ Animation Event trong clip Slash
    public void DealMeleeDamage()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= meleeRange)
        {
            player.TakeDamege(enterDamage);
            Debug.Log("Boss gây sát thương cận chiến!");
        }
    }


    //private void DanhThuong()
    //{
    //    if (player == null) return;

    //    float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

    //    // Nếu trong phạm vi đánh
    //    if (distanceToPlayer <= meleeRange)
    //    {
    //        // chỉ chém nếu chưa chém hoặc đã cooldown xong
    //        if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
    //        {
    //            isAttacking = true;
    //            lastAttackTime = Time.time;

    //            if (meleeAnimator != null)
    //                meleeAnimator.SetBool("isSlashing", true); // bật loop animation

    //            // Gây sát thương cho player
    //            player.TakeDamege(enterDamage);
    //            Debug.Log("Boss đang chém!");
    //        }
    //    }
    //    else
    //    {
    //        // nếu player ra khỏi phạm vi => dừng chém
    //        if (isAttacking)
    //        {
    //            isAttacking = false;
    //            if (meleeAnimator != null)
    //                meleeAnimator.SetBool("isSlashing", false);
    //            Debug.Log("Boss dừng chém.");
    //        }
    //    }
    //}

    private void BanDanThuong()
    {
        if (player == null) return;

        // Chờ đến khi đủ cooldown mới bắn
        if (Time.time < lastShootTime + shootCooldown)
            return;

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
            Vector2 direction = transform.position-player.transform.position; // lấy hướng của player hiện tại = sau đít
            Vector3 direction3 = direction.normalized;
            transform.position = player.transform.position + direction3 * 3 * meleeAttackRange;
        }
    }
}


