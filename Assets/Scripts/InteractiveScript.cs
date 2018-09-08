using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveScript : MonoBehaviour {
    public enum ObjectType
    {
        LowButton,
        LiftButton,
        HandLever
    }
    public ObjectType InteractObj;
    public GameObject[] DependObjects;
    	
	// Update is called once per frame
	void Update () {
		
	}
    /*
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (InteractObj == ObjectType.LiftButton)
        {
            if ((coll.gameObject.tag == "Box") || (coll.gameObject.tag == "Player") || (coll.gameObject.tag == "Vaza") || (coll.gameObject.tag == "Mumey"))
            {
                GetComponent<Renderer>().material.color = Color.green;
                Lift.speed = 1f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (InteractObj == ObjectType.LiftButton)
        {
            if ((coll.gameObject.tag == "Box") || (coll.gameObject.tag == "Player") || (coll.gameObject.tag == "Vaza") || (coll.gameObject.tag == "Mumey"))
            {
                Lift.speed = 0;
                GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }*/
}
