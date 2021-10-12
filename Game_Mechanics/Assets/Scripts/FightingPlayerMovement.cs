using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingPlayerMovement : MonoBehaviour
{

    public float moveSpeed = 8;

    public Rigidbody2D rb;
    public Animator anim;

    Vector2 movement;

    public void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey("a"))
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        //Mathf.Abs so it will always be positive, the animator needs a positive number.
        anim.SetFloat("Speed", Mathf.Abs(movement.x));
    }

    //FixedUpdate for the physics.
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
