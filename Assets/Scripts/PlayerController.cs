using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    public float Speed;

    bool isFasingLeft;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
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
    }


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
}
