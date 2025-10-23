using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] public float damage = 10f;
    [SerializeField] private float timeDestroy = 1f;
    [SerializeField] private float timeHit = 1f;
    [SerializeField] GameObject hitPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeDestroy);
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
                GameObject hit=Instantiate(hitPrefab,transform.position, Quaternion.identity);
                Destroy(hit,timeHit);
            }
            Destroy(gameObject);
        }
    }
}
