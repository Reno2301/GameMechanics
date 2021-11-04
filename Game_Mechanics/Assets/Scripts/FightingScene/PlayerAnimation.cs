using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    FightingPlayerCombat playerCombat;
    FightingPlayerMovement playerMovement;

    Animator animator;
    private string currentState;

    //Animation states
    const string    PLAYER_IDLE     = "PlayerIde",
                    PLAYER_RUN      = "PlayerRun",
                    PLAYER_JUMP     = "PlayerJump",
                    PLAYER_ATTACK   = "PlayerAttack",
                    PLAYER_TAKE_HIT = "PlayerTakeHit",
                    PLAYER_DEATH    = "PlayerDeath";

    void Start()
    {
        playerCombat = GetComponent<FightingPlayerCombat>();
        playerMovement = GetComponent<FightingPlayerMovement>();
        animator = GetComponent<Animator>();
    }


    void ChangeAnimationState(string newState)
    {
        //don't interrupt the same animation
        if (currentState == newState) return;

        //play the new animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }

    private void Update()
    {
        if (playerMovement.rb.velocity.x != 0)
        {
            ChangeAnimationState(PLAYER_RUN);
        }
        else
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
    }
}
