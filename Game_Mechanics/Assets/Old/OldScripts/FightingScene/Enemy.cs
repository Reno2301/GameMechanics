using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class OldEnemy : MonoBehaviour
{
    public int maxHealth = 60;
    public int currentHealth;
    public int attackDamage = 15;

    public OldHealthbar healthBar;

    public int speed = 1;

    public Animator animator;
    public GameObject panel;
    public GameObject player;
    public Rigidbody2D enemyRB;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        EnemyAttack();
    }

    public void EnemyAttack()
    {
        if (/*player.transform.position.x - transform.position.x <= 1*/Input.GetKeyDown("e"))
        {
            animator.SetTrigger("EnemyAttack");
            player.GetComponent<OldFightingPlayerCombat>().PlayerTakeDamage(attackDamage);
        }
    }

    public void EnemyTakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        //transform.position = new Vector2(transform.position.x + 1, transform.position.y);

        animator.SetTrigger("EnemyTakeDamage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("EnemyIsDead", true);

        GetComponent<Collider2D>().enabled = false;

        panel.SetActive(true);

        this.enabled = false;

        player.GetComponent<OldFightingPlayerCombat>().maxHealth += 10;
        player.GetComponent<OldFightingPlayerMovement>().moveSpeed += 1;
    }
}
