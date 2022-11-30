using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject staffProjectilePrefab;
    public float bulletForce = 20f;
    public float gunReloadTime = 0.5f;
    public float shotgunReloadTime = 1f;
    public float staffFireSpeed = 0.7f;
    public float bloodGunReloadTime = 0.25f;
    public float staffProjectileSpeed = 1f;

    private void Start()
    {
        gunReloadTime = 0f;
        shotgunReloadTime = 0f;
        staffFireSpeed = 0f;
        bloodGunReloadTime = 0f;
    }

    void Update()
    {
        if (!FindObjectOfType<PauseMenu>().GameIsPaused)
        {
            if (GetComponent<PlayerMovement>().activeWeapon != null)
            {
                if (GetComponent<PlayerMovement>().activeWeapon.CompareTag("Gun") && Input.GetKeyDown(KeyCode.Mouse0) && gunReloadTime == 0f)
                {
                    firePoint = transform.GetChild(1).gameObject.transform.GetChild(0);
                    FindObjectOfType<AudioManager>().Play("Gun");
                    Shoot();
                }
                else if (GetComponent<PlayerMovement>().activeWeapon.CompareTag("Shotgun") && Input.GetKeyDown(KeyCode.Mouse0) && shotgunReloadTime == 0f)
                {
                    firePoint = transform.GetChild(2).gameObject.transform.GetChild(0);
                    FindObjectOfType<AudioManager>().Play("Shotgun");
                    ShootShell();
                    ShootShell();
                    ShootShell();
                    ShootShell();
                    ShootShell();
                    shotgunReloadTime = 1f;
                }
                else if (GetComponent<PlayerMovement>().activeWeapon.CompareTag("LightStaff") && Input.GetKeyDown(KeyCode.Mouse0) && staffFireSpeed == 0f)
                {
                    firePoint = transform.GetChild(3).gameObject.transform.GetChild(0);
                    ShootStaffProjectile();
                    staffFireSpeed = 0.7f;
                    FindObjectOfType<AudioManager>().Play("Staff");
                }
                else if (GetComponent<PlayerMovement>().activeWeapon.CompareTag("BloodGun") && Input.GetKeyDown(KeyCode.Mouse0) && bloodGunReloadTime == 0f)
                {
                    firePoint = transform.GetChild(4).gameObject.transform.GetChild(0);
                    FindObjectOfType<AudioManager>().Play("BloodGun");
                    GetComponent<PlayerMovement>().TakeDamage(1f);
                    Shoot();
                }
                else
                {
                    gunReloadTime -= Time.deltaTime;
                    shotgunReloadTime -= Time.deltaTime;
                    staffFireSpeed -= Time.deltaTime;
                    bloodGunReloadTime = Time.deltaTime;
                }


                if (gunReloadTime < 0 && shotgunReloadTime < 0 && staffFireSpeed < 0 && bloodGunReloadTime < 0)
                {
                    gunReloadTime = 0;
                    shotgunReloadTime = 0;
                    staffFireSpeed = 0;
                    bloodGunReloadTime = 0;
                }
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }

    void ShootShell()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (transform.GetChild(2).gameObject.GetComponent<Rigidbody2D>().rotation > 70 && transform.GetChild(2).gameObject.GetComponent<Rigidbody2D>().rotation < 110)
            rb.AddForce(firePoint.right * bulletForce * 3 * new Vector2(Random.Range(0.2f, 1.8f), 1f), ForceMode2D.Impulse);
        else
            rb.AddForce(firePoint.right * bulletForce * 3 * new Vector2(1f, Random.Range(0.2f, 1.8f)), ForceMode2D.Impulse);
    }

    void ShootStaffProjectile()
    {
        GameObject projectile = Instantiate(staffProjectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }
}
