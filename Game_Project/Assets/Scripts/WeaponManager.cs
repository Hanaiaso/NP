using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    public SpriteRenderer playerGun;

    public Sprite basicGun;
    public Sprite silverGun;
    public Sprite goldGun;
    private bool hasSilverGun = false;
    private bool hasGoldGun = false;


    void Start()
    {
        // Bắt đầu game với súng cơ bản
        SwitchToBasicGun();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToBasicGun();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && hasSilverGun)
        {
            SwitchToSilverGun();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && hasGoldGun)
        {
            SwitchToGoldGun();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
 
        if (collision.gameObject.tag == "SilverGun")
        {
            hasSilverGun = true; 
            SwitchToSilverGun(); 
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "GoldGun")
        {
            hasGoldGun = true;
            SwitchToGoldGun(); 
            Destroy(collision.gameObject);
        }
    }

  

    void SwitchToBasicGun()
    {

        playerGun.gameObject.tag = "Gun";
        playerGun.sprite = basicGun;

    }

    void SwitchToSilverGun()
    {
        playerGun.gameObject.tag = "SilverGun"; 
        playerGun.sprite = silverGun;

    }

    void SwitchToGoldGun()
    {
        playerGun.gameObject.tag = "GoldGun"; 
        playerGun.sprite = goldGun;
    }
}