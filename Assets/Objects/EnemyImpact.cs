using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpact : MonoBehaviour
{
    public bool InImpactZone;
    public Collider2D col;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            col = collision;
            InImpactZone = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            col = collision;
            InImpactZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            col = null;
            InImpactZone = false;
        }
    }
}
