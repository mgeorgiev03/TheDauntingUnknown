using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletForce;

    private Transform firePoint;
    private GameObject player;
    private Vector2 playerPos;
    new private SpriteRenderer renderer;
    new private Rigidbody2D rigidbody;

    void Start()
    {
        firePoint = transform.GetChild(0).transform;
        player = GameObject.FindGameObjectWithTag("Player");
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        playerPos = player.transform.position;

        if (rigidbody.rotation >= 90 || rigidbody.rotation <= -90)
            renderer.flipY = true;
        else
            renderer.flipY = false;
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = playerPos - rigidbody.position;
        lookDir.Normalize();
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }

    public void ShootShell()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rigidbody.rotation > 70 && rigidbody.rotation < 110)
            rb.AddForce(firePoint.right * bulletForce * 0.3f * new Vector2(Random.Range(0.3f, 1.7f), 1f), ForceMode2D.Impulse);
        else
            rb.AddForce(firePoint.right * bulletForce * 0.3f * new Vector2(1f, Random.Range(0.3f, 1.7f)), ForceMode2D.Impulse);
    }
}
