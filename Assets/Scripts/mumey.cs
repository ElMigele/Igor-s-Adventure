using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class mumey : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D mumey)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (mumey.gameObject.CompareTag("Button"))
        {
            mumey.gameObject.SetActive(false);
            //Add one to the current value of our count variable.
        }

    }
}