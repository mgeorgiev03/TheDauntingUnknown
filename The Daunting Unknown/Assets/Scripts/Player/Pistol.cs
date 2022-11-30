using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public Transform player;
    new Rigidbody2D rigidbody;
    private Vector2 mousePos;
    new SpriteRenderer renderer;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (rigidbody.rotation >= 90 || rigidbody.rotation <= -90)
            renderer.flipY = true;
        else
            renderer.flipY = false;
    }

    private void FixedUpdate()
    {
        if (!transform.parent.GetComponent<CharacterController2D>().m_FacingRight)
            rigidbody.MovePosition(new Vector2(player.position.x - (-0.108f), player.position.y + (0.523f)));
        else
            rigidbody.MovePosition(new Vector2(player.position.x - (0.108f), player.position.y + (0.523f)));

        Vector2 lookDir = mousePos - rigidbody.position;
        lookDir.Normalize();
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}