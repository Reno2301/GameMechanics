using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingPlayerMovement : MonoBehaviour
{
    public float xAxis;
    public float yAxis;
    public float moveSpeed = 5;
    public float jumpForce;

    public bool isJumpPressed;
    public bool isGrounded;

    public Rigidbody2D rb;
    public Animator anim;
    public LayerMask groundMask;

    Vector2 movement;

    string previousButton;
    string currentButton;

    public void Update()
    {
        LookingDirection();

        xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPressed = true;
        }
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

    private void FixedUpdate()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundMask);

        if(hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        //==============================================

        Vector2 vel = new Vector2(0, rb.velocity.y);

        if(xAxis < 0)
        {
            vel.x = -moveSpeed;
        } 
        else if (xAxis > 0)
        {
            vel.x = moveSpeed;
        } 
        else
        {
            vel.x = 0;
        }

        //==============================================

        if (isJumpPressed && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isJumpPressed = false;
        }

        rb.velocity = vel;
    }
}
