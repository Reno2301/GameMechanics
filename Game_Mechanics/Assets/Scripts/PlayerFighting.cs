using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighting : MonoBehaviour
{

    public float moveSpeed = 5;

    public Rigidbody2D playerRB;

    Vector2 movement;

    public void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
    }

    //FixedUpdate for the physics.
    void FixedUpdate()
    {
        playerRB.MovePosition(playerRB.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
