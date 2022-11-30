using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingEnemy : MonoBehaviour
{
    public float speed = 25f;
    public float HP = 3f;

    private GameObject[] platforms;
    private GameObject currentPlatform;
    private GameObject player;
    public Vector2 StopPoint1;
    public Vector2 StopPoint2;
    private BoxCollider2D circleCollider;
    private Rigidbody2D rb;
    public SpriteRenderer SkeletonSprite;
    private CapsuleCollider2D capsuleCollider;

    private bool facingRight = true;
    private float minDistance = float.MaxValue;

    private void OnEnable()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        circleCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        platforms = GameObject.FindGameObjectsWithTag("Ground");

        for (int i = 0; i < platforms.Length; i++)
        {
            if (Vector2.Distance(platforms[i].GetComponent<BoxCollider2D>().ClosestPoint(transform.position), transform.position) < minDistance)
            {
                minDistance = Vector2.Distance(platforms[i].GetComponent<BoxCollider2D>().ClosestPoint(transform.position), transform.position);
                currentPlatform = platforms[i];
            }
        }

        StopPoint1 = new Vector2(currentPlatform.transform.position.x - currentPlatform.GetComponent<BoxCollider2D>().bounds.extents.x,
                    currentPlatform.transform.position.y + currentPlatform.GetComponent<BoxCollider2D>().bounds.extents.y * 2 + 0.1f);

        StopPoint2 = new Vector2(currentPlatform.transform.position.x + currentPlatform.GetComponent<BoxCollider2D>().bounds.extents.x,
                    currentPlatform.transform.position.y + currentPlatform.GetComponent<BoxCollider2D>().bounds.extents.y * 2 + 0.1f);
    }

    void Update()
    {
        
            if (facingRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, StopPoint2, speed * Time.deltaTime);
                if (circleCollider.OverlapPoint(StopPoint2))
                {
                    rb.velocity = Vector2.zero;
                    Flip();
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, StopPoint1, speed * Time.deltaTime);
                if (circleCollider.OverlapPoint(StopPoint1))
                {
                    rb.velocity = Vector2.zero;
                    Flip();
                }
            }
    }


    public void TakeDamage(float damage)
    {
        StartCoroutine(FlashRed());
        HP -= damage;
        if (HP <= 0)
            Destroy(gameObject);
    }

    private void Flip()
    {
        if (facingRight)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;

        facingRight = !facingRight;
    }
    IEnumerator FlashRed()
    {
        FindObjectOfType<AudioManager>().Play("Skeleton_TakeDamage");
        SkeletonSprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        SkeletonSprite.color = Color.white;
    }

 
}
