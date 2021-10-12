using UnityEngine;

public class FightingPlayerCombat : MonoBehaviour
{
    public Animator anim;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 2;
    public int attackDamage = 20;

    public float attackRate = 2;
    float attackTime = 0;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= attackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                attackTime = Time.time + 1 / attackRate;
            }
        }
    }

    void Attack()
    {
        //animate attack
        anim.SetTrigger("Attack");

        //check the enemies that are in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage the enemies that are in range
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    //with this function we can see the range of the attackpoint from the attackpoint.position as a circle.
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //This code was inspired from Brackeys: MELEE COMBAT in Unity (Youtube)
}

