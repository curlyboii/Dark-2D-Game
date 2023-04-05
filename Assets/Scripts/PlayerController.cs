using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    public float Speed;
    
    public Transform groundCheckPos;
    bool isFasingLeft;
    Animator anim;
    TrailRenderer trailRen;

    #region Dash (variables)

    bool canDash, isDashing;
    float dashDirection;
    public float dashForce;
    public float waitTimeDash;
    public int dashAmount;
    int dashCounter;

    #endregion

    float xInput;


    #region Jump (variables)
    bool IsGrounded;
    public float jumpForce;
    public int jumpAmount;
    int jumpCounter;
    public float JumpRadius; //The JumpRadius variable determines the radius of the circle used to check if the player is touching the ground.
                             //It is used in the Physics2D.OverlapCircle method in the Update method to determine whether the player is grounded or not.
    public LayerMask WhatIsGround; // A LayerMask is a bitmask that stores information about which layers are included or excluded from certain operations,
                                   // such as raycasting or collision detection.
                                   //In many games, objects like the player character need to know whether they are standing on the ground or not.
                                   //One common way to achieve this is by using a LayerMask to mark certain layers in the scene as "ground" layers,
                                   //and then using a raycast to detect when the player is in contact with an object on one of those layers.
    #endregion

    #region Wall Jump variables

    bool canGrab;
    bool isGrabbing;
    public float wallJumpRadius;
    float scaleX;
    public Transform wallJumpCheckPos;
    float initalGravityScale;
    public float wallJumpGravity;
    public float wallJumpForceX, wallJumpForceY;

    //Timer wall jumping
    public float startWallJumpTimer;
    float wallJumpTimer;

    #endregion

    public GameObject DeathEffect;
    private float jumpInput;



    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trailRen = GetComponent<TrailRenderer>();
        canDash = true;
        trailRen.emitting = false;
        dashCounter = dashAmount;

        initalGravityScale = rb.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        #region Jump and a bit dash (isGrounded)
        IsGrounded = Physics2D.OverlapCircle(groundCheckPos.position, JumpRadius, WhatIsGround); // Physics2D.OverlapCircle method, The method checks if the circle
                                                                                                 // overlaps with any object in the WhatIsGround layer,
                                                                                                 // which is set in the Inspector. If there is a collision,
                                                                                                 // the IsGrounded variable is set to true.
        if (IsGrounded)
        {

            jumpCounter = jumpAmount;
            dashCounter = dashAmount;

        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCounter > 0)
        {

            rb.velocity = Vector2.up * jumpForce;
            if (!IsGrounded)
            {
                jumpCounter--;
            }
        }

        #endregion

        #region Dash

        if (Input.GetKeyDown(KeyCode.Q) && canDash && dashCounter > 0) //checks if the "Q" key is pressed and if the player can currently dash.    
        {

            // dash
            dashDirection = Input.GetAxisRaw("Horizontal"); // gets the value of the horizontal axis input, which will be used to determine the direction of the dash
            if (dashDirection == 0) // checks if the dashDirection variable is equal to zero.
                                    // This will happen if the player is not providing any input in the horizontal axis. If this is the case,
                                    // the direction of the dash is set to the player's facing direction by getting the x component of the transform.localScale vector.
            {
                dashDirection = Mathf.Clamp(transform.localScale.x, -1, 1); //If the player is not providing any input in the horizontal axis,
                                                                            //dashDirection will be set to 0. However, if the player is facing left and hits the "Q" key to dash,
                                                                            //we want the player to dash in the left direction, so we need to set dashDirection to -1.
                                                                            //Similarly, if the player is facing right, we want the player to dash in the right direction,
                                                                            //so we need to set dashDirection to 1.
            }
            dashCounter--;
            isDashing = true; //sets the isDashing boolean variable to true
            canDash = false; // sets the canDash boolean variable to false to prevent dashing until the cooldown period has elapsed

            StartCoroutine(stopDashing()); //starts a coroutine called stopDashing()


        }

        if (isDashing) // checks if the player is currently dashing. If this is true, the following code is executed
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; // By setting rb.constraints to RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation, both of these constraints are applied to the rigidbody,
                                                                                                             // meaning that the player cannot move vertically or rotate while dashing. This can be useful for maintaining the direction and trajectory of the dash without
                                                                                                             // any unexpected changes due to gravity or rotation.
            rb.velocity = new Vector2(dashDirection * dashForce, rb.velocity.y); //sets the rigidbody's velocity to move the player in the direction
            trailRen.emitting = true;                                                                                //of the dash. The dashDirection * dashForce part calculates the speed and direction of the dash.
                                                                                                                     //rb.velocity.y preserves the vertical velocity of the rigidbody. 
        }

        #endregion
    }

    void FixedUpdate()
    {

        #region Movement and Fasing (Left and right)

        if(wallJumpTimer <= 0) //timer, If it has, then the player has finished wall jumping and the code enters the if block.
                               //Inside the if block, there's another if statement that checks if the player is not currently dashing.
                               //If the player is not dashing, then the code retrieves the horizontal input from the player using Input.GetAxisRaw("Horizontal").
        {
            if (!isDashing) // Not dashing
            {
                xInput = Input.GetAxisRaw("Horizontal"); // right arrow xInput = 1 and left xInput = -1, nothing = 0
                rb.velocity = new Vector2(xInput * Speed, rb.velocity.y) ; // speed rigidbody
            }

        }
        else //Inside the else block, the wallJumpTimer variable is decremented by Time.deltaTime.
             //This ensures that the player cannot wall jump indefinitely and there is a limited time frame for the wall jump to be executed.
        {

            wallJumpTimer -= Time.deltaTime;

        }

        if (xInput > 0 && isFasingLeft == true)
        {
            Flip();
            //flip the player
        }
        else if (xInput < 0 && isFasingLeft == false)
        {
            Flip();
            //flip the player
        }
        #endregion

        
        #region Animation

        if (xInput != 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {

            anim.SetBool("isWalking", false);

        }

        // instead of if(!IsGrounded){ anim.SetBool("hasJumped", true);} else {anim.SetBool("hasJumped", false);}
        anim.SetBool("hasJumped", !IsGrounded);

        #endregion


        #region Wall Jump

        canGrab = Physics2D.OverlapCircle(wallJumpCheckPos.position, wallJumpRadius, WhatIsGround);

        isGrabbing = false;
        if(!IsGrounded && canGrab)
        {
            scaleX = transform.localScale.x; //float clampedScale = Mathf.Clamp(transform.position.x, -1, 1);

            if ((scaleX > 0 && xInput > 0) || (scaleX < 0 && xInput < 0))
            {
                isGrabbing = true;
                if(Input.GetKeyDown(KeyCode.Space)) 
                {
                    //Jump
                    wallJumpTimer = startWallJumpTimer;
                    rb.velocity = new Vector2(-xInput * wallJumpForceX, wallJumpForceY);

                    rb.gravityScale = initalGravityScale;
                    isGrabbing = false;
                }
            }

        }

        if(isGrabbing)
        {
            rb.gravityScale = wallJumpGravity;
            rb.velocity = Vector2.zero;

        }
        else
        {
            rb.gravityScale = initalGravityScale;

        }
        #endregion
    }

    #region Flip
    /// <summary>
    /// The ! operator negates the current value of the boolean variable, which means that if isFasingLeft was true,
    /// it becomes false, and vice versa. So, if isFasingLeft was true, 
    /// !isFasingLeft evaluates to false, which means that isFasingLeft is set to false. 
    /// Similarly, if isFasingLeft was false, !isFasingLeft evaluates to true, which means that isFasingLeft is set to true.
    /// </summary>
    void Flip()
    {
        isFasingLeft = !isFasingLeft;   
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
    #endregion


    /// <summary>
    /// This IEnumerator method stopDashing() waits for a specified amount of time using the WaitForSeconds() method,
    /// and then sets canDash back to true and isDashing to false, which allows the player to dash again
    /// </summary>
    /// <returns></returns>
    IEnumerator stopDashing()
    {
        yield return new WaitForSeconds(waitTimeDash);
        rb.constraints = ~RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        canDash = true;
        isDashing = false;
        trailRen.emitting = false;

    }




    #region Draw radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(groundCheckPos.position, JumpRadius); // see the radius
        Gizmos.DrawSphere(wallJumpCheckPos.position, wallJumpRadius); // see the radius
    }
    #endregion


}
