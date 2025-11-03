using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    private float rotateoffset = 180f;

    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.15f;
    [SerializeField] public int maxAmmo = 24;
    [SerializeField] private TextMeshProUGUI ammoText;
    Animator myAnim;
    private float nextShot;
    [SerializeField] public float reloadTime = 3f;         
    private float reloadTimer = 0f;        
    private bool isReloading = false;      

    public int currentAmmo;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {
        Rotation();
        Shoot();
        HandleReloadHold();
    }

    void Rotation()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width ||
            Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }

        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateoffset);

        if (angle < -90 || angle > 90)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(1, -1, 1);
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && Time.time > nextShot && !isReloading)
        {
            nextShot = Time.time + shotDelay;
            myAnim.SetBool("IsShooting", true);
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            currentAmmo--;
            UpdateAmmoText();
        }
        else
        {
            myAnim.SetBool("IsShooting", false);
        }
    }

    void HandleReloadHold()
    {
        
        if (Input.GetMouseButton(1) && currentAmmo < maxAmmo)
        {
            isReloading = true;
            reloadTimer += Time.deltaTime;

            
            float remaining = Mathf.Clamp(reloadTime - reloadTimer, 0, reloadTime);
            ammoText.text = $"{remaining:F1}s";

            
            if (reloadTimer >= reloadTime)
            {
                currentAmmo = maxAmmo;
                reloadTimer = 0f;
                isReloading = false;
                UpdateAmmoText();
            }
        }

        
        if (Input.GetMouseButtonUp(1))
        {
            if (isReloading && reloadTimer < reloadTime)
            {
                ammoText.text = "Canceled";
                StartCoroutine(ShowAmmoAfterCancel());
            }
            reloadTimer = 0f;
            isReloading = false;
        }
    }

    IEnumerator ShowAmmoAfterCancel()
    {
        yield return new WaitForSeconds(1f);
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            if (currentAmmo > 0)
                ammoText.text = currentAmmo.ToString();
            else
                ammoText.text = "Empty";
        }
    }
}
