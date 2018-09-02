using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Sword : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && (unit is Monster || unit is Vaza))
        {
            unit.ReceiveDamage();
        }
    }
}
