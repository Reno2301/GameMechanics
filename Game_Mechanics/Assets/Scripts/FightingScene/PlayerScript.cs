using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public Healthbar healthBar;
    public GameObject playerDeadPanel;
    public GameObject enemy;

    Animator animator;

    //=============================

    private float xAxis;
    private float yAxis;
    public float moveSpeed = 5;
    private float jumpForce = 750;

    private bool isJumpPressed;
    private bool isGrounded;

    public int playerCanAttack;
    public int playerCanAttackTimer = 400;
    public int attackDamage = 10;
    private float attackRange = 1.8f;
    private float attackDelay = 0.15f;
    private float attackRealHit = 0.15f;
    public bool isAttackPressed;
    public bool isAttacking;
    public bool isJumping;

    public int maxHealth;
    public int currentHealth;

    //=============================

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
        playerCanAttack--;

        xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPressed = true;
        }

        if (Input.GetKeyDown("e") && playerCanAttack < 0)
        {
            isAttackPressed = true;
            playerCanAttack = playerCanAttackTimer;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumpPressed = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        //checking if the player can jump
        if (isJumpPressed && isGrounded)
        {
            isJumpPressed = false;
            if (!isJumping)
            {
                isJumping = true;

                Invoke("JumpComplete", attackDelay);

                rb.AddForce(new Vector2(0, jumpForce));

                ChangeAnimationState(PLAYER_JUMP);
            }
        }

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

        if (isGrounded && !isAttacking && !isJumping)
        {
            if (xAxis != 0)
            {
                ChangeAnimationState(PLAYER_RUN);
            }
            else if(xAxis == 0)
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
        }

        //==============================================

        rb.velocity = vel;

        if (isAttackPressed)
        {
            isAttackPressed = false;

            if (!isAttacking)
            {
                isAttacking = true;

                ChangeAnimationState(PLAYER_ATTACK);

                Invoke("Attack", attackRealHit);
            }
        }
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

        Invoke("AttackComplete", attackDelay);
    }

    void AttackComplete()
    {
        isAttacking = false;
    }

    void JumpComplete()
    {
        isJumping = false;
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

    public void PlayerTakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        ChangeAnimationState(PLAYER_TAKE_HIT);

        if (currentHealth <= 0)
        {
            PlayerDies();
        }
    }
    void PlayerDies()
    {
        ChangeAnimationState(PLAYER_DEATH);

        GetComponent<Collider2D>().enabled = false;

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

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
