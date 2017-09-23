using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public PhysicsMaterial2D playerMat;

    private bool moveable { get; set; }
    private bool crouching = false;

    private float maxForce = 2500f;
    private float constantSpeedForce;
    private float stoppingForce;
    private float runningSpeed = 10;
    //private float crouchSpeed = 5;
    private float currentForce;
    
    private float jumpTakeOffSpeed = 15;

    private KeyCode left, right, crouch, jump;

    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 move;

    private enum MovementState { idle, dashing, running, stopping }
    private MovementState state = MovementState.idle;

    private int dashFrames;

    void Start()
    {
        Application.targetFrameRate = 60;
        Physics2D.gravity = new Vector3(0, -10, 0);

        if (name == "Player1")
        {
            left = KeyCode.A;
            right = KeyCode.D;
            crouch = KeyCode.S;
            jump = KeyCode.W;
        }
        else if (name == "Player2")
        {
            left = KeyCode.LeftArrow;
            right = KeyCode.RightArrow;
            crouch = KeyCode.DownArrow;
            jump = KeyCode.UpArrow;
        }

        moveable = true;

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        constantSpeedForce = Mathf.Abs(playerMat.friction * rb2d.mass * Physics2D.gravity.y);
        stoppingForce = 2 * constantSpeedForce;

    }

    void Update()
    {

        move = new Vector2();
        
        switch (state)
        {

            case MovementState.idle:
                Debug.Log("idle");
                anim.SetBool("idle", true);
                anim.SetBool("dashing", false);
                anim.SetBool("running", false);
                anim.SetBool("stopping", false);

                currentForce = 0;

                // Want to move dash
                if (moveable && (Input.GetKey(left) || Input.GetKey(right)))
                {
                    dashFrames = 0;
                    state = MovementState.dashing;
                }

                break;

            case MovementState.dashing:
                Debug.Log("dashing");
                anim.SetBool("idle", false);
                anim.SetBool("dashing", true);
                anim.SetBool("running", false);
                anim.SetBool("stopping", false);

                if (moveable)
                {
                    // Want to dash left
                    if (Input.GetKey(left) /*&& !Input.GetKey(right)*/)
                    {
                        Debug.Log("dashFrames: " + dashFrames);
                        // If we are moving to the right and slam left or we are already moving left
                        if (rb2d.velocity.x <= 0)
                        {
                            rb2d.velocity = new Vector2(-runningSpeed, 0);
                        }
                        else if (rb2d.velocity.x > 0 && dashFrames < 20)
                        {
                            sr.flipX = true;
                            dashFrames = 0;
                            rb2d.velocity = new Vector2(-runningSpeed, 0);
                        }
                        if (dashFrames >= 20)
                        {
                            Debug.Log("changed to running because ran out of frames");
                            state = MovementState.running;
                        }
                    }
                    // Want to dash right
                    else if (Input.GetKey(right) /*&& !Input.GetKey(left)*/)
                    {
                        Debug.Log("dashFrames: " + dashFrames);
                        // If we are moving left and slam right or we are already moving right
                        if (rb2d.velocity.x >= 0)
                        {
                            rb2d.velocity = new Vector2(runningSpeed, 0);
                        }
                        else if (rb2d.velocity.x < 0 && dashFrames < 20)
                        {
                            dashFrames = 0;
                            sr.flipX = false;
                            rb2d.velocity = new Vector2(runningSpeed, 0);
                        }
                        if (dashFrames >= 20)
                        {
                            Debug.Log("changed to running because ran out of frames");
                            state = MovementState.running;
                        }
                    }
                    else
                    {
                        state = MovementState.stopping;
                    }
                    dashFrames++;
                }

                break;

            case MovementState.running:
                Debug.Log("running");
                anim.SetBool("idle", false);
                anim.SetBool("dashing", false);
                anim.SetBool("running", true);
                anim.SetBool("stopping", false);

                if (rb2d.velocity.x < 0 && Input.GetKey(left))
                {
                    currentForce = -constantSpeedForce;
                }
                else if (rb2d.velocity.x > 0 && Input.GetKey(right))
                {
                    currentForce = constantSpeedForce;
                }
                else
                {
                    state = MovementState.stopping;
                }

                break;

            case MovementState.stopping:
                Debug.Log("stopping");
                anim.SetBool("idle", false);
                anim.SetBool("dashing", false);
                anim.SetBool("running", false);
                anim.SetBool("stopping", true);

                // If we are going left
                if (rb2d.velocity.x < 0)
                {
                    currentForce = stoppingForce;
                } 
                else
                {
                    currentForce = -stoppingForce;
                }

                if (Mathf.Abs(rb2d.velocity.x) <= 0.5f)
                {
                    currentForce = 0;
                    state = MovementState.idle;
                }

                break;

        }

        Debug.Log("currentForce: " + currentForce);
        move = new Vector2(currentForce, 0);
        Debug.Log("velocity: " + rb2d.velocity);

    }

    void FixedUpdate()
    {
        rb2d.AddForce(move);
    }

}