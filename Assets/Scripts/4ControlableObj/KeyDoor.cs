using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour {
    public enum KeySelection
    {
        KeyType1,                       
        KeyType2,                         
        KeyType3,
        KeyType4
    }
    public KeySelection KeySelectionType = KeySelection.KeyType1;
    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            if (KeySelectionType == KeySelection.KeyType1)
            { 
                    if (GameObject.Find("Key1(Clone)") != null)
                    {
                        Destroy(gameObject);
                        Destroy(GameObject.Find("Key1(Clone)"));
                    }
              }

            if (KeySelectionType == KeySelection.KeyType2)
            {
                if (GameObject.Find("Key2(Clone)") != null)
                {
                    Destroy(gameObject);
                    Destroy(GameObject.Find("Key2(Clone)"));
                }
            }

            if (KeySelectionType == KeySelection.KeyType3)
            {
                if (GameObject.Find("Key3(Clone)") != null)
                {
                    Destroy(gameObject);
                    Destroy(GameObject.Find("Key3(Clone)"));
                }
            }
            if (KeySelectionType == KeySelection.KeyType4)
            {
                if (GameObject.Find("Key4(Clone)") != null)
                {
                    Destroy(gameObject);
                    Destroy(GameObject.Find("Key4(Clone)"));
                }
            }
        }
    }
}
