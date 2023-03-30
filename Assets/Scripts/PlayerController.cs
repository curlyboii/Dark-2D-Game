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

    bool canDash, isDashing;
    float dashDirection;
    public float dashForce;
    public float waitTimeDash;

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
        canDash = true;
        

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

        #region Jump
        IsGrounded = Physics2D.OverlapCircle(groundCheckPos.position, JumpRadius, WhatIsGround); // Physics2D.OverlapCircle method, The method checks if the circle
                                                                                                 // overlaps with any object in the WhatIsGround layer,
                                                                                                 // which is set in the Inspector. If there is a collision,
                                                                                                 // the IsGrounded variable is set to true.
        if (IsGrounded)
        {

            jumpCounter = jumpAmount;
          
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

        if (Input.GetKeyDown(KeyCode.Q) && canDash) //checks if the "Q" key is pressed and if the player can currently dash.    
        {
            // dash
            isDashing = true;
            canDash = false;
            StartCoroutine(stopDashing());
        }

        if (isDashing)
        {
            dashDirection = Input.GetAxisRaw("Horizontal");
            if(dashDirection == 0)
            {
                dashDirection = transform.localScale.x;
            }
            rb.velocity = new Vector2(dashDirection * dashForce, rb.velocity.y) * Time.deltaTime;
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

    IEnumerator stopDashing()
    {
        yield return new WaitForSeconds(waitTimeDash);
        canDash = true;
        isDashing = false;

    }


    #region Draw radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(groundCheckPos.position, JumpRadius); // see the radius
    }
    #endregion
}
