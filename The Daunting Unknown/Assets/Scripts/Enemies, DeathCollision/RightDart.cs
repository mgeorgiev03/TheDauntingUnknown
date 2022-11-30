using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightDart : MonoBehaviour
{
    float Damage = 0.5f;
    void Update()
    {
        transform.Translate(Vector2.right * 20f * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(Damage);
            collision.gameObject.GetComponent<PlayerMovement>().SlowDownPlayer();
            Destroy(gameObject);
        }
    }
}
