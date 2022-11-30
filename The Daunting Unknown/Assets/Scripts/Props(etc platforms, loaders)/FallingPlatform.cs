using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    new public BoxCollider2D collider;
    public Animator animator;
    void Start()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<AudioManager>().Play("WoodCrack");
            animator.SetBool("Starts_cracking", true);
            Invoke("DisableCollider", 1f);
            Invoke("Reverse_Crack", 2f);
            Invoke("FixCrack", 1.5f);
        }
    }

    void DisableCollider()
    {
        collider.enabled = false;
        animator.SetBool("Reverse_cracking", false);
    }

    void Reverse_Crack()
    {
        animator.SetBool("Reverse_cracking", true);
    }

    void FixCrack()
    {
        animator.SetBool("Starts_cracking", false);

        Invoke("EnableCollider", 1.8f);
    }

    void EnableCollider()
    {
        collider.enabled = true;
    }
}
