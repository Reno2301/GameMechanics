using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float xAxis;
    private float yAxis;
    public float moveSpeed = 7;
    private float jumpForce = 750;

    private bool isJumpPressed;
    private bool isGrounded;

    public Rigidbody2D rb;

    //=============================

    public Transform attackPoint;
    public LayerMask enemyLayers;

    private float attackRange = 2;
    public int attackDamage = 10;
    private float attackRate = 2;
    private float attackTime = 0;

    public int maxHealth;
    public int currentHealth;
    public bool isAttackPressed;
    public bool isAttacking;

    public Healthbar healthBar;
    public GameObject playerDeadPanel;
    public GameObject enemy;

    //=============================

    Animator animator;

    private string currentState;

    //Animation states
    const string PLAYER_IDLE = "PlayerIdle",
                     PLAYER_RUN = "PlayerRun",
                     PLAYER_JUMP = "PlayerJump",
                     PLAYER_ATTACK = "PlayerAttack",
                     PLAYER_TAKE_HIT = "PlayerTakeHit",
                     PLAYER_DEATH = "PlayerDeath";

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //=============================

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPressed = true;
        }

        Debug.Log(currentState);
    }

    private void FixedUpdate()
    {
        //is the player hitting the ground


        //==============================================

        Vector2 vel = new Vector2(0, rb.velocity.y);

        if (xAxis < 0)
        {
            vel.x = -moveSpeed;
            transform.localScale = new Vector2(-1, 1);
        }
        else if (xAxis > 0)
        {
            vel.x = moveSpeed;
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            vel.x = 0;
        }

        if (isGrounded)
        {
            if (xAxis != 0)
            {
                ChangeAnimationState(PLAYER_RUN);
            }
            else
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
        }

        //==============================================

        if (isJumpPressed && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isJumpPressed = false;
            ChangeAnimationState(PLAYER_JUMP);
        }

        rb.velocity = vel;


        if (isAttackPressed)
        {
            isAttackPressed = false;

            if (!isAttacking)
            {
                isAttacking = true;

                Attack();
            }
        }
    }

    void ChangeAnimationState(string newState)
    {
        //don't interrupt the same animation
        if (currentState == newState) return;

        //play the new animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }

    void Attack()
    {
        //check the enemies that are in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage the enemies that are in range
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().EnemyTakeDamage(attackDamage);
        }

        attackTime = Time.time + 1 / attackRate;

        isAttackPressed = false;
    }

    public void PlayerTakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        //anim.SetTrigger("GetHit");

        if (currentHealth <= 0)
        {
            PlayerDies();
        }
    }
    void PlayerDies()
    {
        //anim.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;

        playerDeadPanel.SetActive(true);

        this.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
