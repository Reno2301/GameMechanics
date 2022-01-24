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

    TransitionAnimation tranAnim;

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

        dialogueText.text = "Your enemy is " + enemyUnit.unitName + ".\nGo for it!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator EnemyTurn()
    {
        if(enemyUnit.currentStamina >= enemyUnit.maxStamina)
            enemyUnit.currentStamina = enemyUnit.maxStamina;

        dialogueText.text = enemyUnit.unitName + " is now choosing.";

        yield return new WaitForSeconds(2f);

        //Enemy will run away
        if (enemyUnit.currentStamina <= 30 && Vector2.Distance(enemyUnit.transform.position, playerUnit.transform.position) <= 200)
        {
            dialogueText.text = enemyUnit.unitName + " is creating distance.";

            enemyUnit.currentStamina -= enemyUnit.attack1Stamina;
            enemyHUD.SetHealthAndStamina(enemyUnit.currentHP, enemyUnit.currentStamina);

            enemyUnit.transform.position = Vector2.Lerp(enemyUnit.transform.position, new Vector2(enemyUnit.transform.position.x + 100, enemyUnit.transform.position.y), 100);

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        } 
        //Enemy will rest
        else if( enemyUnit.currentStamina <= 30 && Vector2.Distance(enemyUnit.transform.position, playerUnit.transform.position) > 200)
        {
            dialogueText.text = enemyUnit.unitName + " is resting.";

            yield return new WaitForSeconds(2f);

            enemyUnit.currentStamina += enemyUnit.staminaAdd;
            enemyHUD.SetHealthAndStamina(enemyUnit.currentHP, enemyUnit.currentStamina);

            dialogueText.text = enemyUnit.unitName + "'s turn is over.";

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        //Enemy will hit you
        else
        {
            if (Vector2.Distance(enemyUnit.transform.position, playerUnit.transform.position) <= 200)
            {
                dialogueText.text = enemyUnit.unitName + " hits you!";
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
            else
            {
                dialogueText.text = enemyUnit.unitName + " is coming closer.";

                enemyUnit.transform.position = Vector2.Lerp(enemyUnit.transform.position, new Vector2(enemyUnit.transform.position.x - 100, enemyUnit.transform.position.y), 100);

                yield return new WaitForSeconds(2f);

                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
    }

    IEnumerator PlayerAttack1()
    {
        if (Vector2.Distance(playerUnit.transform.position, enemyUnit.transform.position) <= 200)
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
                    PlayerPrefs.SetInt("EnemiesDefeated", 1 + PlayerPrefs.GetInt("EnemiesDefeated"));
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
        else
        {
            dialogueText.text = "You are not close enough.\nTry getting closer.";

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator PlayerAttack2()
    {
        if (Vector2.Distance(playerUnit.transform.position, enemyUnit.transform.position) <= 200)
        {
            if (playerUnit.currentStamina >= playerUnit.attack2Stamina)
            {
                dialogueText.text = "You used KICK!";
                playerUnit.currentStamina -= playerUnit.attack2Stamina;
                playerHUD.SetHealthAndStamina(playerUnit.currentHP, playerUnit.currentStamina);

                yield return new WaitForSeconds(2f);

                bool isDead = enemyUnit.TakeDamage(playerUnit.attack2Damage);
                enemyHUD.SetHealthAndStamina(enemyUnit.currentHP, enemyUnit.currentStamina);

                dialogueText.text = "The attack is successful!";

                yield return new WaitForSeconds(2f);

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

                yield return new WaitForSeconds(2f);

                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
        else
        {
            dialogueText.text = "You are not close enough.\nTry getting closer.";

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator PlayerAddStamina()
    {
        dialogueText.text = "You are resting.";

        yield return new WaitForSeconds(2f);

        playerUnit.AddStamina(playerUnit.staminaAdd);
        dialogueText.text = "Your stamina is now " + playerUnit.currentStamina + ".";

        playerHUD.SetHealthAndStamina(playerUnit.currentHP, playerUnit.currentStamina);

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerHeal()
    {
        if (playerUnit.potion >= 1)
        {
            playerUnit.potion--;
            dialogueText.text = "You are healing.";

            yield return new WaitForSeconds(2f);

            playerUnit.AddHealth(playerUnit.healthAdd);
            dialogueText.text = "Your health is now " + playerUnit.currentHP + ".";

            playerHUD.SetHealthAndStamina(playerUnit.currentHP, playerUnit.currentStamina);

            yield return new WaitForSeconds(2f);

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

    IEnumerator WalkLeft()
    {
        if (Vector2.Distance(playerUnit.transform.position, enemyUnit.transform.position) > 600)
        {
            dialogueText.text = "You can't go any further.";

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        else
        {
            dialogueText.text = "You are creating distance.";

            playerUnit.transform.position = Vector2.Lerp(playerUnit.transform.position, new Vector2(playerUnit.transform.position.x - 100, playerUnit.transform.position.y), 100);

            yield return new WaitForSeconds(2f);

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator WalkRight()
    {
        if (Vector2.Distance(playerUnit.transform.position, enemyUnit.transform.position) <= 200)
        {
            dialogueText.text = "You can't get any closer.";

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        else
        {
            dialogueText.text = "You are coming closer.";

            playerUnit.transform.position = Vector2.Lerp(playerUnit.transform.position, new Vector2(playerUnit.transform.position.x + 100, playerUnit.transform.position.y), 100);

            yield return new WaitForSeconds(2f);

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EndBattle()
    {
        playerUnit.CheckPlayerPrefs();
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";

            yield return new WaitForSeconds(2f);

            dialogueText.text = "You earned 115 experience!";

            PlayerPrefs.SetFloat("CurrentExp", 115 + PlayerPrefs.GetFloat("CurrentExp"));

            while (PlayerPrefs.GetFloat("CurrentExp") >= 100)
            {
                PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
                PlayerPrefs.SetFloat("CurrentExp", PlayerPrefs.GetFloat("CurrentExp") - 100);
            }

            yield return new WaitForSeconds(1f);
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You lost the battle!";

            yield return new WaitForSeconds(2f);
        }

        dialogueText.text = "Returning to other scene";

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        tranAnim.SceneTransition();
    }

    void PlayerTurn()
    {
        playerUnit.CheckPlayerPrefs();
        dialogueText.text = "Choose an action.";
    }

    public void OnWalkLeftButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(WalkLeft());
    }

    public void OnWalkRightButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(WalkRight());
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
