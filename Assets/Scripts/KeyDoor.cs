using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour {

    public GameObject UIKey;

    public PickableObject PickableObject;
    // Use this for initialization


    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            if (PickableObject.assembledKeys0)
            {
                UIKey.SetActive(false);
                Destroy(gameObject);
            }
            if (PickableObject.assembledKeys1)
            {
                UIKey.SetActive(false);
                Destroy(gameObject);
            }
            if (PickableObject.assembledKeys2)
            {
                UIKey.SetActive(false);
                Destroy(gameObject);
            }
            if (PickableObject.assembledKeys3)
            {
                UIKey.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
