using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWizard : Enemy
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamege(enterDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamege(stayDamage);
        }
    }
    private void BanDanThuong()
    {

    }
}
