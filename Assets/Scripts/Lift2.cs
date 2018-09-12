using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift2 : Unit
{
    public bool i;
    public Transform Begin;
    public Transform End;
    public float speed;

    public void Start()
    {
        i = true;
    }
    // Update is called once per frame
    public void Update()
    {
        if (i == true)
        {
            transform.localPosition = Vector3.MoveTowards(transform.position, Begin.position, speed * Time.deltaTime);
            if (transform.position == Begin.position)
            {
                i = false;
            }
        }

        if (i == false)
        {
            transform.localPosition = Vector3.MoveTowards(transform.position, End.position, speed * Time.deltaTime);
            if (transform.position == End.position)
            {
                i = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if ((collider.gameObject.tag == "Box") || (collider.gameObject.tag == "Player"))

        {
            //collider.transform.SetParent(transform, true);
        }
        
    }

    void OnCollisionExit2D(Collision2D collider)
    {
        if ((collider.gameObject.tag == "Box") || (collider.gameObject.tag == "Player"))
        {
            //collider.transform.SetParent(null);
        }
    }
}

