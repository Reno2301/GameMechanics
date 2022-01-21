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
    [SerializeField] Animator levelUpAnimator;
    private Animator animator;

    //Movement
    public float moveSpeed = 5f;

    //Collision
    public Transform movePoint;
    public LayerMask tileCollision;

    private void Start()
    {
        animator = GetComponent<Animator>();

        movePoint.position = transform.position;
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
                Debug.Log(i);

                //attack1Damage = 10 + i*2
                //attack2Damage = attack1Damage * 2

                //stamina = 100 + i*20
                //health = 100 + i*20
            }
        }
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
