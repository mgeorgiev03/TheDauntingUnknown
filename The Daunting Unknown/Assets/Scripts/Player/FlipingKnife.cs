using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipingKnife : MonoBehaviour
{
    private Transform player;
    new Rigidbody2D rigidbody;
    private bool hasFlippedRight = true;
    private bool hasFlippedLeft = false;
    private new SpriteRenderer renderer;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (!player.GetComponent<CharacterController2D>().m_FacingRight)
        {
            rigidbody.MovePosition(new Vector2(player.position.x - (-0.182f), player.position.y + (0.484f)));
            renderer.flipX = true;
            if (!hasFlippedLeft)
            {
                transform.GetChild(0).position = new Vector2(transform.GetChild(0).position.x - 0.5f, transform.GetChild(0).position.y);
                hasFlippedLeft = true;
                hasFlippedRight = false;
            }
        }
        else
        {
            rigidbody.MovePosition(new Vector2(player.position.x - (0.182f), player.position.y + (0.484f)));
            renderer.flipX = false;
            if (!hasFlippedRight)
            {
                transform.GetChild(0).position = new Vector2(transform.GetChild(0).position.x + 0.5f, transform.GetChild(0).position.y);
                hasFlippedRight = true;
                hasFlippedLeft = false;
            }
        }
    }
}
