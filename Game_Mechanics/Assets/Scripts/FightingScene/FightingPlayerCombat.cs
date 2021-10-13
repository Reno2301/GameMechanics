using UnityEngine;

public class FightingPlayerCombat : MonoBehaviour
{
    public Animator anim;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    float attackRange = 2;
    public int attackDamage = 20;
    float attackRate = 2;
    float attackTime = 0;

    public int maxHealth = 100;
    public int currentHealth;

    public Healthbar healthBar;
    public GameObject playerDeadPanel;
    public GameObject enemy;

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

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
            enemy.GetComponent<Enemy>().EnemyTakeDamage(attackDamage);
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        anim.SetTrigger("GetHit");

        if (currentHealth <= 0)
        {
            PlayerDies();
        }
    }

    void PlayerDies()
    {
        anim.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;

        playerDeadPanel.SetActive(true);

        this.enabled = false;
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

