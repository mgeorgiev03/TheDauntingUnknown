using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingEnemyDamage : MonoBehaviour
{
    public float damage = 1f;
    public float timeBetweenDamagingPlayer = 0.5f;
    private float time;

    private void Awake()
    {
        time = timeBetweenDamagingPlayer;
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
        if (collision.gameObject.CompareTag("Player") && timeBetweenDamagingPlayer == 0f)
        {
            collision.GetComponent<PlayerMovement>().TakeDamage(damage);
            timeBetweenDamagingPlayer = 1f;
        }
        else
            timeBetweenDamagingPlayer -= Time.deltaTime;

        if (timeBetweenDamagingPlayer < 0)
            timeBetweenDamagingPlayer = 0;
    }
}
