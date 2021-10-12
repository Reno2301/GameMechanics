using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingPlayerCombat : MonoBehaviour
{
    public Animator anim;

    public Transform attackPoint;
    public float attackRange = 2;

    public LayerMask enemyLayers;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
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
            Debug.Log("WOW!");
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

