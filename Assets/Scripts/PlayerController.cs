using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    public float Speed;
    
    public Transform groundCheckPos;
    bool isFasingLeft;
    Animator anim;
    TrailRenderer trailRen;

    bool canDash, isDashing;
    float dashDirection;
    public float dashForce;
    public float waitTimeDash;
    public int dashAmount;
    int dashCounter;

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




    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trailRen = GetComponent<TrailRenderer>();
        canDash = true;
        trailRen.emitting = false;
        dashCounter = dashAmount;
        

    }

// Update is called once per frame
    void Update()
    {

        #region Movement and Fasing (Left and right)
        float xInput = Input.GetAxisRaw("Horizontal"); // right arrow xInput = 1 and left xInput = -1, nothing = 0
        rb.velocity = new Vector2(xInput * Speed * Time.deltaTime, rb.velocity.y); // speed rigidbody

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


        #region Restart scene R key
        if (Input.GetKeyDown(KeyCode.R))
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        }
        #endregion

        #region Dash

        if (Input.GetKeyDown(KeyCode.Q) && canDash && dashCounter > 0) //checks if the "Q" key is pressed and if the player can currently dash.    
        {
            // dash
            dashCounter--;
            isDashing = true; //sets the isDashing boolean variable to true
            canDash = false; // sets the canDash boolean variable to false to prevent dashing until the cooldown period has elapsed
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; // By setting rb.constraints to RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation, both of these constraints are applied to the rigidbody,
                                                                                                             // meaning that the player cannot move vertically or rotate while dashing. This can be useful for maintaining the direction and trajectory of the dash without
                                                                                                             // any unexpected changes due to gravity or rotation.
            StartCoroutine(stopDashing()); //starts a coroutine called stopDashing()

        }

        if (isDashing) // checks if the player is currently dashing. If this is true, the following code is executed
        {
            dashDirection = Input.GetAxisRaw("Horizontal"); // gets the value of the horizontal axis input, which will be used to determine the direction of the dash

            if (dashDirection == 0) // checks if the dashDirection variable is equal to zero.
                                        // This will happen if the player is not providing any input in the horizontal axis. If this is the case,
                                        // the direction of the dash is set to the player's facing direction by getting the x component of the transform.localScale vector.
            {
                dashDirection = transform.localScale.x;
            }

            rb.velocity = new Vector2(dashDirection * dashForce, rb.velocity.y) * Time.deltaTime; //sets the rigidbody's velocity to move the player in the direction
            trailRen.emitting = true;                                                                                //of the dash. The dashDirection * dashForce part calculates the speed and direction of the dash.
                                                                                                  //rb.velocity.y preserves the vertical velocity of the rigidbody. 
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
    }
    #endregion
}
