using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExperienceController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI ExpText;
    [SerializeField] private int Level;
     public float CurrentExp;
    [SerializeField] private float TargetExp;
    [SerializeField] private Image ExpProgressBar;
    [SerializeField] private Player player;


    // Update is called once per frame
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
            player.maxHp += 30;
            player.currentHp=player.maxHp;
            TargetExp += 30;
        }
    }
}
