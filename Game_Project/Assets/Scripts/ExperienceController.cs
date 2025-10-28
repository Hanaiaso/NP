using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class ExperienceController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI ExpText;
    [SerializeField] private TextMeshProUGUI DamageText;
    [SerializeField] private TextMeshProUGUI reloadTimeText;
    [SerializeField] private TextMeshProUGUI Hp;
    [SerializeField] private TextMeshProUGUI UpgradePointText;
    [SerializeField] private int Level;
    public float UpgradePoint=0;
     public float CurrentExp;
    [SerializeField] public float increaseDam=2f;
    [SerializeField] private float TargetExp;
    [SerializeField] private Image ExpProgressBar;
    [SerializeField] public Player player;
    [SerializeField] public PlayerBullet playerBullet;
    [SerializeField] public Gun gun;


    // Update is called once per frame
    private void Start()
    {
        playerBullet.damage =10;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CurrentExp += 12;
        }
        ExpText.text = CurrentExp + " / "+ TargetExp ;
        Hp.text = player.currentHp + "/" + player.maxHp;
        reloadTimeText.text = "Reload: "+gun.reloadTime+"s";
        DamageText.text = "Damage: " + playerBullet.damage;
        UpgradePointText.text = "Upgrade point: " + UpgradePoint;
        ExpController();
    }

    public void ExpController()
    {
        LevelText.text = "Level : "+Level.ToString();
        ExpProgressBar.fillAmount = (CurrentExp/TargetExp);

        if(CurrentExp >= TargetExp && Level <=30)
        {
            CurrentExp = CurrentExp-TargetExp;
            Level++;
            UpgradePoint++;
            playerBullet.damage += increaseDam;
            gun.reloadTime -= 0.1f;
            player.maxHp += 30;
            player.Heal(player.maxHp);
            TargetExp += 30;
        }
    }
}
