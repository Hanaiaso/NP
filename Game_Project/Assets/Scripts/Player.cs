using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] public float maxHp = 100f;
    [SerializeField] public float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] private GameManage gameManage;
    [SerializeField] private ExperienceController expController;
    [SerializeField] private float increaseExp=30f;

    public float damageBonus = 2f;
    public float reloadReduce = 0.2f;
    public float healAmount = 50f;
    public float chestExpBonus = 30f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        UpdateHpBar();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        FlipToMouse();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManage.GamePauseMenu();
        }
    }
    void MovePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = playerInput.normalized * moveSpeed;
        
        if (playerInput != Vector2.zero)
        {
            animator.SetBool("IsRun", true);
        }
        else
        {
            animator.SetBool("IsRun", false);
        }
    }

    void FlipToMouse()
    {
        // Lấy vị trí chuột theo thế giới (world space)
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Nếu chuột ở bên trái player → lật sang trái
        if (mouseWorldPos.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void TakeDamege(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0)
        { 
            Die();
        }
    }

    public void Heal(float healValue)
    {
        if (currentHp < maxHp)
        {
            currentHp += healValue;
            currentHp = Mathf.Min(currentHp, maxHp);
            UpdateHpBar();
        }
    }

    private void Die()
    {
        gameManage.LoseGameMenu();
    }
    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Energy"))
        {
            expController.CurrentExp += increaseExp;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Damage"))
        {
            expController.playerBullet.damage += damageBonus;
            Debug.Log("Damage increased!");
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("ChestEn"))
        {
            expController.CurrentExp += chestExpBonus;
            Debug.Log("Bonus EXP from chest!");
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("UpdatePoint"))
        {
            expController.UpgradePoint += 1;
            Debug.Log("Upgrade point +1");
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Hp"))
        {
            expController.player.maxHp += healAmount;
            expController.player.Heal(healAmount);
            Debug.Log("Healed +" + healAmount);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("ReloadSpeed"))
        {
            expController.gun.reloadTime = Mathf.Max(0.3f, expController.gun.reloadTime - reloadReduce);
            Debug.Log("Reload speed improved!");
            Destroy(collision.gameObject);
        }
    }
}
