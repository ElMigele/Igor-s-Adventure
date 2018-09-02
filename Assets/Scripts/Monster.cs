using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster : Unit
{

    protected virtual void Start() { }
    protected virtual void Update() { }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    { 
        Arrow arrow = collider.gameObject.GetComponent<Arrow>();

        if (arrow)
        {
            ReceiveDamage();
        }

    }


}
