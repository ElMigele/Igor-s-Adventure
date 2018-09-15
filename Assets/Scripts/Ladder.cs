using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {


    public float speed;
    private Rigidbody2D rb;
    public float distance = 1;
    public LayerMask whatIsLadder;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
        Debug.DrawRay(transform.position, distance * Vector2.up);
        
        if (hitInfo.collider != null)
        {
            rb.velocity = speed * new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            rb.gravityScale = 0;
        }
        else {
            rb.gravityScale = 1;
        }
    }
}


