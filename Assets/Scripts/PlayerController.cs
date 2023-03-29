using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    public float Speed;


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
    }
}
