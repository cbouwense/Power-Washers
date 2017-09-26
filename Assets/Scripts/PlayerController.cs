using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public PhysicsMaterial2D playerMat;

    public Camera camera;
    private bool moveable { get; set; }
    private bool jumpable;
    [SerializeField] private bool grounded;
    private int jumps = 2;

    private float normalMass = 40;
    private float fastMass = 80;

    private float constantSpeedForce, stoppingForce;
    private float airForce = 900;
    private float runningSpeed = 5;
    private float firstJumpSpeed = 6.35f;
    private float secondJumpSpeed = 6f;
    private float currentXForce, currentYForce;
    
    private bool justJumped;

    private string horizontal, vertical, jump;

    private Rigidbody2D rb2d;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 move;

    private enum HState { idle, dashing, running, stopping }
    private enum VState { grounded, squatting, air, landing }
    private HState hState = HState.idle;
    private VState vState = VState.grounded;

    public bool onMess = false;
    public bool cleaning = false;
    public float cleaned = 0;

    private int dashFrames, squatFrames, landingFrames;

    private void Start()
    {
        Application.targetFrameRate = 60;
        Physics2D.gravity = new Vector3(0, -10, 0);

        // Input.GetAxisRaw("Horizontal_P1")
        // Input.GetAxisRaw("Vertical_P1")
        // Input.GetAxisRaw("Horizontal_P2")
        // Input.GetAxisRaw("Vertical_P2")
        if (name == "Player1")
        {
            horizontal = "Horizontal_P1";
            vertical = "Vertical_P1";
            jump = "Jump_P1";
        }
        else if (name == "Player2")
        {
            horizontal = "Horizontal_P2";
            vertical = "Vertical_P2";
            jump = "Jump_P2";
        }

        moveable = true;
        jumpable = true;

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        constantSpeedForce = Mathf.Abs(playerMat.friction * rb2d.mass * Physics2D.gravity.y);
        stoppingForce = 2 * constantSpeedForce;

    }

    private void Update()
    {

        if (transform.position.x < -10.33f ||
            transform.position.x > 22.32 ||
            transform.position.y < camera.transform.position.y - 9.42)
        {
            Respawn();
        }

        // Vector of wanted movement
        move = new Vector2();

        // Components of move
        currentXForce = 0;
        currentYForce = 0;

        // Update value of grounded
        grounded = isGrounded();
        if (!moveable)
        {
            moveable = grounded;
        }

        // Reset animations
        resetAnim();

        
        if (moveable)
        {
            // Horizontal movement state machine
            switch (hState)
            {

                case HState.idle:
                    //Debug.Log("idle");
                    changeAnim("idle");

                    currentXForce = 0;

                    // Want to move
                    if (moveable && Input.GetAxisRaw(horizontal) != 0)
                    {
                        if (grounded)
                        {
                            dashFrames = 0;
                            hState = HState.dashing;
                        }
                        else
                        {
                            hState = HState.running;
                        }
                    }

                    break;

                case HState.dashing:
                    //Debug.Log("dashing");
                    if (!cleaning)
                    {
                        changeAnim("dashing");
                    }
                    else
                    {
                        changeAnim("cleaning");
                    }

                    if (moveable)
                    {
                        // Want to dash left
                        Debug.Log("horizontal: " + Input.GetAxisRaw(horizontal));
                        if (Input.GetAxisRaw(horizontal) == -1)
                        {
                            // If we are moving to the right and slam left
                            if (rb2d.velocity.x <= 0)
                            {
                                rb2d.velocity = new Vector2(-runningSpeed, rb2d.velocity.y);
                            }
                            else if (rb2d.velocity.x > 0 && dashFrames < 20)
                            {
                                dashFrames = 0;
                                rb2d.velocity = new Vector2(-runningSpeed, rb2d.velocity.y);
                            }

                            if (dashFrames >= 20 || !grounded)
                            {
                                hState = HState.running;
                            }
                        }
                        // Want to dash right
                        else if (Input.GetAxisRaw(horizontal) == 1)
                        {
                            // If we are moving left and slam right
                            if (rb2d.velocity.x >= 0)
                            {
                                rb2d.velocity = new Vector2(runningSpeed, rb2d.velocity.y);
                            }
                            else if (rb2d.velocity.x < 0 && dashFrames < 20)
                            {
                                dashFrames = 0;
                                rb2d.velocity = new Vector2(runningSpeed, rb2d.velocity.y);
                            }

                            if (dashFrames >= 20 || !grounded)
                            {
                                hState = HState.running;
                            }
                        }
                        else
                        {
                            hState = HState.stopping;
                        }
                        dashFrames++;
                    }

                    break;

                case HState.running:
                    //Debug.Log("running");
                    changeAnim("running");

                    if (grounded)
                    {
                        if (rb2d.velocity.x < 0 && Input.GetAxisRaw(horizontal) == -1)
                        {
                            currentXForce = -constantSpeedForce;
                        }
                        else if (rb2d.velocity.x > 0 && Input.GetAxisRaw(horizontal) == 1)
                        {
                            currentXForce = constantSpeedForce;
                        }
                        else
                        {
                            hState = HState.stopping;
                        }
                    }
                    else
                    {
                        // Moving left normally
                        if (rb2d.velocity.x <= 0 && Input.GetAxisRaw(horizontal) == -1 && rb2d.velocity.x >= -runningSpeed ||
                            rb2d.velocity.x > 0 && Input.GetAxisRaw(horizontal) == -1)
                        {
                            currentXForce = -airForce;
                        }
                        // Moving right normally
                        else if (rb2d.velocity.x >= 0 && Input.GetAxisRaw(horizontal) == 1 && rb2d.velocity.x <= runningSpeed ||
                            rb2d.velocity.x < 0 && Input.GetAxisRaw(horizontal) == 1)
                        {
                            currentXForce = airForce;
                        }

                    }

                    break;

                case HState.stopping:
                    //Debug.Log("stopping");
                    changeAnim("stopping");

                    // If we are going left
                    if (rb2d.velocity.x < 0)
                    {
                        currentXForce = stoppingForce;
                    }
                    else
                    {
                        currentXForce = -stoppingForce;
                    }

                    if (Mathf.Abs(rb2d.velocity.x) <= 0.5f)
                    {
                        currentXForce = 0;
                        hState = HState.idle;
                    }

                    break;

            }

            // Vertical movement state machine
            switch (vState)
            {
            
                case VState.grounded:
                    //Debug.Log("grounded");
                    changeAnim("grounded");

                    jumps = 2;

                    Debug.Log("GetAxisRaw(jump) " + name + ": " + Input.GetAxisRaw(jump));
                    if (Input.GetAxisRaw(jump) == 1 && jumpable)
                    {
                        jumps--;
                        jumpable = false;
                        squatFrames = 0;
                        vState = VState.squatting;
                        // Have to do this because no intentional fallthrough in C#
                        goto case VState.squatting;
                    }

                    if (rb2d.velocity.y < -0.001)
                    {
                        vState = VState.air;
                    }

                    break;

                case VState.squatting:
                    //Debug.Log("squatting");
                    changeAnim("squatting");

                    if (squatFrames < 4)
                    {
                        squatFrames++;
                    }
                    else
                    {
                        rb2d.velocity = new Vector2(rb2d.velocity.x, firstJumpSpeed);
                        jumpable = false;
                        justJumped = true;
                        vState = VState.air;
                    }

                    break;

                case VState.air:
                    //Debug.Log("air");
                    if (rb2d.velocity.y > 0)
                    {
                        changeAnim("air");
                    }
                    else
                    {
                        changeAnim("falling");
                    }

                    // Double jump
                    if (Input.GetAxisRaw(jump) == 1 && jumpable)
                    {
                        changeAnim("squatting");

                        // Snap from left to right
                        if (rb2d.velocity.x > 0 && Input.GetAxisRaw(horizontal) == -1)
                        {
                            rb2d.velocity = new Vector2(-runningSpeed, secondJumpSpeed);
                        }
                        // Snap from right to left
                        else if (rb2d.velocity.x < 0 && Input.GetAxisRaw(horizontal) == 1)
                        {
                            rb2d.velocity = new Vector2(runningSpeed, secondJumpSpeed);
                        }
                        else
                        {
                            rb2d.velocity = new Vector2(rb2d.velocity.x, secondJumpSpeed);
                        }
                        jumpable = false;
                        jumps--;
                    }

                    if (grounded && !justJumped)
                    {
                        landingFrames = 0;
                        vState = VState.landing;
                    }

                    justJumped = false;

                    break;

                case VState.landing:
                    //Debug.Log("landing");
                    changeAnim("landing");

                    if (landingFrames < 4)
                    {
                        landingFrames++;
                    }
                    else
                    {
                        vState = VState.grounded;
                    }

                    break;

            }
        }

        // Fast falling
        Debug.Log("GetAxisRaw(vertical) " + name + ": " + Input.GetAxisRaw(vertical));
        if (Input.GetAxisRaw(vertical) == -1 && !grounded)
        {
            rb2d.gravityScale = 5;
        }
        else
        {
            rb2d.gravityScale = 1;
        }

        // Reset jumpability if you should be able to jump
        if (Input.GetAxisRaw(vertical) == 0 && jumps > 0 ||
            isGrounded())
        {
            jumpable = true;
        }

        // Make character face way you want to go
        if (!cleaning)
        {
            if (Input.GetAxisRaw(horizontal) == -1)
            {
                sr.flipX = true;
            }
            else if (Input.GetAxisRaw(horizontal) == 1)
            {
                sr.flipX = false;
            }
        }

        // Update value of cleaning
        cleaning = (onMess && hState == HState.dashing);

        // Set the components of move from the state machines
        move = new Vector2(currentXForce, currentYForce);

    }

    private void FixedUpdate()
    {
        rb2d.AddForce(move);
    }

    private bool isGrounded()
    {
        Vector2[] origins = new Vector2[3];
        RaycastHit2D[] hits = new RaycastHit2D[3];
        float xBaseOffset = -0.1f;
        float yBaseOffset = -0.66f;
        float checkDistance = 0.01f;
        for (int i = 0; i < origins.Length; i++)
        {
            origins[i] = new Vector2(transform.position.x + xBaseOffset, transform.position.y + yBaseOffset);
            //Debug.DrawLine(origins[i], new Vector2(origins[i].x, origins[i].y - checkDistance));
            xBaseOffset += 0.1f;
            hits[i] = Physics2D.Raycast(origins[i], Vector2.up, checkDistance);
            if (hits[i].collider != null)
            {
                return true;
            }
        }
        return false;
    }

    private void changeAnim(string state)
    {
        string[] states = {"idle", "dashing", "running", "stopping",
                           "grounded", "squatting", "air", "falling", "landing",
                           "cleaning", "respawn"};
        for (int i = 0; i < states.Length; i++)
        {
            if (state == states[i])
            {
                anim.SetBool(states[i], true);
            }
        }
    }

    private void resetAnim()
    {
        string[] states = {"idle", "dashing", "running", "stopping",
                           "grounded", "squatting", "air", "falling", "landing",
                           "cleaning"};
        for (int i = 0; i < states.Length; i++)
        {
            anim.SetBool(states[i], false);
        }
    }

    public bool isDashing()
    {
        return hState == HState.dashing;
    }

    public void Clean()
    {
        cleaned++;
    }

    public void Respawn()
    {
        Debug.Log("Respawning");
        resetAnim();
        anim.SetBool("respawn", true);
        moveable = false;
        rb2d.velocity = new Vector2();
        if (name == "Player1")
        {
            transform.position = new Vector2(-2.38f, camera.transform.position.y + 1);
        }
        else
        {
            transform.position = new Vector2(14.13f, camera.transform.position.y + 1);
        }
    }

}