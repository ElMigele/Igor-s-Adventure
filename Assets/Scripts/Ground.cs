using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Ground : MonoBehaviour
{
    public GameObject respawn;
    void OnTriggerEnter2D(Collider2D Ground)
    {
      
        if (Ground.gameObject.CompareTag("arrow"))
        {
            Ground.gameObject.SetActive(false);
            //Add one to the current value of our count variable.
        }



    }


}