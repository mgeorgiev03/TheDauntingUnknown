using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 5f;
    public float EndX;
    public float EndY;

    Vector3 StartPos;
    Vector3 NextPos;
    Vector3 EndPos;

    void Start()
    {
        EndPos = new Vector3(EndX, EndY, 0f);
        StartPos = transform.position;
        NextPos = StartPos;
    }


    void Update()
    {
        if (transform.position == EndPos)
        {
            NextPos = StartPos; 

            if (gameObject.tag == "Enemy")
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }
        else if (transform.position == StartPos)
        {
            NextPos = EndPos;

            if (gameObject.tag == "Enemy")
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, NextPos, speed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (transform.gameObject.layer != LayerMask.NameToLayer("Enemies"))
            collision.gameObject.transform.parent = transform;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.transform.parent = null;
    }
}