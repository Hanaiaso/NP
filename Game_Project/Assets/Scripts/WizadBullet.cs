using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizadBullet : MonoBehaviour
{
    private Vector3 movementDirection;
    private Transform player;
    void Start()
    {
        player = FindObjectOfType<Player>()?.transform; // tìm player
        if (player != null)
        {
            // tính h??ng t? viên ??n t?i player
            movementDirection = (player.position - transform.position).normalized;
        }
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (movementDirection != Vector3.zero)
        {
            transform.position += movementDirection * 10f * Time.deltaTime;
        }
        else { return; }
    }
    public void SetMovementDirection(Vector3 dir)
    {
        movementDirection = dir;
    }
}
