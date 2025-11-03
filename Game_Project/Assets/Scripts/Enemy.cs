using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnDeath;
    [SerializeField] protected float enemyMoveSpeed = 1f;
    protected Player player;
    protected Rigidbody2D rb;

    [SerializeField] protected float maxHp = 50f;
    protected float currentHp;
    [SerializeField] private Image hpBar;

    [SerializeField] protected float enterDamage = 10f;
    [SerializeField] protected float stayDamage = 1f;
    public bool isBoss = false;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        currentHp = maxHp;
        UpdateHpBar();
    }

    protected virtual void FixedUpdate()
    {
        MoveToPlayer();
    }

    protected void MoveToPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * enemyMoveSpeed * Time.fixedDeltaTime);
        FlipEnemy();
    }

    protected void FlipEnemy()
    {
        if (player != null)
        {
            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
    }

    public virtual void TakeDamege(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke(this);
        if (isBoss)
        {
            AnnouncementManager.Instance?.ShowMessage("Boss đã bị đánh bại!");
        }

        Destroy(gameObject);
    }

    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}