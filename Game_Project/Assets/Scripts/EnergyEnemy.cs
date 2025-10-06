using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyEnemy : Enemy
{
    [SerializeField] private GameObject energryObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamege(enterDamage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamege(stayDamage);
            }
        }
    }

    protected override void Die()
    {
        if (energryObject != null) { 
            GameObject energy = Instantiate(energryObject, transform.position, Quaternion.identity);
            Destroy(energy, 5f);
        }
        base.Die();
    }

}
