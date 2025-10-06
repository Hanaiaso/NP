using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float timeDestroy = 0.5f;
    [SerializeField] private float damage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,timeDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(Vector2.right * moveSpeed*Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamege(damage);
            }
            Destroy(gameObject);
        }
    }
}
