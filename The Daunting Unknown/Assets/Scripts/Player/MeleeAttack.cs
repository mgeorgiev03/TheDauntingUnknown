using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Transform attackPoint;
    public Animator MacheteAnimator;
    public float cooldown = 1f;
    public float attackRange = 1f;
    public float attackDamage = 1.5f;

    public LayerMask enemyLayers;

    private float attackCooldown = 0f;
    private bool attack;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && attackCooldown == 0f)
        {
            Attack();
        }
    }

    void Attack()
    {
        FindObjectOfType<AudioManager>().Play("Machete");

        MacheteAnimator.SetTrigger("Slashing");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            StartCoroutine(StartDealingDamage(enemy));
        }
        attackCooldown = cooldown;
    }
    void FixedUpdate()
    {
        if (!attack)
        {
            attackCooldown -= Time.fixedDeltaTime;

            if (attackCooldown < 0)
                attackCooldown = 0f;

        }
        else if (attack)
            attackCooldown += Time.fixedDeltaTime * 25;

        attack = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    private IEnumerator StartDealingDamage(Collider2D hit)
    {

        if (hit != null)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("bug");
                hit.GetComponent<EnemyController>().TakeDamage(attackDamage);
            }
            else if (hit.gameObject.layer == LayerMask.NameToLayer("FlyingEnemy"))
            {
                Debug.Log("flying");
                hit.GetComponent<Flying_Enemy>().TakeDamage(attackDamage);
            }
            else if (hit.gameObject.layer == LayerMask.NameToLayer("RollingEnemy"))
            {
                hit.GetComponent<RollingEnemy>().TakeDamage(attackDamage);
            }
        }

        yield return new WaitForEndOfFrame();
    }
}
