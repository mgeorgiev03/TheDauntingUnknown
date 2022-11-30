using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffProjectile : MonoBehaviour
{
    public float maxDistance = 45f;
    public float BaseDamage = 1.5f;
    public float RayDamage = 0.5f;
    public float bulletForce = 15f;
    public GameObject bullet;

    private Vector2 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (Vector2.Distance(startPos, transform.position) >= maxDistance)
            Explode();
    }

    private void Explode()
    {
        GameObject bulletRight = Instantiate(bullet, transform.position, transform.rotation);
        Rigidbody2D rb1 = bulletRight.GetComponent<Rigidbody2D>();
        rb1.AddForce(transform.right * bulletForce, ForceMode2D.Impulse);

        GameObject bulletLeft = Instantiate(bullet, transform.position, transform.rotation);
        Rigidbody2D rb2 = bulletLeft.GetComponent<Rigidbody2D>();
        rb2.AddForce(-transform.right * bulletForce, ForceMode2D.Impulse);

        GameObject bulletUp = Instantiate(bullet, transform.position, transform.rotation);
        Rigidbody2D rb3 = bulletUp.GetComponent<Rigidbody2D>();
        rb3.AddForce(transform.up * bulletForce, ForceMode2D.Impulse);

        GameObject bulletDown = Instantiate(bullet, transform.position, transform.rotation);
        Rigidbody2D rb4 = bulletDown.GetComponent<Rigidbody2D>();
        rb4.AddForce(-transform.up * bulletForce, ForceMode2D.Impulse);

        Destroy(gameObject);
    }

    #region OnTrigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTrigger(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTrigger(collision);
    }

    private void OnTrigger(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(1f);

        Explode();
    }
    #endregion
}
