using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LiftButton : MonoBehaviour
{
    public GameObject Door;


    void OnTriggerEnter2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "Box") || (coll.gameObject.tag == "Player") || (coll.gameObject.tag == "Vaza") || (coll.gameObject.tag == "Mumey"))
            Door.gameObject.SetActive(false);
        GetComponent<Renderer>().material.color = Color.green;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "Box") || (coll.gameObject.tag == "Player") || (coll.gameObject.tag == "Vaza") || (coll.gameObject.tag == "Mumey"))
            Door.gameObject.SetActive(true);
        GetComponent<Renderer>().material.color = Color.white;
    }
}