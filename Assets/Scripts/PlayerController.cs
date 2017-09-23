using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    
    protected override void Update()
    {
       
    }

    protected override void ComputeVelocity()
    {

        Vector2 move = Vector2.zero;

        if (cs.hitstunLeft <= 0 && inv.useStunLeft <= 0 || (inv.useStunLeft > 0 && !grounded))
        {
            if (!grounded || !ma.attacking)
            {
                move.x = Input.GetAxisRaw("Horizontal");
                if (grounded && move.x != 0 && walkSoundTimer <= 0)
                {
                    walkSoundTimer = 0.35f;
                    SoundManager.PlaySound("mackWalkSound");
                }
            }

            // Crouching
            if (Input.GetAxisRaw("Vertical") == -1 && (grounded || crouching))
            {
                crouching = true;
                cs.currentSpeed = cs.crouchSpeed;
            }
            else
            {
                // Code so Mack can't stop crouching if something is above his head
                GameObject ch = transform.GetChild(0).gameObject;
                Vector2[] origins = new Vector2[5];
                RaycastHit2D[] hits = new RaycastHit2D[5];
                float xBaseOffset = -0.65f;
                float checkDistance = 0.409f;
                for (int i = 0; i < 5; i++)
                {
                    origins[i] = new Vector2(ch.transform.position.x + xBaseOffset, ch.transform.position.y + 1.19f);
                    xBaseOffset += 0.325f;
                    hits[i] = Physics2D.Raycast(origins[i], Vector2.up, checkDistance);
                    if (hits[i].collider != null)
                    {
                        break;
                    }
                    else if (i == 4)
                    {
                        crouching = false;
                        cs.currentSpeed = cs.maxSpeed;
                    }
                }
            }

            // Jumping
            if (Input.GetButtonDown("Jump") && grounded)
            {
                SoundManager.PlaySound("mackJumpSound");
                velocityY = cs.jumpTakeOffSpeed;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocityY > 0)
                {
                    velocityY = velocity.y * 0.5f;
                }
            }
        }
        velocityX = move.x * cs.currentSpeed;
    }

    public bool isGrounded()
    {
        return grounded;
    }

}