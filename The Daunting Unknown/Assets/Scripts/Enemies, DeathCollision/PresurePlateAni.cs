using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresurePlateAni : MonoBehaviour
{
    public Animator animator;
    new private BoxCollider2D collider;
    // Start is called before the first frame update
    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collider.gameObject.tag == "Player")
        {
            animator.SetBool("IsOpening", true);
        }
    }
}
