using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPositionScript : MonoBehaviour {
    public Transform Respawn;

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            Respawn.position = transform.position;
        }
    }
}
