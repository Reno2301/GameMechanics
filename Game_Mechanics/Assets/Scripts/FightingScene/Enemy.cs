using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 60;
    public int currentHealth;

    public Healthbar healthBar;

    public int speed = 5;

    public Animator enemyAnimator;
    public GameObject panel;
    public PlayerScript playerScript;
    public Rigidbody2D enemyRB;

    public GameObject player;

    public Transform enemyAttackPoint;
    public LayerMask playerLayer;

    public int attackDamage = 15;
    public int enemyCanAttack;
    public int enemyCanAttackTimer = 400;
    private float enemyAttackRange = 1.8f;
    private float enemyAttackDelay = 0.15f;
    private float enemyAttackRealHit = 0.15f;

    public bool isEnemyAttackPressed;
    public bool isEnemyAttacking;

    private string currentState;

    //Animation states
    const string ENEMY_IDLE = "EnemyIdle",
                 ENEMY_RUN = "EnemyRun",
                 ENEMY_ATTACK = "EnemyAttack",
                 ENEMY_TAKE_HIT = "EnemyTakeHit",
                 ENEMY_DEATH = "EnemyDeath",
                 ENEMY_DEATH_2 = "EnemyDeath2";

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
        enemyCanAttack--;

        EnemyChasingPlayer();

        if (Input.GetKeyDown("q") && enemyCanAttack <= 0)
        {
            isEnemyAttackPressed = true;
            enemyCanAttack = enemyCanAttackTimer;
        }

        if(playerScript.currentHealth <= 0)
            enemyRB.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void FixedUpdate()
    {
        if (isEnemyAttackPressed)
        {
            isEnemyAttackPressed = false;

            if (!isEnemyAttacking)
            {
                isEnemyAttacking = true;

                ChangeAnimationState(ENEMY_ATTACK);

                Invoke("EnemyAttack", enemyAttackRealHit);
            }
        }
    }

    void EnemyAttack()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(enemyAttackPoint.position, enemyAttackRange, playerLayer);

        foreach (Collider2D player in players)
        {
            player.GetComponent<PlayerScript>().PlayerTakeDamage(attackDamage);
        }

        Invoke("EnemyAttackComplete", enemyAttackDelay);
    }

    void EnemyAttackComplete()
    {
        isEnemyAttacking = false;
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        enemyAnimator.Play(newState);

        currentState = newState;
    }

    public void EnemyChasingPlayer()
    {
        if (!isEnemyAttacking)
        {
            if (transform.position.x - player.transform.position.x <= 10 || transform.position.x - player.transform.position.x >= -10)
            {
                ChangeAnimationState(ENEMY_RUN);
            }
            else
            {
                ChangeAnimationState(ENEMY_IDLE);
            }
        }
    }

    public void EnemyTakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        //transform.position = new Vector2(transform.position.x + 1, transform.position.y);

        ChangeAnimationState(ENEMY_TAKE_HIT);

        if (currentHealth <= 0)
        {
            EnemyDies();
        }
    }

    void EnemyDies()
    {
        ChangeAnimationState(ENEMY_DEATH_2);

        GetComponent<Collider2D>().enabled = false;

        enemyRB.constraints = RigidbodyConstraints2D.FreezeAll;
        playerScript.rb.constraints = RigidbodyConstraints2D.FreezeAll;

        panel.SetActive(true);

        this.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        if (enemyAttackPoint == null)
            return;

        Gizmos.DrawWireSphere(enemyAttackPoint.position, enemyAttackRange);
    }
}
