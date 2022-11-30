using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 1f;
    private bool hasHitPlayer = false;

    public Sprite ShotgunShellSprite;
    private GameObject enemy;
    private GameObject enemyActiveWeapon; 

    private void Awake()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
       // enemyActiveWeapon = enemy.GetComponent<EnemyController>().weapon;
        if (enemyActiveWeapon.CompareTag("Shotgun"))
            GetComponent<SpriteRenderer>().sprite = ShotgunShellSprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTrigger(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTrigger(collision);
    }

    private void OnTrigger(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasHitPlayer)
        {
            collision.GetComponent<PlayerMovement>().TakeDamage(damage);
            hasHitPlayer = true;
        }
    }
}
