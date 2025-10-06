using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private float rotateoffset=180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.15f;
    private float nextShot;
    [SerializeField] private int maxAmmo = 34;
    public int currentAmmo;
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo= maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        Shoot();
        Reload();
    }

    void Rotation()
    {
        if(Input.mousePosition.x <0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y <0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }
        Vector3 displacement= transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2 (displacement.y,displacement.x)*Mathf.Rad2Deg;
        transform.rotation= Quaternion.Euler(0,0,angle+ rotateoffset);
        if(angle < -90 || angle > 90)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
    }
    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && Time.time > nextShot)
        {
            nextShot = Time.time+ shotDelay;
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            currentAmmo --;
        }
    }
    void Reload()
    {
        if(Input.GetMouseButtonDown(1) && currentAmmo < maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
    }
}
