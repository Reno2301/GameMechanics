using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingPlayerMovement : MonoBehaviour
{

    public float moveSpeed = 10;
    public float jumpSpeed = 10;

    public Rigidbody2D rb;
    public Animator anim;

    Vector2 movement;

    string previousButton;
    string currentButton;

    public void Update()
    {
        Move();
        LookingDirection();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
        }


        //Mathf.Abs so it will always be positive, the animator needs a positive number.
        anim.SetFloat("Speed", Mathf.Abs(movement.x));
    }

    //FixedUpdate for the physics.
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed/2) * Time.fixedDeltaTime);
    }

    private void Move()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
    }

    private void LookingDirection()
    {
        //if the last button pressed is a (left) then the player will look to the left.
        //otherwise the player will look right.

        if (previousButton == "a")
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey("a"))
        {
            transform.localScale = new Vector3(-1, 1, 1);
            currentButton = "a";
        }
        if (Input.GetKey("d"))
        {
            transform.localScale = new Vector3(1, 1, 1);
            currentButton = "d";
        }

        previousButton = currentButton;
    }
}
