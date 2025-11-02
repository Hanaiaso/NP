using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject shopUIPanel;
    [SerializeField] private GameObject pressBHint;
    [SerializeField] private ExperienceController experienceController;
    [SerializeField] private Player player;
    [SerializeField] private PlayerBullet playerBullet;
    [SerializeField] private Gun gun;
    [SerializeField] private TextMeshProUGUI DamageText;
    [SerializeField] private TextMeshProUGUI reloadTimeText;
    [SerializeField] private TextMeshProUGUI Hp;
    [SerializeField] private TextMeshProUGUI uPText;
    [SerializeField] private float detectRange = 2f; // khoảng cách để phát hiện player

    private bool playerInRange = false;
    private Transform playerTransform;

    void Start()
    {
        if (shopUIPanel != null) shopUIPanel.SetActive(false);
        if (pressBHint != null) pressBHint.SetActive(false);
        if (player != null) playerTransform = player.transform;

        Hp.text = "" + player.maxHp;
        reloadTimeText.text = gun.reloadTime.ToString("F1") + "s";
        DamageText.text = "" + playerBullet.damage;
        uPText.text = "Upgrade Point: " + experienceController.UpgradePoint;
    }

    void Update()
    {
        if (playerTransform != null)
        {
            float distance = Vector2.Distance(playerTransform.position, transform.position);
            playerInRange = distance <= detectRange;
        }

        Hp.text = "" + player.maxHp;
        reloadTimeText.text = gun.reloadTime.ToString("F1") + "s";
        DamageText.text = "" + playerBullet.damage;
        uPText.text = "Upgrade Point: " + experienceController.UpgradePoint;

        if (playerInRange)
        {
            if (pressBHint != null && !shopUIPanel.activeSelf)
                pressBHint.SetActive(true);

            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenShop();
            }
        }
        else
        {
            if (pressBHint != null)
                pressBHint.SetActive(false);
        }
    }

    private void OpenShop()
    {
        if (shopUIPanel != null)
        {
            shopUIPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        if (pressBHint != null)
            pressBHint.SetActive(false);
    }

    public void CloseShop()
    {
        if (shopUIPanel != null)
        {
            shopUIPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void UpgradeDamage()
    {
        if (experienceController.UpgradePoint > 0)
        {
            playerBullet.damage += 2;
            experienceController.UpgradePoint--;
        }
        else Debug.Log("Not enough upgrade points!");
    }

    public void UpgradeReload()
    {
        if (experienceController.UpgradePoint > 0)
        {
            if (gun.reloadTime > 0.5f)
                gun.reloadTime -= 0.1f;

            experienceController.UpgradePoint--;
        }
        else Debug.Log("Not enough upgrade points!");
    }

    public void UpgradeHealth()
    {
        if (experienceController.UpgradePoint > 0)
        {
            player.maxHp += 30;
            player.Heal(player.maxHp);
            experienceController.UpgradePoint--;
        }
        else Debug.Log("Not enough upgrade points!");
    }
}
