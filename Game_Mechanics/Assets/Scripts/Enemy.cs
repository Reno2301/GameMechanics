using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int speed = 1;

    public Animator animator;
    public GameObject panel;
    public GameObject player;
    public Rigidbody2D enemyRB;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void EnemyAttack()
    {
        if (/*player.transform.position.x - transform.position.x <= 1*/Input.GetKeyDown("e"))
        {
            player.GetComponent<Health>().TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        transform.position = new Vector2(transform.position.x + 1, transform.position.y);

        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;

        panel.SetActive(true);

        this.enabled = false;
    }
}
