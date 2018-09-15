using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class saw : MonoBehaviour
{
    public GameObject respawn;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && (unit is Player || unit is Vaza))
        {
            unit.ReceiveDamage(25);
        }
    }
}

