using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Sprite ShotgunShellSprite;
    public Sprite BloodGunSprite;
    public Sprite LightSpriteSprite;
    private GameObject player;
    private GameObject playerActiveWeapon;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerActiveWeapon = player.GetComponent<PlayerMovement>().activeWeapon;
        if (playerActiveWeapon.CompareTag("Shotgun"))
            GetComponent<SpriteRenderer>().sprite = ShotgunShellSprite;
        else if (playerActiveWeapon.CompareTag("BloodGun"))
            GetComponent<SpriteRenderer>().sprite = BloodGunSprite;
        else if (playerActiveWeapon.CompareTag("LightStaff"))
            GetComponent<SpriteRenderer>().sprite = LightSpriteSprite;
    }
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
        if (playerActiveWeapon != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(1f);
                if (playerActiveWeapon.CompareTag("BloodGun"))
                    player.GetComponent<PlayerMovement>().HealPlayer();
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("FlyingEnemy"))
            {
                collision.gameObject.GetComponent<Flying_Enemy>().TakeDamage(1f);
                if (playerActiveWeapon.CompareTag("BloodGun"))
                    player.GetComponent<PlayerMovement>().HealPlayer();
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("RollingEnemy"))
            {
                collision.gameObject.GetComponent<RollingEnemy>().TakeDamage(1f);
                if (playerActiveWeapon.CompareTag("BloodGun"))
                    player.GetComponent<PlayerMovement>().HealPlayer();
            }
            else if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy") && playerActiveWeapon.CompareTag("BloodGun"))
            {
                player.GetComponent<PlayerMovement>().TakeDamage(1f);
            }
        }

        Destroy(this.gameObject);
    }
}