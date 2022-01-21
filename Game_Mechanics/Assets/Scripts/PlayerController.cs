using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{


    //Leveling
    private LevelSystem levelSystem;
    private float maxLevel = 10;

    //Animation
    private Animator animator;

    //Movement
    public float moveSpeed = 5f;

    //Collision
    public Transform movePoint;
    public LayerMask tileCollision;

    //Stats
    public int attack1Damage;
    public int attack2Damage;
    public int stamina;
    public int health;

    LevelWindow levelWindow;
    PlayerController player;

    private void Awake()
    {
        levelWindow = GameObject.Find("LevelWindow").GetComponent<LevelWindow>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        LevelSystem levelSystem = new LevelSystem();
        levelWindow.SetLevelSystem(levelSystem);
        player.SetLevelSystem(levelSystem);

        animator = GetComponent<Animator>();

        movePoint.position = transform.position;

        attack1Damage = 10;
        attack2Damage = attack1Damage * 2;
        stamina = 100;
        health = 100;

        for (int i = 0; i < maxLevel; i++)
        {
            if (PlayerPrefs.GetInt("CurrentLevel") == i)
            {
                Debug.Log("Level: " + (PlayerPrefs.GetInt("CurrentLevel")+1));
                Debug.Log("Attack 1: " + attack1Damage);
                Debug.Log("Attack 2: " + attack2Damage);
                Debug.Log("Stamina: " + stamina);
                Debug.Log("Health: " + health);
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
                attack1Damage = 5 + i;
                attack2Damage = attack1Damage * 2;

                stamina = 100 + i * 15;
                health = 100 + i * 15;
            }
        }
        Debug.Log("Level: " + (levelSystem.level + 1));
        Debug.Log("Attack 1: " + attack1Damage);
        Debug.Log("Attack 2: " + attack2Damage);
        Debug.Log("Stamina: " + stamina);
        Debug.Log("Health: " + health);
    }

    private void Update()
    {
        Movement();
        Animation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            levelSystem.AddExperience(40);
        }
    }

    //Player movement
    public void Movement()
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

    //Player animation
    public void Animation()
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
