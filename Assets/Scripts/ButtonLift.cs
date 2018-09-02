using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLift : MonoBehaviour
{
    public Lift2 Lift;
    // Use this for initialization
    void OnTriggerEnter2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "Box") || (coll.gameObject.tag == "Player") || (coll.gameObject.tag == "Vaza") || (coll.gameObject.tag == "Mumey"))
            GetComponent<Renderer>().material.color = Color.green;
        Lift.speed = 1f;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "Box") || (coll.gameObject.tag == "Player") || (coll.gameObject.tag == "Vaza") || (coll.gameObject.tag == "Mumey"))
            Lift.speed = 0;
        GetComponent<Renderer>().material.color = Color.white;
    }
}
