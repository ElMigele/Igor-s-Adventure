using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisibility : MonoBehaviour {
    public bool InVisibilityZone;
    public Vector3 PlayerPos;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Игрок в зоне видимости!");
            InVisibilityZone = true;
            PlayerPos = collision.transform.position;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Игрок в зоне видимости!");
            PlayerPos = collision.transform.position;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Игрок вне зоне видимости!");
            InVisibilityZone = false;
            PlayerPos = Vector3.zero;
        }
    }
}
