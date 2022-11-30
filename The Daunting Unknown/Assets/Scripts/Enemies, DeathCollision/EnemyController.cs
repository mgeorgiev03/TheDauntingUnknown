using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region Variables

    public Animator animator;
    public SpriteRenderer BeetleSprite;
    public float HP = 2;
    public float speed = 30;
    public float StopppingDistance = 1f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public LayerMask PlayerLayerMask;

    private GameObject player;
    private bool HasChangedCollider = false;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private bool reachedStoppingDistance = false;
    private bool facingRight = true;
    private bool isGrounded = true;
    private float JumpForceX = 0f;
    private float JumpForceY = 0f;
    private bool underPlayer = false;

    private GameObject[] ground;
    private GameObject destination;
    private GameObject destinationJumpPoint;
    private GameObject current;
    private float minDistance = float.MaxValue;
    private float minDistance2 = float.MaxValue;
    #endregion

    #region Awake, Update, FixedUpdate
    private void Awake()
    {
        ground = GameObject.FindGameObjectsWithTag("Ground");
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        for (int i = 0; i < ground.Length; i++)
        {
            if (Vector2.Distance(player.transform.position, ground[i].transform.position) <= minDistance2)
            {
                destination = ground[i];
                minDistance2 = Vector2.Distance(player.transform.position, ground[i].transform.position);
            }
        }
        for (int i = 0; i < ground.Length; i++)
        {  
            if (Vector2.Distance(transform.position, ground[i].transform.position) <= minDistance)
            {
                current = ground[i];
                minDistance = Vector2.Distance(transform.position, ground[i].transform.position);
            }
        }

        if (Vector2.Distance(destination.transform.GetChild(0).transform.position, transform.position) < 
            Vector2.Distance(destination.transform.GetChild(1).transform.position, transform.position))
            destinationJumpPoint = destination.transform.GetChild(0).gameObject;
        else
            destinationJumpPoint = destination.transform.GetChild(1).gameObject;

        RaycastHit2D raycast = Physics2D.Raycast(transform.position, Vector2.up, 30f, PlayerLayerMask);
        if (raycast.collider != null)
            StartCoroutine(UnderPlayer());

        if (player != null)
        {
            if (isGrounded && !underPlayer)
            {
                if (HasChangedCollider)
                {
                    boxCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y - 0.61f);
                    boxCollider.offset = new Vector2(boxCollider.offset.x, -0.42f);
                    HasChangedCollider = false;
                }
                animator.SetBool("IsJumping", false);
                if (boxCollider.OverlapPoint(current.transform.GetChild(0).transform.position) ||
                    boxCollider.OverlapPoint(current.transform.GetChild(1).transform.position)
                    && !current.transform.position.Equals(destination.transform.position) && transform.position.y < destinationJumpPoint.transform.position.y)
                {
                    JumpForceX = Mathf.Abs(transform.position.x - destinationJumpPoint.transform.position.x) / 5f;
                    JumpForceY = Mathf.Abs(transform.position.y - destinationJumpPoint.transform.position.y) * 3;

                    if (facingRight)
                        rb.AddForce(new Vector2(JumpForceX, JumpForceY), ForceMode2D.Impulse);
                    else
                        rb.AddForce(new Vector2(-JumpForceX, JumpForceY), ForceMode2D.Impulse);

                    Invoke("Stop", 3f);
                }
                else if (boxCollider.OverlapPoint(current.transform.GetChild(0).transform.position) ||
                    boxCollider.OverlapPoint(current.transform.GetChild(1).transform.position)
                    && !current.transform.position.Equals(destination.transform.position) && transform.position.y >= destinationJumpPoint.transform.position.y &&
                    Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(destinationJumpPoint.transform.position.x)) < 5)
                {
                    JumpForceX = Mathf.Abs(transform.position.x - destinationJumpPoint.transform.position.x) / 10f;
                    JumpForceY = Mathf.Abs(transform.position.y - destinationJumpPoint.transform.position.y);

                    if (facingRight)
                        rb.AddForce(new Vector2(JumpForceX, JumpForceY), ForceMode2D.Impulse);
                    else
                        rb.AddForce(new Vector2(-JumpForceX, JumpForceY), ForceMode2D.Impulse);

                    Invoke("Stop", 1f);
                }
                else if (boxCollider.OverlapPoint(current.transform.GetChild(0).transform.position) ||
                    boxCollider.OverlapPoint(current.transform.GetChild(1).transform.position)
                    && !current.transform.position.Equals(destination.transform.position) && transform.position.y >= destinationJumpPoint.transform.position.y &&
                    Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(destinationJumpPoint.transform.position.x)) >= 5)
                {
                    JumpForceX = Mathf.Abs(transform.position.x - destinationJumpPoint.transform.position.x) / 10f;
                    if (facingRight)
                        rb.AddForce(new Vector2(JumpForceX, 0.2f), ForceMode2D.Impulse);
                    else
                        rb.AddForce(new Vector2(-JumpForceX, 0.2f), ForceMode2D.Impulse);
                    Invoke("Stop", 2f);
                }
            }
            else
            {
                if (!HasChangedCollider)
                {
                    boxCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y + 0.61f);
                    boxCollider.offset = new Vector2(boxCollider.offset.x, (boxCollider.offset.y + 0.61f) / 2);
                    HasChangedCollider = true;
                }
                animator.SetBool("IsJumping", true);
            }
        

            if (facingRight && rb.velocity.x < 0)
                Flip();
            if (!facingRight && rb.velocity.x > 0)
                Flip();
        }

        minDistance = float.MaxValue;
        minDistance2 = float.MaxValue;
    }
    
    private void Stop()
    {
        rb.velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= 20f && !underPlayer) 
        {
            if (!reachedStoppingDistance)
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
            else
                Attack();

            if (StopppingDistance >= Vector2.Distance(transform.position, player.transform.position))
                reachedStoppingDistance = true;
            else
                reachedStoppingDistance = false;
        }
    }

    private IEnumerator UnderPlayer()
    {
        bool goRight;
        underPlayer = true;
        RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, destination.GetComponent<BoxCollider2D>().bounds.extents.x * 2 + GetComponent<BoxCollider2D>().bounds.extents.x * 4, LayerMask.NameToLayer("Wall"));
        if (left.collider == null)
            goRight = true;
        else goRight = false;

        if (goRight)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(destination.transform.GetChild(1).position.x + GetComponent<BoxCollider2D>().bounds.extents.x * 2 + 0.5f, transform.position.y), speed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(destination.transform.GetChild(0).position.x - GetComponent<BoxCollider2D>().bounds.extents.x * 2 - 0.5f, transform.position.y), speed * Time.deltaTime);

        JumpForceX = Mathf.Abs(transform.position.x - destinationJumpPoint.transform.position.x) / 5f;
        JumpForceY = Mathf.Abs(transform.position.y - destinationJumpPoint.transform.position.y) * 3;

        if (facingRight)
            rb.AddForce(new Vector2(JumpForceX, JumpForceY), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(-JumpForceX, JumpForceY), ForceMode2D.Impulse);

        Invoke("Stop", 3f);
        yield return new WaitForSeconds(1f);
        underPlayer = false;
    }
    #endregion

    #region Attack
    void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(GetComponent<Collider2D>().bounds.center, attackRange, PlayerLayerMask);
        if (hit != null)
            StartCoroutine(StartDealingDamage(hit));
    }

    private IEnumerator StartDealingDamage(Collider2D hit)
    {
        if (attackCooldown <= 0f)
        {
            FindObjectOfType<AudioManager>().Play("BugRoach_Attack");
            hit.GetComponent<PlayerMovement>().TakeDamage(1f);
            attackCooldown = 1f;
        }
        else
        {
            attackCooldown -= Time.fixedDeltaTime;
        }
        yield return new WaitForSeconds(1f);
    }
    #endregion

    #region IsGrounded, Flip, TakeDamage
    private bool IsGrounded()
    {
        int enem = 1 << LayerMask.NameToLayer("Enemy");
        int grnd = 1 << LayerMask.NameToLayer("Ground");
        int mask = enem | grnd;

        float extra = .05f;
        RaycastHit2D raycast = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down,
            boxCollider.bounds.extents.y + extra, mask);
        return raycast.collider != null;
    }

    private void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    public void TakeDamage(float damage)
    {
        FindObjectOfType<AudioManager>().Play("BugRoach_TakeDamage");
        StartCoroutine(FlashRed());
        HP -= damage;
        if (HP == 0)
            Destroy(this.gameObject);
    }
    IEnumerator FlashRed()
    {
        BeetleSprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        BeetleSprite.color = Color.white;
    }
    #endregion
}