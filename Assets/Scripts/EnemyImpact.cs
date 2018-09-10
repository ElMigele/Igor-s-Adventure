using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpact : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Sword sword = collider.gameObject.GetComponent<Sword>();
        //if (sword)
        //{
        //    if (CanDie)
        //    {
        //        dieCooldown = dieRate;
        //        LivesMonstr--;
        //        Debug.Log(LivesMonstr);
        //    }
        //        if (LivesMonstr == 0)
        //        {
        //            ReceiveDamage();
        //        }
        //    if (LivesMonstr == 1)
        //    {
        //        speed = 4.0F;
        //    }
        //}
        //Arrow arrow = collider.gameObject.GetComponent<Arrow>();
        //if (arrow)
        //{
        //    LivesMonstr--;
        //    if (LivesMonstr == 0)
        //    {
        //        ReceiveDamage();
        //    }
        //    if (LivesMonstr == 1)
        //    {
        //        GetComponent<Renderer>().material.color = Color.red;
        //        speed = 4.0F;
        //    }
        //}
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Player)
        {
            Debug.Log("Удар!");
            unit.ReceiveDamage();
        }
    }
}
