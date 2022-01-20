using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldFightingPlayerMovement : MonoBehaviour
{

    public float moveSpeed = 10;

    public Rigidbody2D rb;
    public Animator anim;

    Vector2 movement;

    string previousButton;
    string currentButton;

    public void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");

        //if the last button pressed is a (left) then the player will look to the left.
        //otherwise the player will look right.
        if(previousButton == "a")
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

        //Mathf.Abs so it will always be positive, the animator needs a positive number.
        anim.SetFloat("Speed", Mathf.Abs(movement.x));
    }

    //FixedUpdate for the physics.
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed/2) * Time.fixedDeltaTime);
    }
}
