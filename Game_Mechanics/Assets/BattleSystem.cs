using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerPosition;
    public Transform enemyPosition;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    // Start is called before the first frame update
    void Awake()
    {
        state = BattleState.START;

        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGameObject = Instantiate(playerPrefab, playerPosition);
        playerUnit = playerGameObject.GetComponent<Unit>();

        GameObject enemyGameObject = Instantiate(enemyPrefab, enemyPosition);
        enemyUnit = enemyGameObject.GetComponent<Unit>();

        dialogueText.text = "Your enemy is " + enemyUnit.unitName + ".\nTake him out!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " is now choosing.";

        yield return new WaitForSeconds(2f);

        //Enemy will rest
        if (enemyUnit.currentStamina <= 30)
        {
            dialogueText.text = enemyUnit.unitName + " is resting.";

            yield return new WaitForSeconds(1f);

            enemyUnit.currentStamina += enemyUnit.staminaAdd;
            enemyHUD.SetHealthAndStamina(enemyUnit.currentHP, enemyUnit.currentStamina);

            yield return new WaitForSeconds(1f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        //Enemy will hit you
        else
        {
            dialogueText.text = enemyUnit.unitName + " hits you!";

            yield return new WaitForSeconds(1f);
            enemyUnit.currentStamina -= enemyUnit.attack1Stamina;
            enemyHUD.SetHealthAndStamina(enemyUnit.currentHP, enemyUnit.currentStamina);

            yield return new WaitForSeconds(1f);

            bool isDead = playerUnit.TakeDamage(enemyUnit.attack1Damage);

            playerHUD.SetHealthAndStamina(playerUnit.currentHP, playerUnit.currentStamina);

            yield return new WaitForSeconds(1f);

            if (isDead)
            {
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
    }

    IEnumerator PlayerAttack1()
    {
        if (playerUnit.currentStamina >= playerUnit.attack1Stamina)
        {
            dialogueText.text = "You used HIT!";
            playerUnit.currentStamina -= playerUnit.attack1Stamina;
            playerHUD.SetHealthAndStamina(playerUnit.currentHP, playerUnit.currentStamina);

            yield return new WaitForSeconds(1f);

            bool isDead = enemyUnit.TakeDamage(playerUnit.attack1Damage);
            enemyHUD.SetHealthAndStamina(enemyUnit.currentHP, enemyUnit.currentStamina);

            dialogueText.text = "The attack is successful!";

            yield return new WaitForSeconds(1f);

            if (isDead)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
        else
        {
            dialogueText.text = "You don't have enough stamina.";

            yield return new WaitForSeconds(1f);

            state = BattleState.PLAYERTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerAttack2()
    {
        if (playerUnit.currentStamina >= playerUnit.attack2Stamina)
        {
            dialogueText.text = "You used KICK!";
            playerUnit.currentStamina -= playerUnit.attack2Stamina;
            playerHUD.SetHealthAndStamina(playerUnit.currentHP, playerUnit.currentStamina);

            yield return new WaitForSeconds(1f);

            bool isDead = enemyUnit.TakeDamage(playerUnit.attack2Damage);
            enemyHUD.SetHealthAndStamina(enemyUnit.currentHP, enemyUnit.currentStamina);

            dialogueText.text = "The attack is successful!";

            yield return new WaitForSeconds(1f);

            if (isDead)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
        else
        {
            dialogueText.text = "You don't have enough stamina.";

            yield return new WaitForSeconds(1f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator PlayerAddStamina()
    {
        dialogueText.text = "You are resting.";

        yield return new WaitForSeconds(1f);

        playerUnit.AddStamina(playerUnit.staminaAdd);
        dialogueText.text = "Your stamina is now " + playerUnit.currentStamina + ".";

        playerHUD.SetHealthAndStamina(playerUnit.currentHP, playerUnit.currentStamina);

        yield return new WaitForSeconds(1f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerHeal()
    {
        if (playerUnit.potion >= 1)
        {
            playerUnit.potion--;
            dialogueText.text = "You are healing.";

            yield return new WaitForSeconds(1f);

            playerUnit.AddHealth(playerUnit.healthAdd);
            dialogueText.text = "Your health is now " + playerUnit.currentHP + ".";

            playerHUD.SetHealthAndStamina(playerUnit.currentHP, playerUnit.currentStamina);

            yield return new WaitForSeconds(1f);

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        else
        {
            dialogueText.text = "You don't have enough potions.";

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";

            yield return new WaitForSeconds(2f);

            dialogueText.text = "You earned 120 experience!";

            //experience += 120;
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You lost the battle!";
        }

        dialogueText.text = "Returning to other scene";

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action.";
    }

    public void OnAttack1Button()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack1());
    }

    public void OnAttack2Button()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack2());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }

    public void OnRestButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAddStamina());
    }
}
