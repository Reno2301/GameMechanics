using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Leveling
    private LevelSystem levelSystem;
    private float maxLevel = 10;

    //Movement
    public bool moveable;
    public float moveSpeed = 5f;

    //Stats
    public int attack1Damage;
    public int attack2Damage;
    public int attack1Stamina;
    public int attack2Stamina;
    public int maxStamina;
    public int maxHealth;

    //Animation
    private Animator animator;

    //Collision
    public LayerMask tileCollision;

    LevelWindow levelWindow;
    Transform movePoint;

    private void Awake()
    {
        levelWindow = GameObject.Find("LevelWindow").GetComponent<LevelWindow>();
        movePoint = GameObject.Find("MovePoint").GetComponent<Transform>();
    }

    void SetDefaultStats()
    {
        if (levelSystem.level == 0)
        {
            PlayerPrefs.SetInt("attack1Damage", 7);
            PlayerPrefs.SetInt("attack2Damage", 14);
            PlayerPrefs.SetInt("maxStamina", 100);
            PlayerPrefs.SetInt("maxHealth", 100);
        }
    }

    public void Start()
    {
        LevelSystem levelSystem = new LevelSystem();

        levelWindow.SetLevelSystem(levelSystem);
        this.SetLevelSystem(levelSystem);

        SetDefaultStats();

        //Get the stats from PlayerPrefs
        attack1Damage = PlayerPrefs.GetInt("attack1Damage");
        attack2Damage = PlayerPrefs.GetInt("attack2Damage");
        maxStamina = PlayerPrefs.GetInt("maxStamina");
        maxHealth = PlayerPrefs.GetInt("maxHealth");

        animator = GetComponent<Animator>();

        movePoint.position = transform.position;

        for (int i = 0; i < maxLevel; i++)
        {
            if (PlayerPrefs.GetInt("CurrentLevel") == i)
            {
                Debug.Log("Level: " + (PlayerPrefs.GetInt("CurrentLevel") + 1));
                Debug.Log("Attack 1: " + attack1Damage);
                Debug.Log("Attack 2: " + attack2Damage);
                Debug.Log("Stamina: " + maxStamina);
                Debug.Log("Health: " + maxHealth);
            }
        }
    }

    //set levelSystem
    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;

        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < maxLevel; i++)
        {
            if (levelSystem.level == i)
            {
                PlayerPrefs.SetInt("attack1Damage", (7 + i));
                attack1Damage = PlayerPrefs.GetInt("attack1Damage");

                PlayerPrefs.SetInt("attack2Damage", attack1Damage * i);
                PlayerPrefs.SetInt("maxStamina", 100 + i * 5);
                PlayerPrefs.SetInt("maxHealth", 100 + i * 5);

                attack2Damage = PlayerPrefs.GetInt("attack2Damage");
                maxStamina = PlayerPrefs.GetInt("maxStamina");
                maxHealth = PlayerPrefs.GetInt("maxHealth");
            }
        }
        Debug.Log("Level: " + (levelSystem.level + 1));
        Debug.Log("Attack 1: " + attack1Damage);
        Debug.Log("Attack 2: " + attack2Damage);
        Debug.Log("Stamina: " + maxStamina);
        Debug.Log("Health: " + maxHealth);
    }

    public void Update()
    {
        Moveable();
        Movement();
        Animation();
    }

    private void Moveable()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Fighting1"))
            moveable = false;
        else
            moveable = true;
    }

    //Player movement
    public void Movement()
    {
        if (moveable)
        {
            //Player move towards movePoint
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .1f, tileCollision))
                    {
                        movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    }
                }
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .1f, tileCollision))
                    {
                        movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    }
                }
            }
        }
    }

    //Player animation
    public void Animation()
    {
        if (moveable)
        {
            if (Input.GetKey(KeyCode.D))
                animator.Play("Main_Player_Walk_Right");
            else if (Input.GetKey(KeyCode.A))
                animator.Play("Main_Player_Walk_Left");
            else if (Input.GetKey(KeyCode.W))
                animator.Play("Main_Player_Walk_Up");
            else if (Input.GetKey(KeyCode.S))
                animator.Play("Main_Player_Walk_Down");
            else
                animator.Play("Main_Player_Idle");
        }
    }

    public void PlayerAddExperience(int exp)
    {
        levelSystem.AddExperience(exp);
    }
}
