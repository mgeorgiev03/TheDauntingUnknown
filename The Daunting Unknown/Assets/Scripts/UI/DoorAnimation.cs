using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public float distance;
    public float speed;
    public bool moving;
    private Vector2 EndPos;
    private void Start()
    {
        EndPos = new Vector2(transform.position.x, transform.position.y + distance);
    }
    void FixedUpdate()
    {
        if (moving)
        {
            MoveDoor(this.distance);
        }
        if (Vector2.Distance(transform.position , EndPos) < 0.03)
        {
            transform.position = EndPos;
        }

    }

    public void MoveDoor(float distance)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, EndPos, step);
    }
}
