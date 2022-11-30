using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float Damage = 1;
    public float BurningDamage = 0.25f;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(BurnDamageExiting(collision));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float Timer = 0.5f;
            Timer -= Time.deltaTime;
           /* Debug.Log(Timer);*/
            if (Timer < 0)
            {
                Timer = 0;
            }
            if (Timer <= 0)
            {
                collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(Damage);
            }
        }
    }

    private void Start()
    {
       /* StartCoroutine(SelfDestruct());*/
    }
   

    IEnumerator BurnDamageExiting(Collider2D collision)
    {
        collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(BurningDamage);
        yield return new WaitForSeconds(0.5f);
        collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(BurningDamage);
        yield return new WaitForSeconds(0.5f);
        collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(BurningDamage);
        yield return new WaitForSeconds(0.5f);
        collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(BurningDamage);
        yield return new WaitForSeconds(0.5f);
    }
}
