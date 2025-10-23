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
    [SerializeField] private int Level;
     public float CurrentExp;
    [SerializeField] public float increaseDam=2f;
    [SerializeField] private float TargetExp;
    [SerializeField] private Image ExpProgressBar;
    [SerializeField] private Player player;
    [SerializeField] private PlayerBullet playerBullet;
    [SerializeField] private Gun gun;


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
        ExpController();
    }

    public void ExpController()
    {
        LevelText.text = "Level : "+Level.ToString();
        ExpProgressBar.fillAmount = (CurrentExp/TargetExp);

        if(CurrentExp >= TargetExp)
        {
            CurrentExp = CurrentExp-TargetExp;
            Level++;
            playerBullet.damage += increaseDam;
            gun.reloadTime -= 0.1f;
            player.maxHp += 30;
            player.Heal(player.maxHp);
            TargetExp += 30;
        }
    }
}
