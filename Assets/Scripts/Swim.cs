using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Swim : MonoBehaviour
{

    public float swimSpeed;
    private Rigidbody2D rb;
    private float inputHorizontal;
    private float inputVertical;
    public float distance;
    public LayerMask whatIsWater;
    private bool isClimbing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputHorizontal * swimSpeed, rb.velocity.y);
        var p = transform.position;
        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector3(p.x, p.y-0.1f, p.z), Vector2.up, distance,whatIsWater);

        if (hitInfo.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                isClimbing = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                isClimbing = false;
            }
        }

        if (isClimbing = true && hitInfo.collider != null)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, inputVertical * swimSpeed);
            rb.gravityScale = 2;
        }
        else
        {
            rb.gravityScale = 5;
        }
    }
}

