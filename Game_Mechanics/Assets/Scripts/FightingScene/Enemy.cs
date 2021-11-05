using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 60;
    public int currentHealth;
    public int attackDamage = 15;

    public Healthbar healthBar;

    public int speed = 5;

    public Animator enemyAnimator;
    public GameObject panel;
    public PlayerScript playerScript;
    public Rigidbody2D enemyRB;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    private void Update()
    {
        EnemyChasingPlayer();
        EnemyAttack();
    }

    public void EnemyChasingPlayer()
    {
        if (transform.position.x - player.transform.position.x <= 10 || transform.position.x - player.transform.position.x >= -10)
        {
            transform.position = Vector2.MoveTowards(enemyAnimator.transform.position, player.transform.position, speed * Time.deltaTime);
            //enemyAnimator.SetBool("EnemyIsFollowing", false);
        }
        else
        {
            //enemyAnimator.SetBool("EnemyIsFollowing", false);
        }
    }

    public void EnemyAttack()
    {
        if (Input.GetKeyDown("q"))
        {
            //enemyAnimator.SetTrigger("EnemyAttack");
            playerScript.PlayerTakeDamage(attackDamage);
        }
    }


    public void EnemyTakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        //transform.position = new Vector2(transform.position.x + 1, transform.position.y);

        //enemyAnimator.SetTrigger("EnemyTakeDamage");

        if (currentHealth <= 0)
        {
            EnemyDies();
        }
    }

    void EnemyDies()
    {
        //enemyAnimator.SetBool("EnemyIsDead", true);

        GetComponent<Collider2D>().enabled = false;

        enemyRB.constraints = RigidbodyConstraints2D.FreezeAll;

        panel.SetActive(true);

        this.enabled = false;
    }
}
