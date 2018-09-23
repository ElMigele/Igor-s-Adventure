using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHoldingRope : MonoBehaviour {
    public GameObject Rammer;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Arrow"))
        {
            Destroy(gameObject);
            Rammer.GetComponent<DestroyDistanceJoint>().Destroy();
        }
    }    
}
