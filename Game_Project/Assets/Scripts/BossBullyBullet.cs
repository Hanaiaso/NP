using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullyBullet : MonoBehaviour
{
    private Vector3 movementDirection;
    [SerializeField] private float bulletDamage = 10f;
    void Start()
    {
        Destroy(gameObject,5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (movementDirection == Vector3.zero) return;

        transform.position += movementDirection * Time.deltaTime;


    }

    public void SetMovementDirection(Vector3 direction)
    {
       movementDirection = direction ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamege(bulletDamage);
                Debug.Log("Đạn boss gây sát thương!");
            }

            Destroy(gameObject);
        }
    }

}
