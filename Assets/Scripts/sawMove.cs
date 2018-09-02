using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class sawMove : MonoBehaviour
{
    private bool i;
    public Transform LeftPosition;
    public Transform RightPosition;
    public float speed;

    private void Start()
    {
        i = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (i == true)
        {
            transform.localPosition = Vector3.MoveTowards(transform.position, LeftPosition.position, speed * Time.deltaTime);
            if (transform.position == LeftPosition.position)
            {
                i = false;
            }
        }

        if (i == false)
        {
            transform.localPosition = Vector3.MoveTowards(transform.position, RightPosition.position, speed * Time.deltaTime);
            if (transform.position == RightPosition.position)
            {
                i = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Player)
        {
            unit.ReceiveDamage();
        }
    }

}
