using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 60;
    public int currentHealth;
    public int attackDamage = 15;

    public Healthbar healthBar;

    public int speed = 5;

    public Animator animator;
    public GameObject panel;
    public GameObject player;
    public Rigidbody2D enemyRB;

    private Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        EnemyChasePlayer();
        EnemyAttack();
    }

    public void EnemyChasePlayer()
    {
        if (transform.position.x - playerPos.position.x <= 10 || transform.position.x - playerPos.position.x >= -10)
        {
            transform.position = Vector2.MoveTowards(animator.transform.position, playerPos.position, speed * Time.deltaTime);
            animator.SetBool("EnemyIsFollowing", false);
        }
        else
        {
            animator.SetBool("EnemyIsFollowing", false);
        }
    }

    public void EnemyAttack()
    {
        if (Input.GetKeyDown("q"))
        {
            animator.SetTrigger("EnemyAttack");
            player.GetComponent<FightingPlayerCombat>().PlayerTakeDamage(attackDamage);
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

        player.GetComponent<FightingPlayerCombat>().maxHealth += 10;
        player.GetComponent<FightingPlayerMovement>().moveSpeed += 1;
    }
}
