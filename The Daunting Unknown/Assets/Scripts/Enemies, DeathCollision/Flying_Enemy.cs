using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Flying_Enemy : MonoBehaviour
{
    public SpriteRenderer FlySprite;
    public Transform plr;
    public float HP = 2;
    public AIPath aiPath;
    public float attackCooldown = 1f;
    public float attackRange = 1f;
    float Damage = 1f;
    public LayerMask PlayerLayerMask;

    private void Awake()
    {
        plr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (aiPath.desiredVelocity.x > 0.01f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (aiPath.desiredVelocity.x < -0.01f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void FixedUpdate()
    {
        Attack();
    }
    public void TakeDamage(float Damage)
    {
        FindObjectOfType<AudioManager>().Play("Fly_TakeDamage");
        StartCoroutine(FlashRed());
        HP -= Damage;
        if (HP == 0)
            Destroy(gameObject.transform.parent.gameObject);
    }
    private IEnumerator StartDealingDamage(Collider2D hit)
    {
        if (attackCooldown <= 0f)
        {
            FindObjectOfType<AudioManager>().Play("Fly_Attack");
            hit.GetComponent<PlayerMovement>().TakeDamage(Damage);
            attackCooldown = 1f;
        }
        else
        {
            attackCooldown -= Time.fixedDeltaTime;
        }
        yield return new WaitForSeconds(1f);
    }
    void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(GetComponent<Collider2D>().bounds.center, attackRange, PlayerLayerMask);
        if (hit != null)
            StartCoroutine(StartDealingDamage(hit));
    }
    IEnumerator FlashRed()
    {
        FlySprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        FlySprite.color = Color.white;
    }
}
