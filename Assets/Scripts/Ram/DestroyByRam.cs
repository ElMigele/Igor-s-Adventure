using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByRam : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Ram"))
        {
            gameObject.SetActive(false);
        }
    }
}
